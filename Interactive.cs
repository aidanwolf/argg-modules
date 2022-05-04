using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Interactive : Module
{   

    public string onTap {get;set;}

    public string onGrab {get;set;}

    private EventTrigger eventTrigger;

    public override void Init () {
        base.Init();

        Debug.Log("gogogoogog");

        eventTrigger = Componentizer.DoComponent<EventTrigger>(gameObject,true);
        eventTrigger.triggers.Clear();

        if (!string.IsNullOrEmpty(onTap)) {
            Debug.Log("onTap!!");
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((data) => {
                Debug.Log("TAP!!");
                ModuleParser.Parse(gameObject,onTap);
            });
            eventTrigger.triggers.Add(entry);
        }
    }

    public override void Deinit () {
        base.Deinit();

        // if (eventTrigger)
        //     eventTrigger.triggers.Clear();
    }
}
