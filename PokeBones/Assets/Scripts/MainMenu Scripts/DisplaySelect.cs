using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplaySelect : MonoBehaviour
{
    public int index, selectedID = 0;
    Image myImage;
    SelectionManager sM;
    

    private void Start()
    {
        sM = SelectionManager.instance;
        myImage = GetComponent<Image>();
    }

    private void Update()
    {
        if (sM.selectedPokemon.Count >= index)
        {
            myImage.enabled = true;
            selectedID = sM.selectedPokemon[index-1];
            myImage.sprite = sM.sprites[selectedID-1];
        }
    }
}
