using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetState : Module
{
    //SetState[to:int]

    public int to;

    public override void Init() {
        base.Init();
        GetComponent<StateMachine>().state = to;
        Deinit();
    }
}
