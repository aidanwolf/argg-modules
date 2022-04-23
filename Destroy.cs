using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Destroy : Module
{
    public override void Init () {
        base.Init();
        Destroy(gameObject);
    }
}