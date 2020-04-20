using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public string selectedPokemon;
    public List<GameObject> sprites = new List<GameObject>();

    NetworkClient myClient;
    public GameObject confirmButton;

    public static SelectionManager instance;
    
    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Too many selection Managers!");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        
    }

    private void Start()
    {
        myClient = GameObject.Find("ClientObject").GetComponent<NetworkClient>();
    }

    // Update is called once per frame
    void Update()
    {
        myClient.myPlayer.selectedPokemon = selectedPokemon;
        if (confirmButton)
        {
            if (selectedPokemon.Length != 3)
            {
                confirmButton.SetActive(false);
            }
            else confirmButton.SetActive(true);
        }
    }
}
