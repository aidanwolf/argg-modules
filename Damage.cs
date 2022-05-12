using System.Collections;
using System.Collections.Generic;
using Shared;
using UnityEngine;
using System;

[RequireComponent(typeof(Collidable))]
[RequireComponent(typeof(Rigidbody))]
public class Damage : Module
{
    public int damage {get;set;}

    public float hitRate {get;set;}

    public string onDamage {get;set;}

    public Dictionary<string,Module[]> damagableObjects;

    private Collidable collidable;

    public override void Init()
    {

        collidable = Componentizer.DoComponent<Collidable>(gameObject,true);
        collidable.Init();

        base.Init();

        damagableObjects = new Dictionary<string,Module[]>();

        SetDefaults();
    }

    public override void SetDefaults() {
        base.SetDefaults();
        if (hitRate == 0)
            hitRate = 0.1f;

        //if we don't have a rigidbody we make our colliders triggers

        var physical = gameObject.GetComponent<Physical>();

        if (physical == null || !physical.init) {
            Debug.Log("Physical module none or off");
            var rigidbody = GetComponent<Rigidbody>();
            rigidbody.isKinematic = true;
        }
    }

    public override void Deinit() {
        base.Deinit();
        collidable.Deinit();
    }

    float lastHitTime = 0f;

    public override void Update()
    {
        base.Update();

        if (Time.time-lastHitTime < hitRate)
            return;

        if (damagableObjects == null || damagableObjects.Count == 0)
            return;
        
        foreach (Module[] modules in damagableObjects.Values) {
            foreach (Module module in modules) {
                var moduleName = module.GetType().Name;
                Type type = Type.GetType(moduleName);
                var prop = type.GetMethod("ReceiveDamage");
                if (module != null && prop != null) {
                    Debug.Log(moduleName + " has function ReceiveDamage");
                    prop.Invoke(module, new object[] {damage});
                }
            }
        }

        lastHitTime = Time.time;
        if (!string.IsNullOrEmpty(onDamage))
            ModuleParser.Parse(gameObject,onDamage);
    }

    private void OnCollisionEnter(Collision collision)
    {

        Debug.Log(gameObject.name + " Damage OnCollisionEnter!!!");
        if (!init)
            return;

        Module[] modules = collision.collider.transform.GetRootParent().gameObject.GetComponents<Module>();

        if (!damagableObjects.ContainsKey(collision.collider.name)) {
            damagableObjects.Add(collision.collider.name,modules);
        } else {
            damagableObjects[collision.collider.name] = modules;
        }
    }

    private void OnCollisionExit(Collision collision) {
         if (!init)
            return;  

        if (damagableObjects.ContainsKey(collision.collider.name))
            damagableObjects.Remove(collision.collider.name);

        if (damagableObjects.Count == 0)
            lastHitTime = 0;
    }
}
