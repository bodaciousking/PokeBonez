using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSelect : MonoBehaviour
{
    public int ID;
    public Sprite mySprite;

    SelectionManager sM;

    private void Awake()
    {
    }
    private void Start()
    {
        sM = SelectionManager.instance;
        mySprite = GetComponent<Sprite>();
    }

    //public void AddToTeam()
    //{
    //    if(sM.selectedPokemon.Count <= 2)
    //        sM.selectedPokemon.Add(ID);
    //    if (sM.selectedPokemon.Contains(ID))
    //        return;
    //    else
    //    {
    //        sM.selectedPokemon[2] = sM.selectedPokemon[1];
    //        sM.selectedPokemon[1] = sM.selectedPokemon[0];
    //        sM.selectedPokemon[0] = ID;
    //    }
    //}
    public void AddToTeam()
    {
        bool canAdd = true;
        foreach (char c in sM.selectedPokemon)
        {
            if (c.ToString() == ID.ToString())
            {
                canAdd = false;
            }
        }
        if (canAdd)
        {
            if (sM.selectedPokemon.Length <= 2)
            {
                sM.selectedPokemon = sM.selectedPokemon.Insert(sM.selectedPokemon.Length, ID.ToString());
            }

            else
            {
                string newSelection = "";
                for (int i = 0; i < sM.selectedPokemon.Length; i++)
                {
                    if (i == sM.selectedPokemon.Length -1)
                    {
                        newSelection = newSelection.Insert(i, ID.ToString());
                    }
                    else
                    {
                        char c = sM.selectedPokemon[i + 1];
                        newSelection = newSelection.Insert(i, c.ToString());
                    }
                }
                sM.selectedPokemon = newSelection;
            }
        }
    }
}
