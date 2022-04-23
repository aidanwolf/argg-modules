using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyGravity : Physical
{

    public string onGroundHit {get;set;}

    public override void Init () {
        base.Init();
        rigidbody.useGravity = true;
    }

    public override void Update () {
        base.Update();
    }

    public override void Deinit () {
        base.Deinit();
    }

    bool groundHit = false;

    void OnCollisionEnter(Collision collision)
    {
        if (!groundHit && collision.gameObject.layer == LayerMask.NameToLayer("Mesh")) {
            Debug.Log("hit ground!");
            FlagManager.SET_FLAG("gooose_attack",1);
            groundHit = true;
            if (!string.IsNullOrEmpty(onGroundHit))
                ModuleParser.Parse(gameObject, onGroundHit);
        }
    }

    private void OnCollisionExit(Collision collision) {
        if (groundHit && collision.gameObject.layer == LayerMask.NameToLayer("Mesh")) {
            //groundHit = false;
        }
    }
}
