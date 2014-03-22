using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlameDamager : ProjectileDamager {

    CapsuleCollider col;
    float firePulseTimer;
    float flameLifeTimer;
    float currentTimer=0f;
    List<GameObject> collidedObjects;

    void Awake()
    {
        col = GetComponent<CapsuleCollider>();
    }
    void Start()
    {
        collidedObjects = new List<GameObject>();
        currentTimer = firePulseTimer;
    }
    public void Init(GameObject _origin, int _damage,float _firePulseTimer, float _flameLifeTimer)
    {
        firePulseTimer=_firePulseTimer;
        flameLifeTimer = _flameLifeTimer;
        origin = _origin;
        damage = _damage;
        Destroy(gameObject, flameLifeTimer);
    }
	// Update is called once per frame
	void Update () {
        float dist = Vector3.Distance(transform.position, origin.transform.position);
        col.center = new Vector3(0.0f,0.0f,dist / -2f);
        col.height = dist;

        if (currentTimer >= firePulseTimer)
        {
            bool doneDamage = false;
            foreach (GameObject obj in collidedObjects.ToArray())
            {
                if (obj)
                {
                    obj.GetComponent<Health>().UpdateHealth(-damage);
                    doneDamage = true;
                }
            }
            if (doneDamage)
                currentTimer = 0f;
        }

        currentTimer += Time.deltaTime;
	}
    protected  override void OnTriggerEnter(Collider other)
    {
        if(origin)
        {
            switch(origin.tag)
            {
                case "Soldier":
                    if(  currentTimer>=firePulseTimer  &&(other.tag=="Enemy"||other.tag=="EnemyStructure"))
                    {
                        collidedObjects.Add(other.gameObject);
                    }
                    break;
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (origin)
        {
            switch (origin.tag)
            {
                case "Soldier":
                    if (other.tag == "Enemy" || other.tag == "EnemyStructure")
                    {
                        collidedObjects.Remove(other.gameObject);
                    }
                    break;
            }
        }
    }
}
