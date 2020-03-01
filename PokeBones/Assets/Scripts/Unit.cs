using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;
    public Sprite profilePic;
    public bool fainted = false;
    public int unitLevel, maxHP, currentHP;
    public List<Ability> AbilitiesList = new List<Ability>();

    BattleSystem bS;
    DialogueScript dS;

    public void Init()
    {
        currentHP = maxHP;
        AbilitiesList.Add(new Tackle());
        bS = BattleSystem.instance;
        dS = DialogueScript.instance;
    }
}

public class Ability
{
    public string abilityName;
    public PkType abilityType;

    public virtual void ActivateAbility()
    {
        Debug.Log("Activated " + abilityName + "!");
        ButtonUIDisplay bUI = ButtonUIDisplay.instance;
        bUI.DeleteAllButtons();
    }
}

public class Tackle : Ability
{
    protected int abilityDamage;
    public
    Tackle()
    {
        abilityName = "Tackle";
        abilityDamage = 5;
        //abilityType = new Normal();
    }

    public override void ActivateAbility()
    {
        base.ActivateAbility();
        BattleSystem bS = BattleSystem.instance;
        bS.DealDamage(abilityDamage);
    }
}
