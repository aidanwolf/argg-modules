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
    
    private string _play;
    public string play {
        get {
            return _play;
        }
        set {
            resetAnim();
            _play = value;
        }
    }

    private string _loop;
    public string loop {
        get {
            return _loop;
        }
        set {
            resetAnim();
            _loop = value;
        }
    }

    private string _pingpong;
    public string pingpong {
        get {
            return _pingpong;
        }
        set {
            resetAnim();
            _pingpong = value;
        }
    }
    public float? speed {get; set;}

    Animation anim;

    private void resetAnim () {
        _play = "";
        _loop = "";
        _pingpong = "";
    }

    public override void Init () {
        if (string.IsNullOrEmpty(play+loop+pingpong))
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
            state.wrapMode = !string.IsNullOrEmpty(play)?WrapMode.Once:(!string.IsNullOrEmpty(loop)?WrapMode.Loop:WrapMode.PingPong);
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

        anim.Rewind();
        anim.Play(!string.IsNullOrEmpty(play)?play:(!string.IsNullOrEmpty(loop)?loop:pingpong));
    }

    public void AnimationStartHandler (string clipName) {
        Debug.Log("AnimationStartHandler");
        if (!string.IsNullOrEmpty(onAnimStart))
            ModuleParser.Parse(gameObject, onAnimStart);
    }

    public void AnimationCompleteHandler (string clipName) {
        Debug.Log("AnimationCompleteHandler");
        if (!string.IsNullOrEmpty(onAnimEnd))
            ModuleParser.Parse(gameObject, onAnimEnd);
    }

    public override void SetDefaults () {
        base.SetDefaults();

        if (speed == null)
            speed = 1;
    }

    public override void Update () {
        base.Update();
    }

    public override void Deinit () {
        base.Deinit();
    }
}
