using UnityEngine;
using UnityEngine.Assertions;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine.Networking;
using NetworkMessages;
using System;
using System.Collections.Generic;
using System.Text;

public class NetworkServer : MonoBehaviour
{
    public NetworkDriver m_Driver;
    public ushort serverPort;
    private NativeList<NetworkConnection> m_Connections;
    public List<NetworkObjects.NetworkPlayer> playersWaiting = new List<NetworkObjects.NetworkPlayer>();
    public List<NetworkObjects.NetworkPlayer> playersConnected = new List<NetworkObjects.NetworkPlayer>();
    public List<OnlineGame> currentGames = new List<OnlineGame>();

    Pokedex2000 myDex;

    public float updateFrequency, updateTimer;

    void Start()
    {
        m_Driver = NetworkDriver.Create();
        var endpoint = NetworkEndPoint.AnyIpv4;
        endpoint.Port = serverPort;
        if (m_Driver.Bind(endpoint) != 0)
            Debug.Log("Failed to bind to port " + serverPort);
        else
            m_Driver.Listen();

        m_Connections = new NativeList<NetworkConnection>(16, Allocator.Persistent);

        myDex = GetComponent<Pokedex2000>();
        foreach (GameObject item in myDex.Pokemon)
        {
            Unit unit = item.GetComponent<Unit>();
            unit.AddTackle();
        }
    }

    void SendToClient(string message, NetworkConnection c)
    {
        var writer = m_Driver.BeginSend(NetworkPipeline.Null, c);
        NativeArray<byte> bytes = new NativeArray<byte>(Encoding.ASCII.GetBytes(message), Allocator.Temp);
        writer.WriteBytes(bytes);
        m_Driver.EndSend(writer);
    }

    public void OnDestroy()
    {
        m_Driver.Dispose();
        m_Connections.Dispose();
    }

    void OnConnect(NetworkConnection c)
    {
        m_Connections.Add(c);
        //Debug.Log("Accepted a connection");

        // Example to send a handshake message:
        HandshakeMsg m = new HandshakeMsg();
        m.player.id = c.InternalId.ToString();
        SendToClient(JsonUtility.ToJson(m), c);
    }

    void OnCreateUser()
    {

    }

    void OnData(DataStreamReader stream, int i)
    {
        NativeArray<byte> bytes = new NativeArray<byte>(stream.Length, Allocator.Temp);
        stream.ReadBytes(bytes);
        string recMsg = Encoding.ASCII.GetString(bytes.ToArray());
        NetworkHeader header = JsonUtility.FromJson<NetworkHeader>(recMsg);

        switch (header.cmd)
        {
            case Commands.HANDSHAKE:
                HandshakeMsg hsMsg = JsonUtility.FromJson<HandshakeMsg>(recMsg);
                playersConnected.Add(hsMsg.player);
                hsMsg.player.id = (playersConnected.Count - 1f).ToString();
                //Debug.Log("Handshake message received on server!");
                break;
            case Commands.PLAYER_UPDATE:
                PlayerUpdateMsg puMsg = JsonUtility.FromJson<PlayerUpdateMsg>(recMsg);
                NetworkObjects.NetworkPlayer thePlayer = puMsg.player;
                NetworkObjects.NetworkPlayer theUpdatePlayer = playersConnected[Int16.Parse(thePlayer.id)];
                //Debug.Log("Player update message received on server!");
                break;
            case Commands.SERVER_UPDATE:
                GameUpdateMsg suMsg = JsonUtility.FromJson<GameUpdateMsg>(recMsg);
                Debug.Log("Game update message received!");
                break;

            case Commands.BATTLE_READY:
                ReadyUpMsg rUMsg = JsonUtility.FromJson<ReadyUpMsg>(recMsg);
                NetworkObjects.NetworkPlayer readyPlayer = (rUMsg.player);
                playersWaiting.Add(readyPlayer);
                
                Debug.Log("Player is ready to fight!");
                break;

            case Commands.LOADEDIN:
                LoadedInMsg lIMsg = JsonUtility.FromJson<LoadedInMsg>(recMsg);
                NetworkObjects.NetworkPlayer player = lIMsg.player;

                OnlineGame playerGame = null;
                foreach (OnlineGame game in currentGames)
                {
                    foreach (NetworkObjects.NetworkPlayer fightingplayer in game.fightingPlayers)
                    {
                        if(fightingplayer.id == player.id)
                        {
                            playerGame = game;
                            break;
                        }
                        if (playerGame != null)
                            break;
                    }
                }
                if (playerGame == null)
                {
                    Debug.Log("Coudlnt find a matching ID to player!");
                    return;
                }
                if (playerGame.fightingPlayers[0].id == player.id)
                {
                    playerGame.player1loaded = true;
                    Debug.Log(playerGame.fightingPlayers[0].playerName + "loaded in!");
                }
                else if (playerGame.fightingPlayers[1].id == player.id)
                {
                    playerGame.player2loaded = true;
                    Debug.Log(playerGame.fightingPlayers[1].playerName + "loaded in!");
                }

                if(playerGame.player1loaded && playerGame.player2loaded)
                {
                    StartGame(playerGame);
                }
                break;

            case Commands.ABILITY:
                AbilityMsg abilityMsg = JsonUtility.FromJson<AbilityMsg>(recMsg);
                Ability usedAbility = null;
                usedAbility =  myDex.Pokemon[abilityMsg.attackingUnit].GetComponent<Unit>().AbilitiesList[0];

                OnlineGame playersGame = null;
                foreach (OnlineGame game in currentGames)
                {
                    foreach (NetworkObjects.NetworkPlayer fightingplayer in game.fightingPlayers)
                    {
                        if (fightingplayer.id == abilityMsg.playerID)
                        {
                            playersGame = game;
                            break;
                        }
                        if (playersGame != null)
                            break;
                    }
                }
                if(playersGame==null)
                {
                    Debug.Log("oop! some1 fukd up");
                    break;
                }
                Debug.Log("I'm comparing " + playersGame.p1Current.unitName );
                Debug.Log(" to " + myDex.Pokemon[abilityMsg.attackingUnit].GetComponent<Unit>().unitName);
                if (playersGame.p1Current.unitID == myDex.Pokemon[abilityMsg.attackingUnit].GetComponent<Unit>().unitID)
                {
                    Debug.Log(playersGame.p1Current.unitName + " is attacking.");
                    if (playersGame.p1Current.fainted)
                    {
                        Debug.Log("But it's fainted!");
                        return;
                    }
                    if (usedAbility.dealsDamage)
                    {
                        //Deal damage to p2
                        playersGame.p2Current.currentHP -= usedAbility.abilityDamage;
                        UpdateBothHuds(playersGame);
                        if (playersGame.p2Current.currentHP <= 0)
                        {
                            playersGame.p2Current.fainted = true;
                            for (int z = 0; z < playersGame.p2Units.Count; z++)
                            {
                                if (CheckDeadPkmn(playersGame.p2Units))
                                {
                                    EndGame(playersGame, playersGame.fightingPlayers[0]);
                                }
                                else if (!playersGame.p2Units[z].fainted)
                                {
                                    playersGame.p2Current = playersGame.p2Units[z];
                                    DisplayBothPlayersPkmn(playersGame);
                                }
                                
                            }
                        }
                        StartTurn(playersGame, playersGame.fightingPlayers[1]);
                    }
                }

                else if (playersGame.p2Current.unitID == myDex.Pokemon[abilityMsg.attackingUnit].GetComponent<Unit>().unitID)
                {
                    Debug.Log(playersGame.p2Current.unitName + " is attacking.");
                    if (playersGame.p2Current.fainted)
                    {
                        Debug.Log("But it's fainted!");
                        return;
                    }
                    if (usedAbility.dealsDamage)
                    {
                        //Deal damage to p1
                        playersGame.p1Current.currentHP -= usedAbility.abilityDamage;
                        UpdateBothHuds(playersGame);

                        if(playersGame.p1Current.currentHP <= 0)
                        {
                            playersGame.p1Current.fainted = true;
                            for (int z = 0; z < playersGame.p1Units.Count + 1; z++)
                            {
                                if (z < playersGame.p1Units.Count)
                                    if (!playersGame.p1Units[z].fainted)
                                    {
                                        playersGame.p1Current = playersGame.p1Units[z];
                                        DisplayBothPlayersPkmn(playersGame);
                                    }
                                if (z == playersGame.p1Units.Count + 1)
                                {
                                    EndGame(playersGame, playersGame.fightingPlayers[1]);
                                }
                            }
                        }
                        StartTurn(playersGame, playersGame.fightingPlayers[0]);
                    }
                }

                SendNextEventMessage(playersGame.fightingPlayers[0]);
                SendNextEventMessage(playersGame.fightingPlayers[1]);
                break;  
            default:
                Debug.Log("SERVER ERROR: Unrecognized message received!");
                break;
        }
    }

    void StartGame(OnlineGame oG)
    {
        InitializeUnits(oG);
        DisplayBothPlayersPkmn(oG);

        int coinFlip = UnityEngine.Random.Range(0, 1);
        NetworkObjects.NetworkPlayer playerGoingFirst = oG.fightingPlayers[coinFlip];
        Debug.Log(playerGoingFirst.playerName + " is going first.");
        if (coinFlip == 0)
            oG.currentTurn = TurnState.PLAYER1TURN;
        if (coinFlip == 1)
            oG.currentTurn = TurnState.PLAYER2TURN;

        StartTurn(oG, playerGoingFirst);
    }


    void InitializeUnits(OnlineGame oG)
    {
        oG.p1Units = new List<Unit>();
        oG.p2Units = new List<Unit>();
        string gameHolderName = oG.name + " Data";
        GameObject gameHolderObject = new GameObject();
        gameHolderObject.name = gameHolderName;
        

        for (int i = 0; i < oG.fightingPlayers.Count; i++) // Initialize for each player
        {
            SendDialogueMessage("Let the battle commence!", oG.fightingPlayers[i]);
            SendNextEventMessage(oG.fightingPlayers[i]);

            string holderName = "Player #" + (i+1) + "'s Pokemon";
            GameObject holderObject = new GameObject();
            holderObject.name = holderName;
            holderObject.transform.parent = gameHolderObject.transform;

            NetworkObjects.NetworkPlayer player = oG.fightingPlayers[i];

            for (int x = 0; x < player.selectedPokemon.Length; x++) // Initialize the three pokemon
            {
                char c = player.selectedPokemon[x];
                int y = int.Parse(c.ToString());

                GameObject pk = Instantiate(GetComponent<Pokedex2000>().Pokemon[y]);
                Unit pkUnit = pk.GetComponent<Unit>();
                pkUnit.Init();
                if (i == 0)
                {
                    oG.p1Units.Add(pkUnit);
                }
                if (i == 1)
                {
                    oG.p2Units.Add(pkUnit);
                }
                pk.transform.parent = holderObject.transform;
                if (x == 0 && i == 0)
                {
                    oG.p1Current = pkUnit;
                }
                else if (x == 0 && i == 1)
                    oG.p2Current = pkUnit;
            }
            if (i == 0) //Populate the client's list of pokeomn with the list we just generated;
            {
                PopulatePkmnMsg ppMsg = new PopulatePkmnMsg(); 
                ppMsg.pkmn = oG.p1Units;

                int playerID = int.Parse(player.id);
                SendToClient(JsonUtility.ToJson(ppMsg), m_Connections[playerID]);
            }
            if (i == 1) //Populate the client's list of pokeomn with the list we just generated;
            {
                PopulatePkmnMsg ppMsg = new PopulatePkmnMsg();
                ppMsg.pkmn = oG.p2Units;

                int playerID = int.Parse(player.id);
                SendToClient(JsonUtility.ToJson(ppMsg), m_Connections[playerID]);
            }
        }
    }
    

    void DisplayBothPlayersPkmn(OnlineGame oG)
    {
        Debug.Log(oG.name);
        for (int i = 0; i < oG.fightingPlayers.Count; i++) // Do this for both players
        {
            int clientDisplayID = 0;
            int enemyDisplayID = 0;
            if (i == 0)
            {
                clientDisplayID = oG.p1Current.unitID;
                enemyDisplayID = oG.p2Current.unitID;
            }
            if (i == 1)
            {
                clientDisplayID = oG.p2Current.unitID;
                enemyDisplayID = oG.p1Current.unitID;
            }

            NetworkObjects.NetworkPlayer player = oG.fightingPlayers[i]; DisplayGfxMsg displayClientPkmn = new DisplayGfxMsg();
            Debug.Log(player.playerName);
            Pokedex2000 pkd = GetComponent<Pokedex2000>();
           
            int obj = clientDisplayID;
            displayClientPkmn.thingToDisplay = obj;
            displayClientPkmn.clientPlayer = true;

            int playerID = int.Parse(player.id);
            SendToClient(JsonUtility.ToJson(displayClientPkmn), m_Connections[playerID]); // Player 1's Gfx displayed by Player 

            foreach (NetworkObjects.NetworkPlayer networkPlayer in oG.fightingPlayers)
            {
                if (networkPlayer.id != player.id)
                {
                    player.myOpponent = networkPlayer;
                }
            }

            DisplayGfxMsg displayEnemyPkmn = new DisplayGfxMsg();
            int eObj = enemyDisplayID;
            displayEnemyPkmn.thingToDisplay = eObj;
            displayEnemyPkmn.clientPlayer = false;

            SendToClient(JsonUtility.ToJson(displayEnemyPkmn), m_Connections[playerID]); // Player 2's Gfx displayed by Player
        }
        UpdateBothHuds(oG);
    }

    public void UpdateBothHuds(OnlineGame oG)
    {
        for (int i = 0; i < oG.fightingPlayers.Count; i++)
        {
            if (i == 0) //Update Player 1's UI
            {
                Debug.Log("Updating P1 " + oG.p1Current);
                SendUpdateHUDMessage(oG.p1Current, true, oG.fightingPlayers[i]);
                SendUpdateHUDMessage(oG.p2Current, false, oG.fightingPlayers[i]);
            }
            if (i == 1) //Update Player 2's UI
            {
                SendUpdateHUDMessage(oG.p1Current, false, oG.fightingPlayers[i]);
                SendUpdateHUDMessage(oG.p2Current, true, oG.fightingPlayers[i]);
            }
        }
    }

    public void StartTurn(OnlineGame oG, NetworkObjects.NetworkPlayer player)
    {
        for (int i = 0; i < oG.fightingPlayers.Count; i++)
        {
            if(player.id == oG.fightingPlayers[i].id)
            {
                SendDialogueMessage("Your turn!", oG.fightingPlayers[i]);
                BeginTurnMsg bTMsg = new BeginTurnMsg();
                int playerID = int.Parse(player.id);
                SendToClient(JsonUtility.ToJson(bTMsg), m_Connections[playerID]);
            }
            else 
            {
                SendDialogueMessage("Opponent's turn.", oG.fightingPlayers[i]);
            }
        }
    }

    public bool CheckDeadPkmn(List<Unit> squad)
    {
        for (int i = 0; i < squad.Count; i++)
        {
            Unit pkmn = squad[i];
            if (pkmn.fainted == false)
                return false;
        }
        return true;
    }

    public void EndGame(OnlineGame oG, NetworkObjects.NetworkPlayer winner)
    {
        Debug.Log(winner.playerName + "has won " + oG.name);
        VictoryMsg vMsg = new VictoryMsg();
        int playerID = int.Parse(winner.id);
        SendToClient(JsonUtility.ToJson(vMsg), m_Connections[playerID]);


        NetworkObjects.NetworkPlayer loser;
        loser = winner.myOpponent;
        Debug.Log(loser.playerName + "has lost " + oG.name);
        DefeatMsg dMsg = new DefeatMsg();
        int player2ID = int.Parse(loser.id);
        SendToClient(JsonUtility.ToJson(dMsg), m_Connections[player2ID]);

        currentGames.Remove(oG);

    }

    void SendDialogueMessage(string message, NetworkObjects.NetworkPlayer player)
    {
        DisplayDialogueMsg dMsg = new DisplayDialogueMsg();
        dMsg.dialogueString = message;

        int playerID = int.Parse(player.id);
        SendToClient(JsonUtility.ToJson(dMsg), m_Connections[playerID]);
    }

    void SendNextEventMessage(NetworkObjects.NetworkPlayer player)
    {
        NextEventMessage nEMsg = new NextEventMessage();

        int playerID = int.Parse(player.id);
        SendToClient(JsonUtility.ToJson(nEMsg), m_Connections[playerID]);

        Debug.Log("Sent a Next Event message to " + player.playerName);
    }

    void SendUpdateHUDMessage(Unit pkmn, bool clientSidePkmn, NetworkObjects.NetworkPlayer player)
    {
        UpdateHUDMsg uHMsg = new UpdateHUDMsg();
        uHMsg.pkmnName = pkmn.unitName;
        uHMsg.currentHealth = pkmn.currentHP;
        uHMsg.maxHealth = pkmn.maxHP;
        uHMsg.level = pkmn.unitLevel;
        uHMsg.unitID = pkmn.unitID;
        
        uHMsg.closePkmn = clientSidePkmn;

        int playerID = int.Parse(player.id);
        SendToClient(JsonUtility.ToJson(uHMsg), m_Connections[playerID]);

        Debug.Log("Sent an Update Hud with " + uHMsg.pkmnName + " message to " + player.playerName);
    }

    void OnDisconnect(int i)
    {
        Debug.Log("Client disconnected from server");
        m_Connections[i] = default(NetworkConnection);
    }

    void OnUpdate()
    {
        if (playersWaiting.Count >= 2)
        {
            //This needs to be modified for multiple concurrent games/lobbies
            bool canStart = false;
            if (playersWaiting.Count == 2)           
            {
                canStart = true;
            }
            else canStart = false;
            if (canStart)
            {
                OnlineGame newGame = new OnlineGame();
                newGame.name = "Game #" + (currentGames.Count + 1);
                currentGames.Add(newGame);
                newGame.fightingPlayers = new List<NetworkObjects.NetworkPlayer>();
                for (int i = 0; i < 2; i++)
                {
                    int playerID = int.Parse(playersWaiting[i].id);
                    newGame.fightingPlayers.Add(playersConnected[playerID]);
                }
                foreach (NetworkObjects.NetworkPlayer player in newGame.fightingPlayers)
                {
                    for (int i = 0; i < playersWaiting.Count; i++)
                    {
                        if (playersWaiting[i].id == player.id)
                        {
                            playersWaiting.Remove(playersWaiting[i]);
                        }
                    }
                    GameUpdateMsg gUM = new GameUpdateMsg();
                    gUM.game = newGame;
                    SendToClient(JsonUtility.ToJson(gUM), m_Connections[int.Parse(player.id)]);
                }
                NotifyPlayers(newGame);
            }
        }


        if (playersConnected.Count > 0)
            for (int i = 0; i < playersConnected.Count; i++)
            {
                NetworkObjects.NetworkPlayer player;
                player = playersConnected[i];
                GameUpdateMsg m = new GameUpdateMsg();
                SendToClient(JsonUtility.ToJson(m), m_Connections[i]);
            }
    }

    void NotifyPlayers(OnlineGame game)
    {
        foreach (NetworkObjects.NetworkPlayer player in game.fightingPlayers)
        {
            BeginBattleMessage bbMsg = new BeginBattleMessage();

            int playerID = int.Parse(player.id);
            SendToClient(JsonUtility.ToJson(bbMsg), m_Connections[playerID]);
        }
    }

    void Update()
    {
        m_Driver.ScheduleUpdate().Complete();

        // CleanUpConnections
        for (int i = 0; i < m_Connections.Length; i++)
        {
            if (!m_Connections[i].IsCreated)
            {

                m_Connections.RemoveAtSwapBack(i);
                --i;
            }
        }

        // AcceptNewConnections
        NetworkConnection c = m_Driver.Accept();
        while (c != default(NetworkConnection))
        {
            OnConnect(c);

            // Check if there is another new connection
            c = m_Driver.Accept();
        }


        // Read Incoming Messages
        DataStreamReader stream;
        for (int i = 0; i < m_Connections.Length; i++)
        {
            Assert.IsTrue(m_Connections[i].IsCreated);

            NetworkEvent.Type cmd;
            cmd = m_Driver.PopEventForConnection(m_Connections[i], out stream);
            while (cmd != NetworkEvent.Type.Empty)
            {
                if (cmd == NetworkEvent.Type.Data)
                {
                    OnData(stream, i);
                }
                else if (cmd == NetworkEvent.Type.Disconnect)
                {
                    OnDisconnect(i);
                }

                cmd = m_Driver.PopEventForConnection(m_Connections[i], out stream);
            }
        }


        updateTimer += Time.deltaTime;
        if (updateTimer >= updateFrequency)
        {
            OnUpdate();
            updateTimer = 0f;
        }
    }
}

public enum TurnState {  START, PLAYER1TURN, PLAYER2TURN, BETWEENTURNS}
[System.Serializable]
public class OnlineGame
{
    public string name;
    public List<NetworkObjects.NetworkPlayer> fightingPlayers;
    public bool player1loaded, player2loaded;

    public TurnState currentTurn;
    public List<Unit> p1Units;
    public List<Unit> p2Units;
    public Unit p1Current, p2Current;

}