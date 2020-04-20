using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ScoreUpdater : MonoBehaviour
{
    public string userID;
    public int score;

    public void Start()
    {
        StartCoroutine("UpdateScore");
    }

    public IEnumerator UpdateScore()
    {
        var requestBody = new ScoreUpdate()
        {
            username = userID.Trim(),
            score = score.ToString().Trim()
        };

        var postData = JsonUtility.ToJson(requestBody);

        using (UnityWebRequest www = UnityWebRequest.Put("https://h5yn1jjwp6.execute-api.us-east-1.amazonaws.com/default/PokeBonesScoreUpdate", postData))
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
                Debug.Log("Success!" + www.downloadHandler.text);
                
            }
        }
    }
    class ScoreUpdate
    {
        public string username;
        public string score;

    }
}
