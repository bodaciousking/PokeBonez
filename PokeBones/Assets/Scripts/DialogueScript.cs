using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueScript : MonoBehaviour
{
    public TextMeshProUGUI dialogue;

    public static DialogueScript instance;
    public GameObject continueButton, backButton;
    BattleSystem bS;
    public bool endTurn = true;
    

    public string furtherInformation = null;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Too many dialogue managers!");
            return;
        }
        instance = this;
    }

    private void Start()
    {
        bS = BattleSystem.instance;
        furtherInformation = null;
    }

    public void DisplayMessage(string message)
    {
        dialogue.text = message;
    }

    public void Continue()
    {
        if (furtherInformation != null)
        {
            dialogue.text = furtherInformation;
            furtherInformation = null;
        }
        else
        {
            continueButton.SetActive(false);
            if (bS.player1Pokemon.GetComponent<Unit>().fainted || bS.player2Pokemon.GetComponent<Unit>().fainted)
            {
                return;
            }
            if (endTurn)
            {
                if (bS.state == BattleState.PLAYER1TURN)
                    bS.BeginPlayerTurn(2);
                else
                    bS.BeginPlayerTurn(1);
            }
            else
            {
                endTurn = true;
                if (bS.state == BattleState.PLAYER1TURN)
                    bS.BeginPlayerTurn(1);
                else
                    bS.BeginPlayerTurn(2);
                return;
            }
        }

    }

    public void EnableContinueButton()
    {
        continueButton.SetActive(true);
        backButton.SetActive(false);
    }

    public void Update()
    {
        if (furtherInformation != null)
        {
            continueButton.SetActive(true);
        }
    }
}
