using System.Collections;
using System.Collections.Generic;
using Shared;
using UnityEngine;
using System;

[RequireComponent(typeof(Collidable))]
public class Damage : Module
{
    public int damage {get;set;}

    public float hitRate {get;set;}

    public string onDamage {get;set;}

    public Dictionary<string,HealthSystem> healthSystems;

    public override void Init()
    {
        base.Init();

        healthSystems = new Dictionary<string,HealthSystem>();

        SetDefaults();
    }

    float lastHitTime = 0f;

    public override void Update()
    {
        base.Update();
        if (Time.time-lastHitTime >= hitRate) {    
            
            foreach (HealthSystem healthSystem in healthSystems.Values) {
                if (healthSystem != null)
                    healthSystem.ReceiveDamage(damage);
            }

            lastHitTime = Time.time;
            if (!string.IsNullOrEmpty(onDamage))
                ModuleParser.Parse(gameObject,onDamage);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!init)
            return;

        if (!healthSystems.ContainsKey(collision.collider.name))
            healthSystems.Add(collision.collider.name,collision.collider.transform.GetRootParent().gameObject.GetComponent<HealthSystem>());
    }

    void OnCollisionExit(Collision collision) {
         if (!init)
            return;  

        if (healthSystems.ContainsKey(collision.collider.name))
            healthSystems.Remove(collision.collider.name);

        if (healthSystems.Count == 0)
            lastHitTime = 0;
    }
}
