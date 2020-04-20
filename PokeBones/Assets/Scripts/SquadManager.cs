//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class SquadManager : MonoBehaviour
//{
//    public List<GameObject> player1Pokemon = new List<GameObject>();
//    public List<GameObject> player2Pokemon = new List<GameObject>();
//    SelectionManager sM;
//    Pokedex pX;
//    public static SquadManager instance;

//    private void Awake()
//    {
//        if (instance != null)
//        {
//            Debug.Log("Too many SquadManagers!");
//            return;
//        }
//        instance = this;
//    }

//    void Start()
//    {
//        if (SelectionManager.instance != null)
//            sM = SelectionManager.instance;
//        pX = Pokedex.instance;

//        //PopulatePlayer1Team();
//        //PopulatePlayer2Team();
//    }

//    void PopulatePlayer1Team()
//    {
//        if (sM)
//            for (int i = 0; i < sM.selectedPokemon.Count; i++)
//            {
//                GameObject newPkmn = Instantiate(pX.pokemon[sM.selectedPokemon[i] - 1]);
//                newPkmn.GetComponent<Unit>().Init();
//                player1Pokemon.Add(newPkmn);
//                newPkmn.transform.parent = GameObject.Find("MainBattleObjects").transform;
//                newPkmn.SetActive(false);
//            }
//        else {
//            GameObject p1Test = Instantiate(pX.pokemon[0]);
//            player1Pokemon.Add(p1Test);
//            p1Test.transform.parent = GameObject.Find("MainBattleObjects").transform;
//            p1Test.SetActive(false);
//            p1Test.GetComponent<Unit>().Init();
//            Debug.Log("No selection found to populate team! Added a Charmander for debugging.");
//        }
//    }
//    void PopulatePlayer2Team()
//    {
//        GameObject p2test = Instantiate(pX.pokemon[3]);
//        player2Pokemon.Add(p2test);
//        p2test.transform.parent = GameObject.Find("MainBattleObjects").transform;
//        p2test.GetComponent<Unit>().Init();
//        p2test.SetActive(false);
//    }

//    void Update()
//    {
        
//    }

//}
