using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LoggedInUser : MonoBehaviour
{
    bool userLoggedIn, instantiated;
    public string username, level;
    public GameObject usertext, leveltext;
    public Image trainerImage;
    public Trainer myTrainer;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(userLoggedIn && !instantiated)
        {

        }
        usertext.GetComponent<Text>().text = username;
        leveltext.GetComponent<Text>().text = "Level " + leveltext;
    }
}
