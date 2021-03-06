﻿using UnityEngine;
using System.Collections;

public class Weapon_Lightning: Weapon {

    LineRenderer line;
    public float lineNoise = 1f;
    int length;
    bool firing=false;
    int enemiesLayer = 11;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
    }
    protected override void Start()
    {
        length = Mathf.RoundToInt(range)+1;
        base.Start();
    }
    void Update()
    {
        if(firing)
        {
            CheckForCollision();
            CreateBeamEffect();
        }
        else
        {
            line.enabled = false;
        }
        currentTimer += Time.deltaTime;
    }

    public override void Fire(GameObject origin)
    {
        firing = true;    
    }
    public override void StopFiring()
    {
        firing = false;
    }
    void CreateBeamEffect()
    {
        line.enabled = true;
        line.SetVertexCount(length);
        float noise = 0.1f;
        float noiseIncrement = lineNoise / (float)length;
        noiseIncrement *= 2f;
        line.SetPosition(0, shootPoint.position);
        for (int i = 1; i < length-1; i++)
        {
            Vector3 newPos = shootPoint.position;
            Vector3 offset = Vector3.zero;
            offset.x = newPos.x + i * shootPoint.forward.x + Random.Range(-noise, noise);
            offset.y = newPos.y + i * shootPoint.forward.y;
            offset.z = newPos.z + i * shootPoint.forward.z;
            newPos = offset;
            line.SetPosition(i, newPos);
            if(i>length/2)
                noise -= noiseIncrement;
            else
                noise += noiseIncrement;
        }
        line.SetPosition(length-1, shootPoint.position+shootPoint.forward*(length-1));
    }
    void CheckForCollision()
    {
        bool didDamage = false;
        RaycastHit[] hits;
        hits = Physics.RaycastAll(shootPoint.position, shootPoint.forward, 100f, 1 << enemiesLayer);
        int i = 0;
        while (i < hits.Length)
        {

            RaycastHit hit = hits[i];
            if (currentTimer >= reloadTimer && (hit.transform.tag == "Enemy" || hit.transform.tag == "EnemyStructure"))
            {
                hit.transform.GetComponent<Health>().UpdateHealth(-damage);
                didDamage = true;
            }
            i++;
        }
        if (didDamage)
            currentTimer = 0f;
    }
}
