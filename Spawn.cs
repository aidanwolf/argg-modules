using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Shared;

public class Spawn : Module
{
    private ModuleParser.GameObjectWithScript _spawnable;
    public ModuleParser.GameObjectWithScript? spawnable {get { return _spawnable;}
     set {
         Debug.Log("SPAWNABLE SET!!!!");
         _spawnable = value;
         _spawnable.gObj.SetActive(false);
    }}

    public Vector3? position {get; set;}
    public Quaternion? rotation {get; set;}

    public float? time {get; set;}

    public override void Init () {
        base.Init();

        if (spawnable == null) {
            Debug.Log("Spawnable is null!");
            return;
        }

        SetDefaults();

        GameObject spawnableCopy = Instantiate(spawnable.gObj);
        spawnableCopy.transform.position = transform.position+(Vector3)position;
        spawnableCopy.transform.rotation = transform.rotation*(Quaternion)rotation;
        spawnableCopy.SetActive(true);
        ModuleParser.Parse(spawnableCopy, spawnable.script);

        if (time != null)
            Destroy(spawnableCopy,(float)time);
    }

    public override void SetDefaults () {
        base.SetDefaults();
  
        if (position == null)
            position = Vector3.zero;

        if (rotation == null) 
            rotation = Quaternion.identity;
    }
}
