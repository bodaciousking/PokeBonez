using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TrainerCreation : MonoBehaviour
{
    public string username, password;
    public string trainerID;
    public GameObject userBox, passBox;

    public Button confirmButton;

    void Update()
    {
        username = userBox.GetComponent<TextMeshProUGUI>().text;
        password = passBox.GetComponent<TextMeshProUGUI>().text;


        if (username != "" && password != "" && trainerID != "")
        {
            confirmButton.interactable = true;
        }
        else confirmButton.interactable = false;
    }

    public void CreateIndex()
    {
        string path = Application.dataPath + "/Log.txt";
        if (!File.Exists(path))
        {
            File.WriteAllText(path, "Login log \n\n");
        }
        string content = username + " " + password + " " + trainerID + " " + System.DateTime.Now + "\n";
        File.AppendAllText(path, content);
    }
}
