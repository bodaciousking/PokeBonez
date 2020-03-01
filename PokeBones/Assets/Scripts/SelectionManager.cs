using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public List<int> selectedPokemon = new List<int>();
    public List<Sprite> sprites = new List<Sprite>();

    public static SelectionManager instance;

    void Start()
    {
            
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Too many selection Managers!");
            Destroy(this.gameObject);
            return;
        }
        instance = this;

        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
