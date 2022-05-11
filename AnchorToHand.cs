using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorToHand : Module
{
    public AnchorManager.Anchor target  { get; set; }
    public float slowParent { get; set; }

    public bool bothHands { get; set; }

    public override void Init () {
        base.Init();
        
        SetDefaults();

        if (target == null)
            target = AnchorManager.GetAnchor("HAND_ANCHOR");

        if (slowParent == 0)
            slowParent = 12;
    }

    public override void SetDefaults() {
        base.SetDefaults();
        bothHands = true;
    }

    public override void Update () {
        //if init = false, don't continue
        base.Update();

        if (target == null)
            return;

        float step = slowParent * Time.deltaTime;
        Vector3 position = Vector3.Lerp(transform.position,target.position, step);

        transform.position = position;
    }

    public override void Deinit () {
        base.Deinit();
    }

}
