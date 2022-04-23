using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : Module
{
    public string script {get;set;}

    private GameScriptParser parser;

    public override void Init () {
        base.Init();

        if (string.IsNullOrEmpty(script)) {
            Debug.Log("no script to load!");
            return;
        }

        parser = Componentizer.DoComponent<GameScriptParser>(gameObject,true);
        parser.LoadCartridge(script);

    }

    public override void Deinit() {
        base.Deinit();
        if (parser)
            parser.UnloadCartridge();
    }
}
