using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog : Module
{
    public string message {get;set;}

    private GameObject speechBubblePrefab;
    private GameObject speechBubble;

    private Bounds bounds;

    public override void Init() {
        base.Init();

        if (speechBubblePrefab == null)
                speechBubblePrefab = Resources.Load<GameObject>("SpeechBubble");

        if (speechBubble == null)
            speechBubble = Instantiate<GameObject>(speechBubblePrefab);

        bounds = gameObject.getBounds();

        speechBubble.GetComponent<STMDialogueSample>().lines = new string[]{
            message
        };
    }

    public override void Update() {
        base.Update();

        if (!init)
            return;

        speechBubble.transform.position = transform.position + new Vector3(0,bounds.extents.y,0);
    }

    public override void Deinit() {
        base.Deinit();
        Destroy(speechBubble);
    }
}
