using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PkmnGFXScript : MonoBehaviour
{
    Unit entity;
    Animator anim;
    BattleSystem bS;
    SpriteRenderer sprite;

    bool flip;

    // Start is called before the first frame update
    void Start()
    {
        entity = GetComponentInParent<Unit>();
        anim = GetComponent<Animator>();
        bS = GameObject.Find("GameManager").GetComponent<BattleSystem>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        bool faint = entity.fainted;
        anim.SetBool("Fainted", faint);

        if(faint)
        {
            anim.StopPlayback();
        }

        if (bS.player1Pokemon == this.transform.parent.gameObject)
        {
            flip = true;
        }
        else
        {
            flip = false;
        }
        sprite.flipX = flip;
    }

    public void TakeHit()
    {
        if (flip)
            anim.Play("ImpactR");
        else
            anim.Play("Impact");
    }

    public void ToggleSprite()
    {
        sprite.enabled = !sprite.enabled;
    }
}
