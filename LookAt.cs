using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : Module
{

    private AnchorManager.Anchor targetAnchor;
    public string target { get; set; }
    public float speed { get; set; }

    public bool upright { get; set; }

    public override void Init () {
        base.Init();

        if (string.IsNullOrEmpty(target)) {
            init = false;
            return;
        }

        targetAnchor = AnchorManager.GetAnchor(target); //a point 1000m out in front of the camera

        if (speed == 0)
            speed = 12;
    }

    public override void Update () {
        //if init = false, don't continue
        base.Update();

        if (!init)
            return;

        Vector3 targetPos = targetAnchor.position;

        if (upright)
            targetPos.y = transform.position.y;

        Vector3 targetDir = targetPos - transform.position;
        //targetDir.y = 0;
        float step = speed * Time.deltaTime;
 
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
        transform.rotation = Quaternion.LookRotation(newDir);
    }

    public override void Deinit () {
        base.Deinit();
    }

}
