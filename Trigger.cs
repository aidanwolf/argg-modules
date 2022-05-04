using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Shared;
using System;

public class Trigger : Module
{

    //Trigger[label:string,onTrigger:string]

    public string label {get;set;}
    public string onTrigger {get;set;}

    private GameObject triggerButton;
    private EventTrigger triggerScript;

    public override void Init () {
        base.Init();
        Debug.Log("looking for triggerButton");

        if (triggerButton == null)
            triggerButton = GlobalData.GlobalGameObject("Trigger");

        triggerButton.SetActive(true);

        if (triggerScript == null)
            triggerScript = triggerButton.GetComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data)=>{Triggered((PointerEventData)data);});
        triggerScript.triggers.Add(entry);
    }

    public override void Update () {
        //if init = false, don't continue
        base.Update();

    }

    public override void Deinit () {
        base.Deinit();
        if (triggerScript)
            triggerScript.triggers.Clear();

        if (triggerButton)
            triggerButton.SetActive(false);
    }

    private void Triggered (PointerEventData data) {
        if (!string.IsNullOrEmpty(onTrigger))
            ModuleParser.Parse(gameObject,onTrigger);
    }
}
