using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Physical : Collidable
{
    public float? weight {get; set;}

    public bool freezeRotation {get;set;}

    public bool gravity {get;set;}

    public Rigidbody rigidbody;

    public Collider[] colliders;

    public override void Init () {
        base.Init();

        SetDefaults();

        rigidbody = Componentizer.DoComponent<Rigidbody>(gameObject,true);
        rigidbody.useGravity = gravity;
        rigidbody.mass = (float)weight;
        rigidbody.freezeRotation = freezeRotation;

        Componentizer.DoComponent<DragRigidbody>(gameObject,true);
        gameObject.layer = LayerMask.NameToLayer("Interactive");

        colliders = GetComponentsInChildren<Collider>();
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

        rigidbody = Componentizer.DoComponent<Rigidbody>(gameObject,false);
        Componentizer.DoComponent<DragRigidbody>(gameObject,false);
    }

}
