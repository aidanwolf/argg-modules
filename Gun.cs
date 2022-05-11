using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Shared;
using System;

public class Gun : Module
{
    //Gun.cs is one of the more complex modules, but..
    //The goal is simple: affect other objects /at a distance/
    //The most basic version of this is a raycaster, therefore raycaster is default

    //Recommend the FBX has a child object named "BARREL" to direct the projectile

    //DEFAULT VALUES
    //--------------
    //projectile=ray (opt: projectile itemId)
    //rateOfFire=constant (opt:number in ms)
    //ammo=none/unlimited (opt:ammo itemId)
    //fireMode=auto (opt:semiauto)
    //sfx=none (opt: soundId)

    //Gun[rayColor:,rayTexture:,rateOfFire:,Projectile:,Force:,Ammo:]

    private ModuleParser.GameObjectWithScript _projectile;
    public ModuleParser.GameObjectWithScript? projectile {
        get { 
            return _projectile;
        }
        set {
            Debug.Log("PROJECTILE SET!!!!");
            _projectile = value;
            _projectile.gObj.SetActive(false);
        }
    }
    public Vector4? rayColor {get; set;}
    public Texture2D? rayTexture {get; set;}
    public float damage {get; set;}
    public float rateOfFire {get; set;}
    public bool semiAuto {get;set;}
    public string shootAnim {get; set;}


    private float rayForce {get; set;}

    public GameObject triggerButton;
    public GameObject EFFECT;
    public GameObject BARREL;

    private bool shooting = false;
    private EventTrigger triggerScript;

    private LineRenderer lineRenderer;

    private bool shootOnce = false;

    private Anim animModule;

    public override void Init () {
        base.Init();

        SetDefaults();

        EFFECT = transform.FindDeepChild("EFFECT")?.gameObject;
        BARREL = transform.FindDeepChild("BARREL")?.gameObject;

        if (BARREL == null) {
            Debug.LogWarning("MISSING BARREL: this asset was not set up properly to work as a gun. Using default gameobject");
            BARREL = gameObject;
        }
        if (EFFECT == null) {
            Debug.LogWarning("MISSING EFFECT: Adding line renderer");
            EFFECT = AddLineRenderer();
        }

        if (!string.IsNullOrEmpty(shootAnim)) {
            Debug.Log("Has shoot anim, adding Anim module");
            animModule = Componentizer.DoComponent<Anim>(gameObject,true);
        }

        ToggleEffects(false);

        triggerButton = GlobalData.GlobalGameObject("Trigger");
        triggerButton.SetActive(true);
        triggerScript = triggerButton.GetComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((data)=>{startShooting((PointerEventData)data);});
        triggerScript.triggers.Add(entry);

        EventTrigger.Entry exit = new EventTrigger.Entry();
        exit.eventID = EventTriggerType.PointerUp;
        exit.callback.AddListener((data)=>{endShooting((PointerEventData)data);});
        triggerScript.triggers.Add(exit);
    }

    public override void SetDefaults () {
        base.SetDefaults();
        if (rayForce == 0)
            rayForce = 100f;

        if (rayColor == null)
            rayColor = new Vector4(57f/255f,1f,179f/255f,1f);

        if (damage == 0)
            damage = 1;
    }

    public override void Update () {
        //if init = false, don't continue
        base.Update();

        if (!init)
            return;

        ToggleEffects(shooting);

        Vector3 muzzlePos = BARREL.transform.position;
        Vector3 fwd = BARREL.transform.TransformDirection(Vector3.forward);
        Vector3 fwdWithDist = fwd * 5000f;
        RaycastHit hit = new RaycastHit();

        if (!shooting)
            return;

        if (projectile != null) {

            if (lineRenderer) {
                lineRenderer.positionCount = 0;
            }

            GameObject projectileCopy = Instantiate(projectile.gObj);
            projectileCopy.transform.position = BARREL.transform.position;
            projectileCopy.transform.rotation = BARREL.transform.rotation;
            projectileCopy.SetActive(true);

            ModuleParser.Parse(projectileCopy, projectile.script);

            shooting = false;
            shootOnce = true;
            return;
        }

        Debug.DrawRay(muzzlePos, fwdWithDist, Color.yellow);

        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(BARREL.transform.position, fwd, out hit, 5000f)) {
            Debug.Log("Did Hit");

            Module[] modules;

            if (hit.rigidbody) {
                hit.rigidbody.AddForceAtPosition (fwd * rayForce, hit.point);
                modules = hit.rigidbody.gameObject.GetComponents<Module>();
            } else {
                modules = hit.collider.transform.GetRootParent().gameObject.GetComponents<Module>();
            }

            foreach (Module module in modules) {
                var moduleName = module.GetType().Name;
                Debug.Log(moduleName);
                Type type = Type.GetType(moduleName);
                var prop = type.GetMethod("ReceiveDamage");
                if (prop != null) {
                    Debug.Log(moduleName + " has function ReceiveDamage");
                    prop.Invoke(module, new object[] {damage});
                }
            }
        }

        if (lineRenderer) {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, BARREL.transform.position);
            lineRenderer.SetPosition(1, (hit.point!=Vector3.zero)?hit.point:(fwdWithDist));
        }

        if (animModule) {
            animModule.play = shootAnim;
        }

    }

    public override void Deinit () {
        if (init) {
            triggerScript.triggers.Clear();
            triggerButton.SetActive(false);
        }
        base.Deinit();
    }

    private void startShooting (PointerEventData data) {
        if (!shootOnce)
        shooting = true;
    }

    private void endShooting (PointerEventData data) {
        shooting = false;
        shootOnce = false;
        ToggleEffects(false);
    }


    private GameObject AddLineRenderer () {
        Debug.Log("AddLineRenderer");
        //if we have a line texture or color, we apply
        lineRenderer = Componentizer.DoComponent<LineRenderer>(BARREL,true);
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.sharedMaterial = Resources.Load<Material>("rayDefault");

        Debug.Log("rayColor = " + rayColor);

        if (rayColor != null) {
            lineRenderer.startColor = (Color)rayColor;
            lineRenderer.endColor = (Color)rayColor;
        }
        // if (rayTexture != null) {
        //     lineRenderer.material.mainTexture = rayTexture;
        // }
        return lineRenderer.gameObject;
    }

    private void ToggleEffects (bool shooting) {
        if (EFFECT)
            EFFECT.SetActive(shooting);
        if (BARREL)
            BARREL.SetActive(shooting);
    }
}
