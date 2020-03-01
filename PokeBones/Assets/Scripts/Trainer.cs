using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trainer : MonoBehaviour
{
    public Sprite trainerFront, trainerBack;
    public string username;
    public int level = 0, exp, expNeeded;

    // Start is called before the first frame update
    void Start()
    {
        LevelUp();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LevelUp()
    {
        level++;
        exp = 0;
        expNeeded = level * 100;
    }
}
