using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : Module
{
    public float time {get;set;}

    public string onTime {get;set;}

    private Coroutine timer;

    public override void Init() {
        base.Init();
        timer = StartCoroutine(timerRoutine(time));
    }

    IEnumerator timerRoutine (float time) {
        yield return new WaitForSeconds(time);
        if (!string.IsNullOrEmpty(onTime))
            ModuleParser.Parse(gameObject,onTime);
    }

    public override void Deinit() {
        base.Deinit();
        StopCoroutine(timer);
    }
}
