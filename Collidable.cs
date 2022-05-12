using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collidable : Module {

    public string collider {get;set;}

    public string onHit {get;set;}

    public List<Collider> colliders;

    public override void Init () {
        base.Init();

        colliders = new List<Collider>();

        //we can specify a child object by name to be the collider
        if (!string.IsNullOrEmpty(collider)) {
            
            var child = transform.RecursiveFindChild(collider);
            if (child) {
                var meshRenderer = child.GetComponent<MeshRenderer>();
                var skinnedMeshRenderer = child.GetComponent<SkinnedMeshRenderer>();

                if (meshRenderer) {
                    var meshCollider = Componentizer.DoComponent<MeshCollider>(meshRenderer.gameObject,true);
                    meshCollider.convex = true;
                    colliders.Add(meshCollider);
                } else if (skinnedMeshRenderer) {
                    var meshCollider = Componentizer.DoComponent<MeshCollider>(skinnedMeshRenderer.gameObject,true);
                    meshCollider.sharedMesh = skinnedMeshRenderer.sharedMesh;
                    meshCollider.convex = true;
                    colliders.Add(meshCollider);
                }
            }

        } else {
            var meshRenderers = gameObject.GetComponentsInChildren<MeshRenderer>();
            var skinnedMeshRenderers = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();

            foreach (var meshRenderer in meshRenderers) {
                var meshCollider = Componentizer.DoComponent<MeshCollider>(meshRenderer.gameObject,true);
                meshCollider.convex = true;
                colliders.Add(meshCollider);
            }
            foreach (var skinnedMeshRenderer in skinnedMeshRenderers) {
                var meshCollider = Componentizer.DoComponent<MeshCollider>(skinnedMeshRenderer.gameObject,true);
                meshCollider.sharedMesh = skinnedMeshRenderer.sharedMesh;
                meshCollider.convex = true;
                colliders.Add(meshCollider);
            }
        }
    }

    public override void Deinit() {
        base.Deinit();
        var colliders = gameObject.GetComponentsInChildren<Collider>();
        foreach (var collider in colliders) {
            Destroy(collider);
        }
    }

    public void ReceiveDamage (float damage) {
        if (!string.IsNullOrEmpty(onHit))
            ModuleParser.Parse(gameObject,onHit);
    }
}
