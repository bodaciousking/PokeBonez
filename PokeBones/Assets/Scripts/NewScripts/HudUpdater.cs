using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudUpdater : MonoBehaviour
{
    public BattleHud closeHUD, farHUD;
    public Unit p1current, p2current;
    Pokedex2000 dex;

    public void UpdateBattleHUD(string unitName, int _cHealth, int _mHealth, int _lvl, int unitID, bool clientSide)
    {
        BattleHud currentHUD;
        dex = GameObject.Find("newGM").GetComponent<Pokedex2000>();

        if (clientSide)
        {
            currentHUD = closeHUD;
            p1current = dex.Pokemon[unitID].GetComponent<Unit>();
            p1current.AddTackle();
        }
        else {
            currentHUD = farHUD;
            p2current = dex.Pokemon[unitID].GetComponent<Unit>();
            p2current.AddTackle();
        }

        currentHUD.SetHud(unitName, _cHealth, _mHealth, _lvl);
    }
}
