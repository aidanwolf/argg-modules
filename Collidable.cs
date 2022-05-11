using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collidable : Module {

    public override void Init () {
        base.Init();
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
