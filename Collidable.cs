using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collidable : Module {

    public string collider {get;set;}

    public string onHit {get;set;}

    public override void Init () {
        base.Init();

        //we can specify a child object by name to be the collider
        if (!string.IsNullOrEmpty(collider)) {
            
            var child = transform.RecursiveFindChild(collider);
            if (child) {
                var meshRenderer = child.GetComponent<MeshRenderer>();
                var skinnedMeshRenderer = child.GetComponent<SkinnedMeshRenderer>();

                if (meshRenderer) {
                    var meshCollider = Componentizer.DoComponent<MeshCollider>(meshRenderer.gameObject,true);
                    meshCollider.convex = true;
                } else if (skinnedMeshRenderer) {
                    var meshCollider = Componentizer.DoComponent<MeshCollider>(skinnedMeshRenderer.gameObject,true);
                    meshCollider.sharedMesh = skinnedMeshRenderer.sharedMesh;
                    meshCollider.convex = true;
                }
            }

        } else {
            var meshRenderers = gameObject.GetComponentsInChildren<MeshRenderer>();
            var skinnedMeshRenderers = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();

            foreach (var meshRenderer in meshRenderers) {
                var meshCollider = Componentizer.DoComponent<MeshCollider>(meshRenderer.gameObject,true);
                meshCollider.convex = true;
            }
            foreach (var skinnedMeshRenderer in skinnedMeshRenderers) {
                var meshCollider = Componentizer.DoComponent<MeshCollider>(skinnedMeshRenderer.gameObject,true);
                meshCollider.sharedMesh = skinnedMeshRenderer.sharedMesh;
                meshCollider.convex = true;
            }
        }
    }

    public void ReceiveDamage (float damage) {
        if (!string.IsNullOrEmpty(onHit))
            ModuleParser.Parse(gameObject,onHit);
    }
}
