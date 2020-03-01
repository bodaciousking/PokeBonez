using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pokedex : MonoBehaviour
{
    public List<GameObject> pokemon = new List<GameObject>();

    public static Pokedex instance;

    private void Awake()
    {
        if(instance!=null)
        {
            Debug.Log("Too many Pokedii!");
            return;
        }
        instance = this;
    }
}
