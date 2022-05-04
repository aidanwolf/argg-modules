using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class HealthSystem : Collidable
{
    //callbacks
    public string onHpGain { get; set; }
    public string onHpLost { get; set; }
    public string onHpZero { get; set; }

    //params
    public float hp { get; set; }
    public string damagedBy { get; set; }

    public override void Init () {
        base.Init();
        SetDefaults();
    }

    public override void SetDefaults () {
        base.SetDefaults();
        if (hp == 0)
            hp = 100;

        if (string.IsNullOrEmpty(onHpZero)) {
            onHpZero = "Destroy";
        }
    }

    public void ReceiveDamage (float damage) {
        hp -= damage;
        Debug.Log("Received Damage! " + damage + "\nhp is now: " + hp);

        if (hp <= 0 && !string.IsNullOrEmpty(onHpZero)) {
            ModuleParser.Parse(gameObject, onHpZero);
            return;
        }
        if (!string.IsNullOrEmpty(onHpLost))
            ModuleParser.Parse(gameObject, onHpLost);
    }

    public void ReceiveHealth (float health) {
        hp += health;
        if (!string.IsNullOrEmpty(onHpGain))
            ModuleParser.Parse(gameObject, onHpGain);
    }
}