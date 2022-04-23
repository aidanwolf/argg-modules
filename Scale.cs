using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scale : Module
{

    public float scale { get; set; }

    private Vector3 orgScale;

    private void Awake() {
        orgScale = transform.localScale;
    }

    public override void Init()
    {
        base.Init();

        SetDefaults();

        transform.localScale = orgScale*scale;

    }

    public override void SetDefaults()
    {
        base.SetDefaults();
        if (scale == 0) {
            scale = 1f;
        }
    }
}
