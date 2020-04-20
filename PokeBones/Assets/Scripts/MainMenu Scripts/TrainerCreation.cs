using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class TrainerCreation : MonoBehaviour
{
    public string username, password;
    public GameObject userBox, passBox;
    public TextMeshProUGUI displayText;
    public GameObject selectPkmnScreen, existingUScreen, clientObj;

    public Button confirmButton;

    void Update()
    {
        username = userBox.GetComponent<TextMeshProUGUI>().text;
        password = passBox.GetComponent<TextMeshProUGUI>().text;

        if (username != "" && password != "")
        {
            confirmButton.interactable = true;
        }
        else confirmButton.interactable = false;
    }

    public void CreateIndex( string uN)
    {
        NetworkClient networkClient = GameObject.Find("ClientObject").GetComponent<NetworkClient>();
        networkClient.myPlayer.playerName = uN;
    }

    public void OnLoginButton()
    {
        StartCoroutine("PostLogin");
    }

    public void OnNewUserButton()
    {
        StartCoroutine("PostNewUser");
    }

    public void OnEndGame()
    {
        StartCoroutine("UpdateScore");
    }

    IEnumerator PostNewUser()
    {
        var requestBody = new LoginRequest()
        {
            username = username.Trim(),
            password = password.Trim()
        };
        var postData = JsonUtility.ToJson(requestBody);

        using (UnityWebRequest www = UnityWebRequest.Put("https://09eq856ygl.execute-api.us-east-1.amazonaws.com/default/AddUser", postData))
        {
            www.method = UnityWebRequest.kHttpVerbPOST;
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Accept", "application.json");

            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                ReturnLoginRegister(www.downloadHandler.text);
            }
        }
    }


    IEnumerator PostLogin()
    {

        var requestBody = new LoginRequest()
        {
            username = username.Trim(),
            password = password.Trim()
        };
        var postData = JsonUtility.ToJson(requestBody);


        using (UnityWebRequest www = UnityWebRequest.Put("https://wxrtjphwqg.execute-api.us-east-1.amazonaws.com/default/Assign4func", postData))
        {
            www.method = UnityWebRequest.kHttpVerbPOST;
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Accept", "application.json");

            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                ReturnLoginRegister(www.downloadHandler.text);
            }
        }
    }

    public void ReturnLoginRegister(string returnString)
    {
        if (returnString.IndexOf("error") >= 0)
        {
            var srvError = JsonUtility.FromJson<ServerError>(returnString);
            displayText.text = srvError.error;
        }
        else
        {
            var userData = JsonUtility.FromJson<UserData>(returnString);
            

            displayText.text = "Welcome, " + userData.user_id + "!!";
            NetworkClient nC = clientObj.GetComponent<NetworkClient>();
            nC.myPlayer.playerName = userData.user_id;
            nC.myPlayer.playerSkill = int.Parse(userData.playerSkill);

            
            selectPkmnScreen.SetActive(true);
            existingUScreen.SetActive(false);

        }
    }

    public void OnLogout()
    {
        displayText.text = "User logged out";
    }

    [System.Serializable]
    class ServerError
    {
        public string error;
    }

    [System.Serializable]
    class UserData
    {
        public string user_id;
        public string password;
        public string playerSkill;

    }

    class LoginRequest
    {
        public string username;
        public string password;
        
    }
}
