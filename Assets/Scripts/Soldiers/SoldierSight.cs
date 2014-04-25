using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoldierSight : Subject {

    public float sightRange;
    SphereCollider col;
    List<DroneBehavior> dronesInSight;
    List<SwarmSpawner> enemyStructsInSight;
    List<Enemy> enemiesInSight;

    void Awake()
    {
        col = GetComponent<SphereCollider>();
    }
	void Start () {
        enemiesInSight = new List<Enemy>();
        col.radius = sightRange;
    }
    void Update()
    {
        foreach(Enemy enemy in enemiesInSight)
        {
            if (enemy)
            {
                Health health_script = enemy.gameObject.GetComponent<Health>();
                if (health_script && !health_script.Alive)
                {
                    enemiesInSight.Remove(enemy);
                    Notify();
                    break;
                }
            }
        }
    }
    public void UpdateSight(float range)
    {
        sightRange = range;
        col.radius = sightRange;
    }
    
    public override void Notify()
    {
        foreach (Observer obs in observers)
        {
            obs.UpdateEnemiesInSight(enemiesInSight);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" || other.tag == "EnemyStructure")
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (enemy)
            {
                enemiesInSight.Add(enemy);
                Notify();
            }
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy" || other.tag == "EnemyStructure")
        {
            enemiesInSight.Remove(other.gameObject.GetComponent<Enemy>());
            Notify();
        }
        
    }


}
