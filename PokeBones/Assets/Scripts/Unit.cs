using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;
    public bool fainted = false;
    public bool close;
    public int unitLevel, maxHP, currentHP, unitID;
    public List<Ability> AbilitiesList = new List<Ability>();
    
    DialogueScript dS;

    public void Init()
    {
        currentHP = maxHP;
        dS = DialogueScript.instance;
    }
    public void AddTackle()
    {
        AbilitiesList.Add(new Tackle());
    }
}

public class Ability
{
    public string abilityName;
    public PkType abilityType;
    public bool dealsDamage;
    public int abilityDamage;

    public virtual void ActivateAbility()
    {
        Debug.Log("Activated " + abilityName + "!");
        ButtonUIDisplay bUI = ButtonUIDisplay.instance;
        bUI.DeleteAllButtons();
    }
}

public class Tackle : Ability
{
    public
    Tackle()
    {
        abilityName = "Tackle";
        dealsDamage = true;
        abilityDamage = 5;
        //abilityType = new Normal();
    }

    public override void ActivateAbility()
    {
        base.ActivateAbility();
        // bS.DealDamage(abilityDamage);
    }
}
