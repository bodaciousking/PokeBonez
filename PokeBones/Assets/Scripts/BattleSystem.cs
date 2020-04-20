//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.SceneManagement;

//public enum BattleState { START, PLAYER1TURN, PLAYER2TURN};

//public class BattleSystem : MonoBehaviour
//{
//    public static BattleSystem instance;
//    public GameObject player1Pokemon, player2Pokemon;
//    PokemonSwitcher pS;
//    DialogueScript dS;
//    //SquadManager sM;
//    TrainerManager tM;
//    public Image winningTrainer;
//    public GameObject baseButtons;
//    public GameObject[] disableOnGameOverUI;
//    public GameObject enableOnGOUI;
//    public Button pkmnButton;

//    public BattleState state;
//    // Start is called before the first frame update
//    void Start()
//    {
//        state = BattleState.START;
//        pS = PokemonSwitcher.instance;
//        dS = DialogueScript.instance;
//       // sM = SquadManager.instance;
//        tM = GetComponent<TrainerManager>();
//    }
//    public void BeginPlayerTurn()
//    {
//        BattleState myState;
//        myState = state;
//        switch (myState)
//        {
//            case BattleState.PLAYER2TURN:
//                state = BattleState.PLAYER1TURN;
//                dS.AddBattleEvent("Player 1's Turn", false, false);
//                break;
//            case BattleState.PLAYER1TURN:
//                state = BattleState.PLAYER2TURN;
//                dS.AddBattleEvent("Player 2's Turn", false, false);
//                break;
//            case BattleState.START:
//                int num = Random.Range(0, 1);
//                if(num == 0)
//                {
//                    state = BattleState.PLAYER1TURN;
//                    dS.AddBattleEvent("Player 1's Turn", false, false);
//                }
//                else if (num == 0)
//                {
//                    state = BattleState.PLAYER2TURN;
//                    dS.AddBattleEvent("Player 2's Turn", false, false);
//                }
//                break;

//        }
//        baseButtons.SetActive(true);
//    }

//    private void Update()
//    {
//        bool run = false;
//        if (run)
//        {
//            player1Pokemon = pS.closePKMN;
//            player2Pokemon = pS.farPKMN;
            
//        }
//    }

//    public bool CheckDeadPkmn(List<GameObject> squad)
//    {
//        for (int i = 0; i < squad.Count; i++)
//        {
//            Unit pkmn = squad[i].GetComponent<Unit>();
//            if (pkmn.fainted == false)
//                return false;
//        }
//        return true;
//    }

//    public void DealDamage(int damage)
//    {
//        string effectiveness = " ";
//        GameObject targetPkmn;
//        GameObject attackingPkmn;
//        if (state == BattleState.PLAYER1TURN)
//        {
//            attackingPkmn = player1Pokemon;
//            targetPkmn = player2Pokemon;
//        }
//        else
//        {
//            attackingPkmn = player2Pokemon;
//            targetPkmn = player1Pokemon;
//        }

//        Unit targetUnit = targetPkmn.GetComponent<Unit>();
        
//        targetUnit.currentHP -= damage;
//        PkmnGFXScript pkmnAnim = targetPkmn.GetComponentInChildren<PkmnGFXScript>();
//        pkmnAnim.TakeHit();


//        //targetBattleHud.SetHP(targetUnit.currentHP);

//        dS.AddBattleEvent(attackingPkmn.GetComponent<Unit>().unitName + " dealt " + damage + effectiveness + "damage to " + targetUnit.unitName + "!", false, true);

//        if(targetUnit.currentHP <= 0)
//        {
//            //dS.furtherInformation = (targetUnit.unitName + " fainted!");
//            targetUnit.fainted = true;

//            bool p1w = false, p2w = false;
//            if (targetPkmn == player1Pokemon)
//            {
//               // p2w = CheckDeadPkmn(sM.player1Pokemon);
//            }
//            else
//            {
//               // p1w = CheckDeadPkmn(sM.player2Pokemon);
//            }
//            if (!p1w && !p2w)
//                ForceChangePkmn();
//            else
//            {
//                if (p1w)
//                    EndBattle(1);
//                else if (p2w)
//                    EndBattle(2);
//            }
//        }

//    }

//    public void ForceChangePkmn()
//    {
//        dS.AddBattleEvent("Who will you send in?", false, false);
//        if (player1Pokemon.GetComponent<Unit>().fainted)
//        {
//            BeginPlayerTurn();
//            pkmnButton.onClick.Invoke();
//        }
//        if (player2Pokemon.GetComponent<Unit>().fainted)
//        {
//            BeginPlayerTurn();
//            pkmnButton.onClick.Invoke();
//        }
//    }

//    public void EndBattle(int winner)
//    {
//        foreach (GameObject gO in disableOnGameOverUI)
//            gO.SetActive(false);
//        enableOnGOUI.SetActive(true);

//        Sprite winnerSprite = tM.trainers[winner - 1].GetComponent<Trainer>().trainerFront;
//        winningTrainer.sprite = winnerSprite;
//    }

//    public void RestartBattle()
//    {
//        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
//    }

//    private void Awake()
//    {
//        if (instance != null)
//        {
//            Debug.Log("Too many BattleSystems!");
//            return;
//        }
//        instance = this;
//    }
    
//}
