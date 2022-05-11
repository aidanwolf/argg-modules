using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : Module
{
    public AnchorManager.Anchor target { get; set; }
    public float speed { get; set; }

    public override void Init () {
        base.Init();

        if (target == null)
            target = AnchorManager.GetAnchor("CROSSHAIR"); //a point 1000m out in front of the camera

        if (speed == 0)
            speed = 12;
    }

    public override void Update () {
        //if init = false, don't continue
        base.Update();

        Vector3 targetDir = target.position - transform.position;
        //targetDir.y = 0;
        float step = speed * Time.deltaTime;
 
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
        Debug.DrawRay(transform.position, newDir, Color.red);
 
        transform.rotation = Quaternion.LookRotation(newDir);
    }

    public override void Deinit () {
        base.Deinit();
    }

}
