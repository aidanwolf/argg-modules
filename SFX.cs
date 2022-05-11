using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Shared;
using System;

public class SFX : Module
{

    //SFX[play:uri || loop:uri || pin]

    public string onSfxStart { get; set; }
    public string onSfxEnd { get; set; }
    
    private AudioClip _play;
    public AudioClip play {
        get {
            return _play;
        }
        set {
            resetSfx();
            _play = value;
        }
    }

    private AudioClip _loop;
    public AudioClip loop {
        get {
            return _loop;
        }
        set {
            resetSfx();
            _loop = value;
        }
    }

    public float? delay {get; set;}

    AudioSource audioSource;

    private int isPlaying = -1;

    private void resetSfx () {
        _play = null;
        _loop = null;
    }

    public override void Init () {

        Debug.Log("PLAY SFX!!!");

        base.Init();
        SetDefaults();

        audioSource = Componentizer.DoComponent<AudioSource>(gameObject,true);
        audioSource.clip = play!=null?play:loop;

        var clip = audioSource.clip;

        if (clip == null) {
            Debug.Log("No audio clip!!!!");
            init = false;
            return;
        }

        //delay conversion
        var udelay = delay!=null?Convert.ToUInt64(delay):0;

        isPlaying = -1;
        audioSource.loop = loop!=null;
        audioSource.Play(udelay);
    }

    public override void Update() {
        base.Update();

        if (!init)
            return;

        //wait for potential delay
        if (isPlaying == -1 && audioSource.isPlaying) {
            isPlaying = 0;
        } else if (isPlaying == 0) {
            if (!string.IsNullOrEmpty(onSfxStart))
                ModuleParser.Parse(gameObject, onSfxStart);

            isPlaying = 1;
        } else if (!audioSource.isPlaying && isPlaying == 1) {
            if (!string.IsNullOrEmpty(onSfxEnd))
                ModuleParser.Parse(gameObject, onSfxEnd);

            isPlaying = -1;
        }

    }

    public override void SetDefaults () {
        base.SetDefaults();
    }
}
