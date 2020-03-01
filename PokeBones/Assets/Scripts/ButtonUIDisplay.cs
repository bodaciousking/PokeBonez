using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonUIDisplay : MonoBehaviour
{
    public static ButtonUIDisplay instance;
    PokemonSwitcher pS;

    public Button abilityButton;

    GameObject fightingPokemon;
    Unit fPUnit;

    void Start()
    {
    }

    public void UpdateAbilityButtons()
    {
        pS = PokemonSwitcher.instance;
        fightingPokemon = pS.closePKMN;
        fPUnit = fightingPokemon.GetComponent<Unit>();
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
                newButton.onClick.AddListener(() => OnButtonClick(i));
            }
        }
    }
    public void OnButtonClick(int _i)
    {
        fPUnit.AbilitiesList[_i-1].ActivateAbility();
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
