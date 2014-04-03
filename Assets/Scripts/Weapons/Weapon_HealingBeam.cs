﻿using UnityEngine;
using System.Collections;

public class Weapon_HealingBeam : Weapon {

    LineRenderer line;
    public float lineNoise = 1f;
    int length;
    bool firing = false;
    bool healing = false;
    public int healAmount = 1;
    public float healInterval = 1f;

    int enemiesLayer = 11;
    int playerLayer = 9;

    public Color healColour = Color.green;
    public Color fireColour = Color.yellow;

    Transform target;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
    }
    protected override void Start()
    {
        length = Mathf.RoundToInt(range) + 1;
        base.Start();
    }

    void Update()
    {
        if(healing)
        {
            HealTarget();
            CreateBeamEffect();
        }
        else if (firing)
        {
            DamageTarget();
            CreateBeamEffect();
        }

        else
        {
            line.enabled = false;
        }
        currentTimer += Time.deltaTime;
    }
    void HealTarget()
    {
        if(target && currentTimer>=healInterval)
        {
            target.gameObject.GetComponent<Health>().UpdateHealth(healAmount);
            currentTimer = 0f;
        }
    }
    void DamageTarget()
    {
        if(target && currentTimer>=reloadTimer)
        {
            target.gameObject.GetComponent<Health>().UpdateHealth(-damage);
            currentTimer = 0f;
        }
    }
    public void Fire(Transform _target)
    {
        target = _target;
        firing = true;
        line.renderer.material.SetColor("_TintColor", Color.yellow);
    }
    public override void StopFiring()
    {
        firing = false;
    }
    public void Heal(Transform _target)
    {
        target = _target;
        healing = true;
        line.renderer.material.SetColor("_TintColor", Color.green);
    }
    public void StopHealing()
    {
        healing = false;
    }
    void CreateBeamEffect()
    {
        line.enabled = true;
        length = Mathf.RoundToInt(Vector3.Distance(target.position, shootPoint.position));
        length++;
        line.SetVertexCount(length);
        for (int i = 0; i < length; i++)
        {
            Vector3 newPos = shootPoint.position;
            Vector3 offset = Vector3.zero;
            offset.x = newPos.x + i * shootPoint.forward.x + Random.Range(-lineNoise, lineNoise);
            offset.y = newPos.y + i * shootPoint.forward.y;// +Random.Range(-lineNoise, lineNoise);
            offset.z = newPos.z + i * shootPoint.forward.z;// +Random.Range(-lineNoise, lineNoise);
            newPos = offset;
            line.SetPosition(i, newPos);
            //noise += noiseIncrement;
        }
        //line.SetPosition(length - 1, target.position);
        
        //line.SetVertexCount(2);
        //line.SetPosition(0, transform.position);
        //line.SetPosition(1, target.position);
    }
    
}