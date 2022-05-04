using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Shared;

public class Anim : Module
{
    public string onAnimStart { get; set; }
    public string onAnimEnd { get; set; }
    public string play { get; set; }
    public float? speed {get; set;}
    public string animMode {get; set;} //Once //Loop //PingPong

    Animation anim;

    public override void Init () {
        if (string.IsNullOrEmpty(play))
            return;

        base.Init();
        SetDefaults();

        anim = Componentizer.DoComponent<Animation>(gameObject,true);
        var clip = anim.clip;

        if (clip == null) {
            Debug.Log("Animation component has no animation clip!");
            return;
        }
        
        foreach (AnimationState state in anim)
        {
            state.speed = (float)speed;
            state.wrapMode = (WrapMode)System.Enum.Parse( typeof(WrapMode), animMode );
        }

        if (!string.IsNullOrEmpty(onAnimStart)) {
            AnimationEvent animationStartEvent = new AnimationEvent();
            animationStartEvent.time = 0;
            animationStartEvent.functionName = "AnimationStartHandler";
            animationStartEvent.stringParameter = clip.name;
            clip.AddEvent(animationStartEvent);
        }
        
        if (!string.IsNullOrEmpty(onAnimEnd)) {
            AnimationEvent animationEndEvent = new AnimationEvent();
            animationEndEvent.time = clip.length;
            animationEndEvent.functionName = "AnimationCompleteHandler";
            animationEndEvent.stringParameter = clip.name;
            clip.AddEvent(animationEndEvent);
        }

        anim.Play(play);
    }

    public void AnimationStartHandler (string clipName) {
        Debug.Log("AnimationStartHandler");
        ModuleParser.Parse(gameObject, onAnimStart);
    }

    public void AnimationCompleteHandler (string clipName) {
        Debug.Log("AnimationCompleteHandler");
        ModuleParser.Parse(gameObject, onAnimEnd);
    }

    public override void SetDefaults () {
        base.SetDefaults();

        if (speed == null)
            speed = 1;

        if (string.IsNullOrEmpty(animMode))
            animMode = "Once";
    }

    public override void Update () {
        base.Update();
    }

    public override void Deinit () {
        base.Deinit();
    }
}
