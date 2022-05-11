using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Physical : Collidable
{
    public float? weight {get; set;}

    public bool freezeRotation {get;set;}

    public bool? gravity {get;set;}

    public bool? interactive {get;set;}

    public float bounce {get;set;}

    public Rigidbody rigidbody;

    public Collider[] colliders;

    public string onGrab {get;set;}

    public string onHitGround {get;set;}

    public override void Init () {
        base.Init();

        SetDefaults();

        rigidbody = Componentizer.DoComponent<Rigidbody>(gameObject,true);
        rigidbody.useGravity = (bool)gravity;
        rigidbody.mass = (float)weight;
        rigidbody.freezeRotation = freezeRotation;

        if ((bool)interactive == true) {
            var drag = Componentizer.DoComponent<DragRigidbody>(gameObject,true);
            drag.onGrab.AddListener(()=>{
                if (!string.IsNullOrEmpty(onGrab))
                    ModuleParser.Parse(gameObject,onGrab);
            });
            gameObject.layer = LayerMask.NameToLayer("Interactive");
        }

        colliders = GetComponentsInChildren<Collider>();

        if (bounce > 0) {
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
            PhysicMaterial physicsMat = new PhysicMaterial();
            physicsMat.bounciness = bounce;
            foreach (Collider collider in colliders) {
                collider.material = physicsMat;
            }
        }
    }

    public override void SetDefaults () {
        base.SetDefaults();
        if (weight == null)
            weight = 1f;

        if (interactive == null)
            interactive = true;

        if (gravity == null)
            gravity = true;
    }

    public override void Update () {
        base.Update();
    }

    public override void Deinit () {
        base.Deinit();

        rigidbody = Componentizer.DoComponent<Rigidbody>(gameObject,false);
        Componentizer.DoComponent<DragRigidbody>(gameObject,false);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Mesh")) {
            Debug.Log("hit ground! " + gameObject.name);
            if (!string.IsNullOrEmpty(onHitGround))
                ModuleParser.Parse(gameObject, onHitGround);
        }
    }

    private void OnCollisionExit(Collision collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Mesh")) {
            
        }
    }
}
