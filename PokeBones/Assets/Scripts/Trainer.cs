using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trainer : MonoBehaviour
{
    public string trainerName;
    public Sprite trainerFront, trainerBack;
    public bool close;

    SpriteRenderer sR;

    // Start is called before the first frame update
    void Start()
    {
        sR = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (close)
            sR.sprite = trainerBack;
        else
            sR.sprite = trainerFront;
    }
    
}
