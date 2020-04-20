using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonUIDisplay : MonoBehaviour
{
    public static ButtonUIDisplay instance;

    public Button abilityButton;
    
    Unit fPUnit;
    HudUpdater hU;

    public GameObject newGM;



    void Start()
    {
    }

    public void UpdateAbilityButtons()
    {
        hU = newGM.GetComponent<HudUpdater>();
        fPUnit = hU.p1current;
        string holderName = "AbilitiesPanel";
        if (GameObject.Find(holderName))
        {
            for (int i = 0; i < fPUnit.AbilitiesList.Count; i++)
            {
                Button newButton = Instantiate(abilityButton);
                newButton.transform.parent = GameObject.Find(holderName).transform;
                newButton.transform.localScale = Vector3.one;
                newButton.GetComponentInChildren<TextMeshProUGUI>().text = fPUnit.AbilitiesList[i].abilityName;
                newButton.onClick.RemoveAllListeners();
                newButton.onClick.AddListener(() => OnButtonClick());
            }
        }
    }

    public void OnButtonClick()
    {
        NetworkClient nC = null;
        if (GameObject.Find("ClientObject"))
            nC = GameObject.Find("ClientObject").GetComponent<NetworkClient>();
        else if (GameObject.Find("TesClientSceneLoader"))
            nC = GameObject.Find("TesClientSceneLoader").GetComponent<NetworkClient>();

        DeleteAllButtons();
        nC.UseAbility(fPUnit);
        DialogueScript dS = GameObject.Find("newGM").GetComponent<DialogueScript>();
        dS.backButton.SetActive(false);
    }

    public void DeleteAllButtons()
    {
        foreach(GameObject abilityButtonD in GameObject.FindGameObjectsWithTag("AbilityButton"))
        {
            Destroy(abilityButtonD);
        }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Too many Button UI Managers!");
            return;
        }
        instance = this;
    }
}
