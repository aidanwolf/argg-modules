using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEquippable : Item
{

    public string onEquip { get; set; }

    public string onUnequip { get; set; }

    public override void Init () {
        base.Init();
        if (!String.IsNullOrEmpty(onEquip))
            ModuleParser.Parse(gameObject, onEquip);
    }

    public override void Update () {
        //if init = false, don't continue
        base.Update();
    }

    public override void Deinit () {
        base.Deinit();

        if (!String.IsNullOrEmpty(onEquip))
            ModuleParser.Parse(gameObject, onEquip, false); //deinit equip state

        if (!String.IsNullOrEmpty(onUnequip))
            ModuleParser.Parse(gameObject, onUnequip);
    }

}
