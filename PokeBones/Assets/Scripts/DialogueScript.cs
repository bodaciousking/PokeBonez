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
    public bool endTurn = false;
    public int eventQueueCount;

    public Queue<BattleEvent> eventQueue = new Queue<BattleEvent>();
    
    

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
    }

    public void DisplayMessage(string message)
    {
        dialogue.text = message;
    }

    public void OnContinue()
    {
        if (endTurn)
        {
            endTurn = false;
            return;
        }

        NextEvent();
    }

    public void AddBattleEvent(string eventText, bool changeTurn, bool pokemonFainted)
    {
        BattleEvent newBE = new BattleEvent();
        newBE.displayMessage = eventText;
        newBE.changeTurn = changeTurn;
        newBE.pokemonFainted = pokemonFainted;
        eventQueue.Enqueue(newBE);

    }

    public void NextEvent()
    {
        BattleEvent nextEvent = eventQueue.Dequeue();
        DisplayMessage(nextEvent.displayMessage);

        if (nextEvent.changeTurn)
        {
            endTurn = true;
        }

        if (nextEvent.pokemonFainted)
        {
        }
    }
    

    public void Update()
    {
        if (eventQueue.Count > 0 || endTurn)
        {
            continueButton.SetActive(true);
            backButton.SetActive(false);
        }

        else
        {
            continueButton.SetActive(false);
        }
        eventQueueCount = eventQueue.Count;
    }
}

public class BattleEvent
{
    public string displayMessage;
    public bool changeTurn;
    public bool pokemonFainted;

    public BattleEvent()
    {
        displayMessage = "";
        changeTurn = false;
        pokemonFainted = false;
    }
}
