using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainerDisplay : MonoBehaviour
{
    Image myImage;
    public GameObject correspondingButton;

    // Start is called before the first frame update
    void Start()
    {
        myImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if(correspondingButton!=null)
        myImage.sprite =correspondingButton.GetComponentInChildren<Image>().sprite;
    }
}
