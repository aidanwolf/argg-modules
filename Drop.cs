using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : Module
{
    private Item itemModule;

    public override void Init () {
        base.Init();

        if (itemModule == null)
            itemModule = Componentizer.DoComponent<Item>(gameObject,true);

        itemModule.OnDropItem();

    }
}
