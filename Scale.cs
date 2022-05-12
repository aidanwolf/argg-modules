using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scale : Module
{

    public float size { get; set; }

    private Vector3 orgScale;

    private void Awake() {
        orgScale = transform.localScale;
    }

    public override void Init()
    {
        base.Init();

        SetDefaults();

        transform.localScale = orgScale*size;

    }

    public override void SetDefaults()
    {
        base.SetDefaults();
        if (size == 0) {
            size = 1f;
        }
    }
}
