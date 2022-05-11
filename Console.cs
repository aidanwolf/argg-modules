using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Console : Module
{
    public string message {get;set;}

    private TMP_Text consoleOutput;

    public override void Init() {
        base.Init();

        if (consoleOutput == null)
            consoleOutput = GameObject.Find("Console").GetComponent<TMP_Text>();

        if (consoleOutput != null)
            consoleOutput.text = message;
        else
            Debug.Log(message);
    }
}
