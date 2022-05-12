using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Shade : Module
{

    public float glow {get;set;}

    public override void Init() {
        base.Init();
        
        Renderer[] renderers = gameObject.GetComponentsInChildren<MeshRenderer>();
        renderers = renderers.Concat<Renderer>(gameObject.GetComponentsInChildren<SkinnedMeshRenderer>()).ToArray();

        Debug.Log("shade renderer count " + renderers.Length);

        foreach (var renderer in renderers) {
            if (glow > 0) {
                renderer.sharedMaterial.SetVector("_EmissionColor",  new Vector4(glow,glow,glow,1));
            }
        }

    }

}
