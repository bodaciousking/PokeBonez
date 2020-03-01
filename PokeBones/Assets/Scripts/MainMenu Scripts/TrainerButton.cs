using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainerButton : MonoBehaviour
{
    public TrainerDisplay tD;
    public TrainerCreation tC;
    public string trainerID;
    public void SwitchImage()
    {
        tD.correspondingButton = this.gameObject;
        tC.trainerID = trainerID;
    }
}
