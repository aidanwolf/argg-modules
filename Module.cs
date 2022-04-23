using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : MonoBehaviour
{
    [NonSerialized]
    public bool init = false;

    public virtual void Init () {
        init = true;
    }

    public virtual void Update () {
        if (!init)
            return;
    }

    public virtual void Deinit () {
        init = false;
    }

    public virtual void SetDefaults () {
        
    }
}
