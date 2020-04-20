using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicsScript : MonoBehaviour
{
    public Transform startMarkerC, startMarkerF;
    public Transform endMarkerC, endMarkerF;

    GraphicDex gD;

    GameObject closeGFX, farGFX;

    // Start is called before the first frame update
    void Start()
    {
        gD = GetComponent<GraphicDex>();
    }

    public void DisplayGraphic(int displayObjIndex, bool closeLocation)
    {
        Transform spawnPosition;
        if(closeLocation)
        {
            spawnPosition = startMarkerC;
        }
        else
        {
            spawnPosition = startMarkerF;
        }

        GameObject gfx = Instantiate(gD.pkmnGfx[displayObjIndex], spawnPosition);
        if (closeLocation)
        {
            if (closeGFX)
                Destroy(closeGFX);
            closeGFX = gfx;
        }
        else if (!closeLocation)
        {
            if (farGFX)
                Destroy(farGFX);
            farGFX = gfx;
        }

        Animator anim = gfx.GetComponent<Animator>();
        anim.SetBool("Close", closeLocation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
