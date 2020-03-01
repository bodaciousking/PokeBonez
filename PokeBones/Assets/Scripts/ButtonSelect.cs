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

    public void AddToTeam()
    {
        if(sM.selectedPokemon.Count <= 2)
            sM.selectedPokemon.Add(ID);
        if (sM.selectedPokemon.Contains(ID))
            return;
        else
        {
            sM.selectedPokemon[2] = sM.selectedPokemon[1];
            sM.selectedPokemon[1] = sM.selectedPokemon[0];
            sM.selectedPokemon[0] = ID;
        }
    }
}
