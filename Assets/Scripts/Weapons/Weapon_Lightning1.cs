using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Weapon_Lightning1 : Weapon
{

    LineRenderer line;
    public float lineNoise = 1f;
    int length;
    bool firing = false;
    int enemiesLayer = 11;
    public GameObject lightningSpawn;
    List<LightningSpawn> lightningSpawns;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
    }
    protected override void Start()
    {
        lightningSpawns = new List<LightningSpawn>();

        length = Mathf.RoundToInt(range) + 1;
        base.Start();
    }
    void Update()
    {
        if (firing)
        {
            CheckForCollision();
            CreateBeamEffect();
        }
        else
        {
            line.enabled = false;
            for (int i = 0; i < lightningSpawns.Count; i++)
            {
                Destroy(lightningSpawns[i].gameObject);
            }
            lightningSpawns.Clear();
        }
        currentTimer += Time.deltaTime;
    }

    public override void Fire(GameObject origin)
    {
        if(!firing)
        {
            AudioManager.Instance.PlaySound(AudioManager.Sound.LightningGun,.4f, true);
        }
        firing = true;
    }
    public override void StopFiring()
    {
        if (firing)
            AudioManager.Instance.StopSound(AudioManager.Sound.LightningGun);
        firing = false;
    }
    void CreateBeamEffect()
    {
        int numBeams = Random.Range(2, 5);
        while (lightningSpawns.Count < numBeams)
        {
            GameObject lightSpawn = Instantiate(lightningSpawn, shootPoint.position, shootPoint.rotation) as GameObject;
            lightSpawn.transform.parent = transform;
            lightningSpawns.Add(lightSpawn.GetComponent<LightningSpawn>());
        }
        while (lightningSpawns.Count > numBeams)
        {
            LightningSpawn lightSpawn = lightningSpawns[lightningSpawns.Count - 1];
            lightningSpawns.Remove(lightSpawn);
            Destroy(lightSpawn.gameObject);
        }
        foreach (LightningSpawn lightSpawn in lightningSpawns)
        {
            lightSpawn.CreateBeamEffect(Mathf.RoundToInt(range) + 1, lineNoise);
        }

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
