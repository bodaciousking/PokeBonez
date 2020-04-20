using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace NetworkMessages
{
    public enum Commands
    {
        PLAYER_UPDATE,
        SERVER_UPDATE,
        HANDSHAKE,
        BATTLE_READY,
        PLAYER_INPUT,
        LOGIN,
        NEWUSER,
        BEGINBATTLE,
        LOADEDIN,
        DISPLAYGFX,
        DISPLAYDIALOGUE,
        UPDATEHUD,
        POPULATEPOKEMON,
        BEGINTURN,
        ABILITY,
        NEXTEVENT,
        VICTORY, 
        DEFEAT
    }

    [System.Serializable]
    public class NetworkHeader
    {
        public Commands cmd;
    }

    [System.Serializable]
    public class HandshakeMsg : NetworkHeader
    {
        public NetworkObjects.NetworkPlayer player;
        public HandshakeMsg()
        {      // Constructor
            cmd = Commands.HANDSHAKE;
            player = new NetworkObjects.NetworkPlayer();
        }
    }

    [System.Serializable]
    public class ReadyUpMsg : NetworkHeader
    {
        public NetworkObjects.NetworkPlayer player;
        public ReadyUpMsg()
        {      // Constructor
            cmd = Commands.BATTLE_READY;
            player = new NetworkObjects.NetworkPlayer();
        }
    }

    [System.Serializable]
    public class PlayerUpdateMsg : NetworkHeader
    {
        public NetworkObjects.NetworkPlayer player;
        public PlayerUpdateMsg()
        {      // Constructor
            cmd = Commands.PLAYER_UPDATE;
            player = new NetworkObjects.NetworkPlayer();
        }
    };

    public class PlayerInputMsg : NetworkHeader
    {
        public Input myInput;
        public PlayerInputMsg()
        {
            cmd = Commands.PLAYER_INPUT;
            myInput = new Input();
        }
    }

    [System.Serializable]
    public class GameUpdateMsg : NetworkHeader
    {
        public OnlineGame game;
        public GameUpdateMsg()
        {      // Constructor
            cmd = Commands.SERVER_UPDATE;
            game = new OnlineGame();
        }
    }

    [System.Serializable]
    public class LoginRequestMsg : NetworkHeader
    {
        public string username;
        public string password;

        public LoginRequestMsg()
        {      // Constructor
            cmd = Commands.LOGIN;
            username = " ";
            password = " ";
        }
    }
    [System.Serializable]
    public class NewUserRequestMsg : NetworkHeader
    {
        public string username;
        public string password;

        public NewUserRequestMsg()
        {      // Constructor
            cmd = Commands.LOGIN;
            username = " ";
            password = " ";
        }
    }
    [System.Serializable]
    public class BeginBattleMessage : NetworkHeader
    {
        public BeginBattleMessage()
        {
            cmd = Commands.BEGINBATTLE;
        }
    }
    [System.Serializable]
    public class LoadedInMsg : NetworkHeader
    {
        public NetworkObjects.NetworkPlayer player;
        public LoadedInMsg()
        {
            cmd = Commands.LOADEDIN;
            player = new NetworkObjects.NetworkPlayer();
        }
    }

    [System.Serializable]
    public class DisplayGfxMsg : NetworkHeader
    {
        public bool clientPlayer;
        public int thingToDisplay;
        public DisplayGfxMsg()
        {
            cmd = Commands.DISPLAYGFX;

        }
    }
    [System.Serializable]
    public class DisplayDialogueMsg : NetworkHeader
    {
        public string dialogueString;
        public DisplayDialogueMsg()
        {
            cmd = Commands.DISPLAYDIALOGUE;
        }
    }
    [System.Serializable]
    public class UpdateHUDMsg : NetworkHeader
    {
        public string pkmnName;
        public int currentHealth;
        public int maxHealth;
        public int level;
        public int unitID;

        public bool closePkmn;

        public UpdateHUDMsg()
        {
            cmd = Commands.UPDATEHUD;
        }
    }
    [System.Serializable]
    public class PopulatePkmnMsg : NetworkHeader
    {
        public List<Unit> pkmn;
        public PopulatePkmnMsg()
        {
            cmd = Commands.POPULATEPOKEMON;
            pkmn = new List<Unit>();
        }
    }
    [System.Serializable]
    public class BeginTurnMsg : NetworkHeader
    {
        public BeginTurnMsg()
        {
            cmd = Commands.BEGINTURN;
        }
    }
    [System.Serializable]
    public class AbilityMsg : NetworkHeader
    {
        public int attackingUnit;
        public int abilityIndexed;
        public string playerID;

        public AbilityMsg()
        {
            cmd = Commands.ABILITY;
        }
    }
    [System.Serializable]
    public class NextEventMessage : NetworkHeader
    {
        public NextEventMessage()
        {
            cmd = Commands.NEXTEVENT;
        }
    }
    [System.Serializable]
    public class VictoryMsg : NetworkHeader
    {
        public VictoryMsg()
        {
            cmd = Commands.VICTORY;
        }
    }
    [System.Serializable]
    public class DefeatMsg : NetworkHeader
    {
        public DefeatMsg()
        {
            cmd = Commands.DEFEAT;
        }
    }
}

namespace NetworkObjects
{
    [System.Serializable]
    public class NetworkObject
    {
        public string id;
    }
    [System.Serializable]
    public class NetworkPlayer : NetworkObject
    {
        public string selectedPokemon;

        public string playerName;
        public int playerSkill;
        public int playerRank;
        public GameObject playerTrainer;
        public NetworkObjects.NetworkPlayer myOpponent;
        // Have the lambda functions that populate these parameters be called BEFORE the handshake message and AFTER the constructor is called
        public NetworkPlayer()
        {
            //selectedPokemon = "1";
            //playerName = "missingNo";
            //playerSkill = 0;
            //playerRank = 0;
            //playerTrainer = new Trainer();
        }
    }
    //[System.Serializable]
    //public class PlayerCard : NetworkObject
    //{
    //    public player
    //}
}


