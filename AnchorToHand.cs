using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorToHand : Module
{
    public Transform target  { get; set; }
    public float speed { get; set; }

    public override void Init () {
        base.Init();

        if (target == null)
            target = GameObject.Find("HandAnchor").transform;

        if (speed == 0)
            speed = 12;
    }

    public override void Update () {
        //if init = false, don't continue
        base.Update();

        float step = speed * Time.deltaTime;
        Vector3 position = Vector3.Lerp(transform.position,target.position, step);

        transform.position = position;
    }

    public override void Deinit () {
        base.Deinit();
    }

}
