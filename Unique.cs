using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unique : Module
{
    //Unique[id:]

    public string id {
        set {
            AnchorManager.AddAnchor(value,gameObject);
        }
    }
}
