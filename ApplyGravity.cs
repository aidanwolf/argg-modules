using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyGravity : Physical
{

    public float bounce {get;set;}

    public string onGroundHit {get;set;}

    public override void Init () {
        base.Init();
        rigidbody.useGravity = true;

        if (bounce > 0) {
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            PhysicMaterial physicsMat = new PhysicMaterial();
            physicsMat.bounciness = bounce;
            foreach (Collider collider in colliders) {
                collider.material = physicsMat;
            }
        }
    }

    public override void Update () {
        base.Update();
    }

    public override void Deinit () {
        base.Deinit();

        rigidbody.useGravity = false;
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
