using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collidable : Module {

    public override void Init () {
        base.Init();
        var meshRenderer = gameObject.GetComponentInChildren<MeshRenderer>();
        var skinnedMeshRenderer = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
        if (meshRenderer) {
            var meshCollider = Componentizer.DoComponent<MeshCollider>(meshRenderer.gameObject,true);
            meshCollider.convex = true;
        } else if (skinnedMeshRenderer) {
            var meshCollider = Componentizer.DoComponent<MeshCollider>(skinnedMeshRenderer.gameObject,true);
            meshCollider.sharedMesh = skinnedMeshRenderer.sharedMesh;
            meshCollider.convex = true;
        }
    }
}
