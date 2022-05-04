using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Shared;

public class Spawn : Module
{
    
    //Spawn[spawnable:,position:,rotation:,unique:]

    public string unique;

    private ModuleParser.GameObjectWithScript _spawnable;
    public ModuleParser.GameObjectWithScript? uri {get { return _spawnable;}
     set {
         Debug.Log("SPAWNABLE SET!!!!");
         _spawnable = value;
         _spawnable.gObj.SetActive(false);
    }}

    public Vector3? position {get; set;}
    public Quaternion? rotation {get; set;}

    public float? time {get; set;}

    public IEnumerator Wait () {
        yield return new WaitUntil(() => uri != null);
        Init();
    }

    public override void Init () {
        base.Init();

        if (uri == null) {
            Debug.Log("Spawnable is null!");
            StartCoroutine(Wait());
            return;
        }

        SetDefaults();

        GameObject spawnableCopy = Instantiate(uri.gObj);
        spawnableCopy.transform.position = transform.position+(Vector3)position+new Vector3(0,spawnableCopy.getBounds().extents.y,0);
        spawnableCopy.transform.rotation = transform.rotation*(Quaternion)rotation;
        spawnableCopy.SetActive(true);
        ModuleParser.Parse(spawnableCopy, uri.script);

        if (!string.IsNullOrEmpty(unique))
            AnchorManager.AddAnchor(unique,spawnableCopy);

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
