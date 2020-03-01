using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PokemonSwitcher : MonoBehaviour
{
    public static PokemonSwitcher instance;
    public GameObject closePKMN, farPKMN;
    Unit closeUnit, farUnit;
    SquadManager sMan;
    DialogueScript dS;
    BattleSystem bS;
    TrainerManager tM;
    GameObject activePkmnC, activePkmnF;
    public GameObject lightFlash;
    GameObject QueuedPokemon;
    bool initialize = false;

    public BattleHud player1HUD, player2HUD;

    void Start()
    {
        dS = DialogueScript.instance;
        dS.DisplayMessage("Let the battle commence!");

        tM = GetComponent<TrainerManager>();
        DespawnCloseGameObject(tM.trainers[0].gameObject);
        DespawnFarGameObject(tM.trainers[1].gameObject);

        sMan = SquadManager.instance;
        bS = BattleSystem.instance;
    }

    private void SetupBattle()
    {
        ActivatePokemon(startMarkerC, sMan.player1Pokemon[0], true);
        ActivatePokemon(startMarkerF, sMan.player2Pokemon[0], false);
        dS.DisplayMessage("The first matchup! " + bS.player1Pokemon.GetComponent<Unit>().unitName + " VS " + bS.player2Pokemon.GetComponent<Unit>().unitName + "!");
        dS.EnableContinueButton();
    }
    
    public void ActivatePokemon(Transform spawnPoint, GameObject pkmn, bool close)
    {
        DeactivatePokemon();
        GameObject light = Instantiate(lightFlash, spawnPoint);
        Destroy(light, 1.5f);

        pkmn.transform.position = spawnPoint.position;
        pkmn.SetActive(true);

        if (close)
        {
            bS.player1Pokemon = pkmn;
            closePKMN = pkmn;
            player1HUD.SetHud(closePKMN.GetComponent<Unit>());
        }
        else
        {
            bS.player2Pokemon = pkmn;
            farPKMN = pkmn;
            player2HUD.SetHud(farPKMN.GetComponent<Unit>());
        }
    }

    public void DeactivatePokemon()
    {
        if (bS.state == BattleState.START)
            return;
        if(bS.state == BattleState.PLAYER1TURN)
        {
            for (int i = 0; i < sMan.player1Pokemon.Count; i++)
            {
                sMan.player1Pokemon[i].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < sMan.player2Pokemon.Count; i++)
            {
                sMan.player2Pokemon[i].SetActive(false);
            }
        }
    }

    void CheckSpawn()
    {
        if (!initialize)
        {
            initialize = true;
            SetupBattle();
        }
    }
    
    public Transform startMarkerC, startMarkerF;
    public Transform endMarkerC, endMarkerF;
    public float speed;
    private float startTimeC, startTimeF;
    private float journeyLengthC, journeyLengthF;

    void DespawnCloseGameObject(GameObject gOToMove)
    {
        startTimeC = Time.time;
        journeyLengthC = Vector3.Distance(startMarkerC.position, endMarkerC.position);

        StartCoroutine(MoveCloseGuy(gOToMove));
    }

    IEnumerator MoveCloseGuy(GameObject gOToMove)
    {
        while (true)
        {
            float distCovered = (Time.time - startTimeC) * speed;
            
            float fractionOfJourney = distCovered / journeyLengthC;
            
            gOToMove.transform.position = Vector3.Lerp(startMarkerC.position, endMarkerC.position, fractionOfJourney);
            yield return null;
            if (gOToMove.transform.position == endMarkerC.position)
            {
                gOToMove.SetActive(false);
                CheckSpawn();
                yield break;
            }
        }
    }

    void DespawnFarGameObject(GameObject gOToMove)
    {
        startTimeF = Time.time;
        
        journeyLengthF = Vector3.Distance(startMarkerF.position, endMarkerF.position);

        StartCoroutine(MoveFarGuy(gOToMove));
    }

    IEnumerator MoveFarGuy(GameObject gOToMove)
    {
        while (true)
        { 
            float distCovered = (Time.time - startTimeF) * speed;
            
            float fractionOfJourney = distCovered / journeyLengthF;

            gOToMove.transform.position = Vector3.Lerp(startMarkerF.position, endMarkerF.position, fractionOfJourney);
            yield return null;
            if (gOToMove.transform.position == endMarkerF.position)
            {
                gOToMove.SetActive(false);
                CheckSpawn();
                yield break;
            }
        }
    }

    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("Too many PokemonSwitchers!");
            return;
        }
        instance = this;
    }
}
