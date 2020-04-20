using UnityEngine;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine.SceneManagement;
using NetworkMessages;
using NetworkObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UI;

public class NetworkClient : MonoBehaviour
{
    public NetworkDriver m_Driver;
    public NetworkConnection m_Connection;
    bool connected = false;
    bool loadedIn, atPostGame;
    public bool testClient, loadScene;
    public bool wonLastGame;

    public string serverIP;
    public ushort serverPort;
    public NetworkObjects.NetworkPlayer myPlayer;

    GraphicsScript gS;
    GameObject graphicsManager;
    GameObject newGM;
    GameObject baseButtons;
    public GameObject readyUpButton;

    public GameObject victoryStuff, defeatStuff;

    public List<Unit> clientPkmn;
    

    void Start()
    {
        m_Driver = NetworkDriver.Create();
        m_Connection = default(NetworkConnection);
        //myPlayer = new NetworkObjects.NetworkPlayer();
        DontDestroyOnLoad(this);
        clientPkmn = new List<Unit>();
        if (testClient)
            StartConnecting();
    }

    public void StartConnecting()
    {
        var endpoint = NetworkEndPoint.Parse(serverIP, serverPort);
        m_Connection = m_Driver.Connect(endpoint);
    }

    public void ReadyUp()
    {
        ReadyUpMsg rd = new ReadyUpMsg();
        rd.player = myPlayer;
        SendToServer(JsonUtility.ToJson(rd));
    }

    public void EnableReadyUp()
    {
        readyUpButton.SetActive(true);
    }

    public void SendToServer(string message)
    {
        var writer = m_Driver.BeginSend(m_Connection);
        NativeArray<byte> bytes = new NativeArray<byte>(Encoding.ASCII.GetBytes(message), Allocator.Temp);
        writer.WriteBytes(bytes);
        m_Driver.EndSend(writer);
    }

    public void UseAbility(Unit unit)
    {
        AbilityMsg m = new AbilityMsg();
        m.attackingUnit = unit.unitID;
        m.playerID = myPlayer.id;
        SendToServer(JsonUtility.ToJson(m));
    }

    void OnConnect()
    {
        // Debug.Log("We are now connected to the server");

        // Example to send a handshake message:
        HandshakeMsg m = new HandshakeMsg();
        m.player = myPlayer;
        m.player.id = m_Connection.InternalId.ToString();
        SendToServer(JsonUtility.ToJson(m));

        connected = true;
        if(!testClient)
            EnableReadyUp();
        if (testClient)
            ReadyUp();
    }

    void OnData(DataStreamReader stream)
    {
        NativeArray<byte> bytes = new NativeArray<byte>(stream.Length, Allocator.Temp);
        stream.ReadBytes(bytes);
        string recMsg = Encoding.ASCII.GetString(bytes.ToArray());
        NetworkHeader header = JsonUtility.FromJson<NetworkHeader>(recMsg);

        switch (header.cmd)
        {
            case Commands.HANDSHAKE:
                HandshakeMsg hsMsg = JsonUtility.FromJson<HandshakeMsg>(recMsg);
                myPlayer.id = hsMsg.player.id;
               
                break;

            case Commands.PLAYER_UPDATE:
                PlayerUpdateMsg puMsg = JsonUtility.FromJson<PlayerUpdateMsg>(recMsg);
                Debug.Log("Player update message received!");
                break;

            case Commands.SERVER_UPDATE:
                GameUpdateMsg suMsg = JsonUtility.FromJson<GameUpdateMsg>(recMsg);
                //Debug.Log("Game update message received!");
                break;
            case Commands.BEGINBATTLE:
                BeginBattleMessage bbMsg = JsonUtility.FromJson<BeginBattleMessage>(recMsg);
                if(loadScene)
                SceneManager.LoadScene("BattleScene");
                //Debug.Log("Begin Battle message received!");
                break;

            case Commands.DISPLAYGFX:
                DisplayGfxMsg dgm = JsonUtility.FromJson<DisplayGfxMsg>(recMsg);
                gS = GameObject.Find("newGM").GetComponent<GraphicsScript>();
                if(loadScene)
                gS.DisplayGraphic(dgm.thingToDisplay, dgm.clientPlayer);
                //Debug.Log("Display Gfx Message recieved!");
                break;

            case Commands.DISPLAYDIALOGUE:
                DisplayDialogueMsg dMsg = JsonUtility.FromJson<DisplayDialogueMsg>(recMsg);
                newGM = GameObject.Find("newGM");
                DialogueScript dS = newGM.GetComponent<DialogueScript>();
                if(loadScene)
                dS.AddBattleEvent(dMsg.dialogueString, false, false);
                break;
            case Commands.UPDATEHUD:
                UpdateHUDMsg uHMsg = JsonUtility.FromJson<UpdateHUDMsg>(recMsg);
                newGM = GameObject.Find("newGM");
                HudUpdater hU = newGM.GetComponent<HudUpdater>();
                hU.UpdateBattleHUD(uHMsg.pkmnName, uHMsg.currentHealth, uHMsg.maxHealth, uHMsg.level, uHMsg.unitID, uHMsg.closePkmn);
                break;
            case Commands.POPULATEPOKEMON:
                PopulatePkmnMsg pPMsg = JsonUtility.FromJson<PopulatePkmnMsg>(recMsg);
                clientPkmn = pPMsg.pkmn;
                break;

            case Commands.BEGINTURN:
                BeginTurnMsg bTMsg = JsonUtility.FromJson<BeginTurnMsg>(recMsg);
                if (loadScene)
                    baseButtons.SetActive(true);
                if (!loadScene)
                    UseAbility(GameObject.Find("newGM").GetComponent<HudUpdater>().p2current);
                break;
            case Commands.NEXTEVENT:
                newGM = GameObject.Find("newGM");
                if (loadScene)
                {
                    DialogueScript dSc = newGM.GetComponent<DialogueScript>();
                    dSc.OnContinue();
                }
                break;
            case Commands.VICTORY:
                Debug.Log(myPlayer.playerName+ " won");
                wonLastGame = true;
                loadedIn = false;
                if(loadScene) StartCoroutine("LoadLevel");
                break;
            case Commands.DEFEAT:
                Debug.Log(myPlayer.playerName + " lost");
                wonLastGame = false;
                loadedIn = false;
                if (loadScene) StartCoroutine("LoadLevel");
                break;
            default:
                Debug.Log("Unrecognized message received!");
                break;
        }
    }

    void Disconnect()
    {
        m_Connection.Disconnect(m_Driver);
        m_Connection = default(NetworkConnection);
    }

    void OnDisconnect()
    {
        Debug.Log("Client got disconnected from server");
        m_Connection = default(NetworkConnection);
    }

    public void OnDestroy()
    {
        m_Driver.Dispose();
    }

    public void OnUpdate()
    {
        PlayerUpdateMsg m = new PlayerUpdateMsg();
        m.player = myPlayer;
        SendToServer(JsonUtility.ToJson(m));
    }

    void Update()
    {
        m_Driver.ScheduleUpdate().Complete();

        if (!m_Connection.IsCreated)
        {
            return;
        }

        DataStreamReader stream;
        NetworkEvent.Type cmd;
        cmd = m_Connection.PopEvent(m_Driver, out stream);

        while (cmd != NetworkEvent.Type.Empty)
        {
            if (cmd == NetworkEvent.Type.Connect)
            {
                OnConnect();
            }
            else if (cmd == NetworkEvent.Type.Data)
            {
                OnData(stream);
            }
            else if (cmd == NetworkEvent.Type.Disconnect)
            {
                OnDisconnect();
            }

            cmd = m_Connection.PopEvent(m_Driver, out stream);
        }

        if (connected)
        {
            OnUpdate();
            
            
            if (!loadedIn && !atPostGame && GameObject.Find("LoadingDoneObj"))
            {
                loadedIn = true;
                LoadedInMsg m = new LoadedInMsg();
                m.player = myPlayer;
                SendToServer(JsonUtility.ToJson(m));

                if (loadScene)
                {
                    baseButtons = GameObject.Find("BaseButtons");
                    baseButtons.SetActive(false);
                }
                atPostGame = false;

                newGM = GameObject.Find("newGM");
            }
        }
    }

    public void PostGameStuff()
    {
        atPostGame = true;

        victoryStuff = GameObject.Find("VictoryStuff");
        victoryStuff.SetActive(wonLastGame);

        defeatStuff = GameObject.Find("DefeatStuff");
        defeatStuff.SetActive(!wonLastGame);

        Button YesButton = GameObject.Find("ReadyUpButton").GetComponent<Button>();
        YesButton.onClick.RemoveAllListeners();
        YesButton.onClick.AddListener(() => ReadyUp());

        GameObject lambdaObj = GameObject.Find("LambdaScoreUpdater");
        ScoreUpdater sU = lambdaObj.GetComponent<ScoreUpdater>();

        sU.userID = myPlayer.playerName;
        if (wonLastGame)
            sU.score = 5;
        if (!wonLastGame)
            sU.score = -5;

        sU.StartCoroutine("UpdateScore");


        loadedIn = false;
    }

    AsyncOperation asyncLoadLevel;

    IEnumerator LoadLevel()
    {
        asyncLoadLevel = SceneManager.LoadSceneAsync("PostGameScene", LoadSceneMode.Single);
        while (!asyncLoadLevel.isDone)
        {
            yield return null;
        }

        PostGameStuff();
    }
}