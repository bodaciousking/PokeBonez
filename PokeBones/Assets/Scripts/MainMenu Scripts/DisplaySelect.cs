using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplaySelect : MonoBehaviour
{
    public int index;
    public int selectedID = 0;
    Image myImage;
    SelectionManager sM;
    

    private void Start()
    {
        sM = SelectionManager.instance;
        myImage = GetComponent<Image>();
    }

    private void Update()
    {
        if (sM.selectedPokemon.Length > index)
        {
            myImage.enabled = true;
            selectedID = int.Parse(sM.selectedPokemon[index].ToString());
            myImage.sprite = sM.sprites[selectedID].GetComponent<SpriteRenderer>().sprite;
        }
    }
}
