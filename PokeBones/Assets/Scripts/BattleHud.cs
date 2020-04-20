using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHud : MonoBehaviour
{
    public TextMeshProUGUI nameText, levelText;
    public Slider hpSlider;

    public void SetHud(string unitName, int _cHealth, int _mHealth, int _lvl)
    {
        nameText.text = unitName;
        levelText.text = "Lvl " + _lvl;
        hpSlider.maxValue = _mHealth;
        hpSlider.value = _cHealth;
    }
}
