using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHud : MonoBehaviour
{
    public TextMeshProUGUI nameText, levelText;
    public Slider hpSlider;

    public void SetHud(Unit unit)
    {
        nameText.text = unit.unitName;
        levelText.text = "Lvl " + unit.unitLevel;
        hpSlider.maxValue = unit.maxHP;
        hpSlider.value = unit.currentHP;
    }

    public void SetHP(int hp)
    {
        hpSlider.value = hp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
