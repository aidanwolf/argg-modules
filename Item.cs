using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Item : Module
{

    public ItemInfo itemInfo;

    //what happens when an item initializes?
    public string onSpawn { get; set; }

    //what happens when you drop an item? doesn't automatically leave inventory unless set
    public string onDrop { get; set; }

    //what happens when you equip an item? defaults to anchoring to hand
    public string onEquip { get; set; }

     //what happens when we store an item away? null default 
    public string onCollect { get; set; }

    public override void Init () {
        base.Init();

        SetDefaults();

        if (itemInfo == null)
            itemInfo = Componentizer.DoComponent<ItemInfo>(gameObject,true);

        //Spawn in front of player
        //Note: this may not be the correct default behavior for an Item
        transform.position = Camera.main.transform.position+Camera.main.transform.forward;

        //SwitchState(0);

        if (!string.IsNullOrEmpty(onSpawn))
            ModuleParser.Parse(gameObject, onSpawn);
    }

    public override void SetDefaults() {
        base.SetDefaults();

        if (string.IsNullOrEmpty(onEquip))
            onEquip = "AnchorToHand";

        if (string.IsNullOrEmpty(onDrop))
            onDrop = "Physical[gravity:true]";
    }

    public override void Update () {
        base.Update();
    }

    public override void Deinit () {
        base.Deinit();
    }

    private void SwitchState (int i) {
        if (!string.IsNullOrEmpty(onSpawn))
            ModuleParser.Parse(gameObject, onSpawn, i==0);
        if (!string.IsNullOrEmpty(onCollect))
            ModuleParser.Parse(gameObject, onCollect, i==1);
        if (!string.IsNullOrEmpty(onDrop))
            ModuleParser.Parse(gameObject, onDrop, i==2);
        if (!string.IsNullOrEmpty(onEquip))
            ModuleParser.Parse(gameObject, onEquip, i==3);
    }

    public void OnCollectItem () {
        SwitchState(1);
    }

    public void OnDropItem () {
        SwitchState(2);
    }

    public void OnEquipItem () {
        //SwitchState(3);
        if (!string.IsNullOrEmpty(onEquip))
            ModuleParser.Parse(gameObject, onEquip);
    }
}