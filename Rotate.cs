using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : Module
{

    public Vector3 rotation {get;set;}
    // Start is called before the first frame update
    public override void Init() {
        base.Init();

        transform.eulerAngles += rotation;
    }
}
