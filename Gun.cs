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

    //Gun[rayColor:,rayTexture:,rateOfFire:,Projectile:,Force:,Ammo:]

    public string onShoot {get;set;}

    public string onShootEnd {get;set;}

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

    private float rayForce {get; set;}

    public GameObject triggerButton;
    public GameObject EFFECT;
    public GameObject BARREL;

    private bool shooting = false;
    private EventTrigger triggerScript;

    private LineRenderer lineRenderer;

    private bool shootOnce = false;

    private Anim animModule;
    private float lastFireTime = 0;

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

        if (rateOfFire == 0)
            rateOfFire = 0.1f;
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

        if (Time.time-lastFireTime <= rateOfFire) {
            if (lineRenderer) {
                lineRenderer.positionCount = 0;
            }
            return;
        }

        if (projectile != null) {

            lastFireTime = Time.time;

            GameObject projectileCopy = Instantiate(projectile.gObj);
            projectileCopy.transform.position = BARREL.transform.position + BARREL.transform.forward*projectileCopy.getBounds().extents.z;
            projectileCopy.transform.rotation = BARREL.transform.rotation;
            projectileCopy.SetActive(true);

            //projectiles are limited to 10s of life
            Destroy(projectileCopy,10f);

            ModuleParser.Parse(projectileCopy, projectile.script);

            if (semiAuto) {
                shooting = false;
                shootOnce = true;
            }

            if (!string.IsNullOrEmpty(onShoot))
                ModuleParser.Parse(gameObject, onShoot);

        } else {

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
        }

        if (!string.IsNullOrEmpty(onShoot))
            ModuleParser.Parse(gameObject, onShoot);
    }

    public override void Deinit () {
        if (init) {
            Debug.Log("GUN DEINIT!");
            triggerScript.triggers.Clear();
            triggerButton.SetActive(false);
        }
        base.Deinit();
    }

    private void startShooting (PointerEventData data) {
        Debug.Log("startShooting");

        if (!shootOnce)
        shooting = true;
    }

    private void endShooting (PointerEventData data) {
        shooting = false;
        shootOnce = false;
        ToggleEffects(false);

        if (!string.IsNullOrEmpty(onShootEnd))
            ModuleParser.Parse(gameObject, onShootEnd);
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
    }
}
