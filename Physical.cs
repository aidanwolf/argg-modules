using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Physical : Collidable
{
    public float? weight {get; set;}

    public Rigidbody rigidbody;

    public override void Init () {
        base.Init();

        SetDefaults();

        rigidbody = Componentizer.DoComponent<Rigidbody>(gameObject,true);
        rigidbody.useGravity = false;
        rigidbody.mass = (float)weight;

        Componentizer.DoComponent<DragRigidbody>(gameObject,true);
        gameObject.layer = LayerMask.NameToLayer("Interactive");
    }

    public override void SetDefaults () {
        base.SetDefaults();
        if (weight == null)
            weight = 1f;
    }

    public override void Update () {
        base.Update();
    }

    public override void Deinit () {
        base.Deinit();
    }

}
