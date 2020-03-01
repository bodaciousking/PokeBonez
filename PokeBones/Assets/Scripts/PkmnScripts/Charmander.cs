using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charmander : Unit
{
    Charmander()
    {
        unitName = "Charmander";
        maxHP = 20;
        unitLevel = 5;
        //AbilitiesList.Add(new Ember());
    }
}

public class Ember : Tackle
{
    public Ember()
    {
        abilityName = "Ember";
        //abilityType = new Fire();
        abilityDamage = 15;
    }
}
