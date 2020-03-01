using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum BattleState { START, PLAYER1TURN, PLAYER2TURN};

public class BattleSystem : MonoBehaviour
{
    public static BattleSystem instance;
    public GameObject player1Pokemon, player2Pokemon;
    public BattleHud p1BH, p2BH;
    PokemonSwitcher pS;
    DialogueScript dS;
    SquadManager sM;
    TrainerManager tM;
    public Image winningTrainer;
    public GameObject baseButtons;
    public GameObject[] disableOnGameOverUI;
    public GameObject enableOnGOUI;
    public Button pkmnButton;

    public BattleState state;
    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        pS = PokemonSwitcher.instance;
        dS = DialogueScript.instance;
        sM = SquadManager.instance;
        tM = GetComponent<TrainerManager>();
    }

    public void BeginPlayerTurn(int playerNum)
    {
        switch (playerNum)
        {
            case 1:
                state = BattleState.PLAYER1TURN;
                dS.DisplayMessage("Player 1's Turn.");
                break;
            case 2:
                state = BattleState.PLAYER2TURN;
                dS.DisplayMessage("Player 2's Turn.");
                break;
        }
        baseButtons.SetActive(true);
    }

    private void Update()
    {
        player1Pokemon = pS.closePKMN;
        player2Pokemon = pS.farPKMN;

        if (player1Pokemon == null)
            p1BH.gameObject.SetActive(false);
        else
            p1BH.gameObject.SetActive(true);

        if (player2Pokemon == null)
            p2BH.gameObject.SetActive(false);
        else
            p2BH.gameObject.SetActive(true);
    }

    public bool CheckDeadPkmn(List<GameObject> squad)
    {
        for (int i = 0; i < squad.Count; i++)
        {
            Unit pkmn = squad[i].GetComponent<Unit>();
            if (pkmn.fainted == false)
                return false;
        }
        return true;
    }

    public void DealDamage(int damage)
    {
        string effectiveness = " ";
        GameObject targetPkmn;
        GameObject attackingPkmn;
        BattleHud targetBattleHud;
        if (state == BattleState.PLAYER1TURN)
        {
            attackingPkmn = player1Pokemon;
            targetPkmn = player2Pokemon;
            targetBattleHud = p2BH;
        }
        else
        {
            attackingPkmn = player2Pokemon;
            targetPkmn = player1Pokemon;
            targetBattleHud = p1BH;
        }

        Unit targetUnit = targetPkmn.GetComponent<Unit>();

        //if (targetUnit.unitType.weakAgainst.Contains(damageType))
        //{
        //    damage = damage * 2;
        //    effectiveness = " super-effective ";
        //}
        //else if (targetUnit.unitType.strongAgainst.Contains(damageType))
        //{
        //    damage = damage / 2;
        //    effectiveness = " weakened ";
        //}
        targetUnit.currentHP -= damage;
        PkmnGFXScript pkmnAnim = targetPkmn.GetComponentInChildren<PkmnGFXScript>();
        pkmnAnim.TakeHit();


        targetBattleHud.SetHP(targetUnit.currentHP);
        dS.DisplayMessage(attackingPkmn.GetComponent<Unit>().unitName + " dealt " + damage + effectiveness + "damage to " + targetUnit.unitName + "!");
        if(targetUnit.currentHP <= 0)
        {
            dS.furtherInformation = (targetUnit.unitName + " fainted!");
            targetUnit.fainted = true;

            bool p1w = false, p2w = false;
            if (targetPkmn == player1Pokemon)
            {
                p2w = CheckDeadPkmn(sM.player1Pokemon);
            }
            else
            {
                p1w = CheckDeadPkmn(sM.player2Pokemon);
            }
            if (!p1w && !p2w)
                ForceChangePkmn();
            else
            {
                if (p1w)
                    EndBattle(1);
                else if (p2w)
                    EndBattle(2);
            }
        }
        //OPPORTUINITY!! I want to have this continue button call the ForceChangePkmn function instead of have it call instantly above.
        dS.EnableContinueButton();
    }

    public void ForceChangePkmn()
    {
        dS.furtherInformation = "Who will you send in?";
        if (player1Pokemon.GetComponent<Unit>().fainted)
        {
            state = BattleState.PLAYER1TURN;
            pkmnButton.onClick.Invoke();
        }
        if (player2Pokemon.GetComponent<Unit>().fainted)
        {
            state = BattleState.PLAYER2TURN;
            pkmnButton.onClick.Invoke();
        }
    }

    public void EndBattle(int winner)
    {
        foreach (GameObject gO in disableOnGameOverUI)
            gO.SetActive(false);
        enableOnGOUI.SetActive(true);

        Sprite winnerSprite = tM.trainers[winner - 1].GetComponent<Trainer>().trainerFront;
        winningTrainer.sprite = winnerSprite;
    }

    public void RestartBattle()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Too many BattleSystems!");
            return;
        }
        instance = this;
    }

    public IEnumerator DelayTurnSwitch(int player, float delay)
    {
        yield return new WaitForSeconds(delay);

        BeginPlayerTurn(player);
    }
}
