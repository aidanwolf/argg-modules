using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Item : Module
{

    public string onSpawn { get; set; }

    public override void Init () {
        base.Init();

        //Spawn in front of player
        //Note: this may not be the correct default behavior for an Item
        transform.position = Camera.main.transform.position+Camera.main.transform.forward;

        if (!String.IsNullOrEmpty(onSpawn))
            ModuleParser.Parse(gameObject, onSpawn);
    }

    public override void Update () {
        base.Update();
    }

    public override void Deinit () {
        base.Deinit();
    }

}
