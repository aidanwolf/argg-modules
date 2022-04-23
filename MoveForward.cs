using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : Module
{

    public float speed {get;set;}

    private Rigidbody rigidbody;

    public override void Init () {
        base.Init();

        rigidbody = GetComponent<Rigidbody>();

        SetDefaults();
    }

    public override void Update () {
        base.Update();
        if (rigidbody) {
            //var pos = transform.position + transform.forward * speed;
            //rigidbody.MovePosition(pos);
            Debug.Log(speed);
            rigidbody.AddForce((transform.forward.normalized * speed) - rigidbody.velocity, ForceMode.VelocityChange);

        } else {
            transform.position += transform.forward * speed;
        }
    }

    public override void Deinit () {
        base.Deinit();
    }

    public override void SetDefaults () {
        base.SetDefaults();
        if (speed == 0) {
            speed = 0.1f;
        }
    }
}
