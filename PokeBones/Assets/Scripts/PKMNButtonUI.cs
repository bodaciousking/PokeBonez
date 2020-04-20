//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;

//public class PKMNButtonUI : MonoBehaviour
//{
//    BattleSystem bS;
//    //SquadManager sM;
//    PokemonSwitcher pS;
//    DialogueScript dS;

//    public GameObject mainBattleUI, mainBattleObjects;
//    public Button pkmnButton;
//    // Start is called before the first frame update
//    void Start()
//    {
//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }

//    public void UpdatePokemonButtons()
//    {
//        bS = BattleSystem.instance;
//        //sM = SquadManager.instance;
//        string holderName = "ButtonPanel";
//        if (GameObject.Find(holderName))
//        {
//            if (bS.state == BattleState.PLAYER1TURN)
//            {
//                for (int i = 0; i < sM.player1Pokemon.Count; i++)
//                {
//                    Button newButton = Instantiate(pkmnButton);
//                    newButton.transform.parent = GameObject.Find(holderName).transform;
//                    newButton.transform.localScale = Vector3.one;

//                    Unit pkmn = sM.player1Pokemon[i].GetComponent<Unit>();
//                    newButton.GetComponentInChildren<TextMeshProUGUI>().text = pkmn.unitName;
//                    if (pkmn.fainted)
//                    {
//                        newButton.interactable = false;
//                    }

//                    newButton.onClick.RemoveAllListeners();
//                    newButton.onClick.AddListener(() => OnButtonClick(pkmn, 1));
//                }
//            }
//            else if (bS.state == BattleState.PLAYER2TURN)
//            {
//                for (int i = 0; i < sM.player2Pokemon.Count; i++)
//                {
//                    Button newButton = Instantiate(pkmnButton);
//                    newButton.transform.parent = GameObject.Find(holderName).transform;
//                    newButton.transform.localScale = Vector3.one;

//                    Unit pkmn = sM.player2Pokemon[i].GetComponent<Unit>();
//                    newButton.GetComponentInChildren<TextMeshProUGUI>().text = pkmn.unitName;
//                    if (pkmn.fainted)
//                    {
//                        newButton.interactable = false;
//                    }

//                    newButton.onClick.RemoveAllListeners();
//                    newButton.onClick.AddListener(() => OnButtonClick(pkmn, 2));
//                }
//            }
//        }
//    }

//    public void OnButtonClick(Unit pkmnU, int playerNum)
//    {
//        dS = DialogueScript.instance;
//        pS = PokemonSwitcher.instance;
//        sM = SquadManager.instance;
//        if(bS.player1Pokemon.GetComponent<Unit>().fainted || bS.player2Pokemon.GetComponent<Unit>().fainted)
//        {
//            dS.endTurn = false;
//        }
//        string pkmn;
//        if (playerNum == 1)
//        {
//            if (pkmnU.gameObject != bS.player1Pokemon)
//            {
//                pS.ActivatePokemon(pS.startMarkerC, pkmnU.gameObject, true);
//                pkmn = pkmnU.unitName;
//                dS.DisplayMessage("Go! " + pkmn + "!");
//                dS.furtherInformation = null;
//            }
//            else
//            {
//                dS.DisplayMessage("That pokemon is already in battle!");
//                return;
//            }
//        }
//        else
//        {
//            if (pkmnU.gameObject != bS.player2Pokemon)
//            {
//                pS.ActivatePokemon(pS.startMarkerF, pkmnU.gameObject, false);
//                pkmn = pkmnU.unitName;
//                dS.DisplayMessage("Go! " + pkmn + "!");
//                dS.furtherInformation = null;
//            }
//            else
//            {
//                dS.DisplayMessage("That pokemon is already in battle!");
//                return;
//            }
//        }

//        DeleteAllButtons();

//        mainBattleObjects.SetActive(true);
//        mainBattleUI.SetActive(true);
//        dS.EnableContinueButton();
//    }

//    public void DeleteAllButtons()
//    {
//        foreach (GameObject pkmnButtonD in GameObject.FindGameObjectsWithTag("PkmnButton"))
//        {
//            Destroy(pkmnButtonD);
//        }
//    }
//}
