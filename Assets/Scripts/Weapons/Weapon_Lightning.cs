using UnityEngine;
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
        float noise = 0.3f;
        float noiseIncrement = lineNoise / (float)length;
        for (int i = 0; i < length; i++)
        {
            Vector3 newPos = shootPoint.position;
            Vector3 offset = Vector3.zero;
            offset.x = newPos.x + i * shootPoint.forward.x + Random.Range(-noise, noise);
            offset.y = newPos.y + i * shootPoint.forward.y;// +Random.Range(-lineNoise, lineNoise);
            offset.z = newPos.z + i * shootPoint.forward.z;// +Random.Range(-lineNoise, lineNoise);
            newPos = offset;
            line.SetPosition(i, newPos);
            noise += noiseIncrement;
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
        //RaycastHit hit;
        //Ray ray = new Ray(shootPoint.position, shootPoint.forward);
        //Debug.DrawLine(shootPoint.position, shootPoint.forward.normalized * range);
        //if (Physics.Raycast(ray, out hit, range+1))
        //{
        //    GameObject other = hit.transform.gameObject;
        //    if (currentTimer >= reloadTimer && (other.tag == "Enemy" || other.tag == "EnemyStructure"))
        //    {
        //        Debug.Log(damage);
        //        other.GetComponent<Health>().UpdateHealth(-damage);
        //        currentTimer = 0f;
        //    }
        //}
        
    }
}
