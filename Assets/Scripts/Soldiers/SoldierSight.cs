using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoldierSight : Subject {

    public float sightRange;
    SphereCollider col;
    List<DroneBehavior> dronesInSight;
    List<SwarmSpawner> enemyStructsInSight;

    List<Enemy> enemiesInSight;

    public enum NotifyFlag { Drones, Structs}

	// Use this for initialization
    void Awake()
    {
        col = GetComponent<SphereCollider>();
    }
	void Start () {
        //dronesInSight = new List<DroneBehavior>();
        enemiesInSight = new List<Enemy>();
        col.radius = sightRange;
    }
    void Update()
    {
        //foreach (DroneBehavior drone in dronesInSight)
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
    //public void Notify(NotifyFlag flag)
    //{
    //    switch (flag)
    //    {
    //        case NotifyFlag.Drones:
    //            foreach (Observer obs in observers)
    //            {
    //                obs.UpdateDronesInSight(dronesInSight);
    //            }
    //            break;
    //        case NotifyFlag.Structs:
    //            break;
    //        default:
    //            break;
    //    }
        
    //}
    public override void Notify()
    {
        foreach (Observer obs in observers)
        {
            obs.UpdateEnemiesInSight(enemiesInSight);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        //if(other.tag=="Enemy")
        //{
        //    DroneBehavior drone = other.gameObject.GetComponent<DroneBehavior>();
        //    if (drone)
        //    {
        //        dronesInSight.Add(drone);
        //        Notify(NotifyFlag.Drones);
        //    }
        //}
        //if (other.tag == "EnemyStructure")
        //{
        //    SwarmSpawner spawner = other.gameObject.GetComponent<SwarmSpawner>();
        //    if (spawner)
        //    {
        //        enemyStructsInSight.Add(spawner);
        //        Notify(NotifyFlag.Structs);
        //    }
        //}
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
        //if (other.tag == "Enemy")
        //{
        //    dronesInSight.Remove(other.gameObject.GetComponent<DroneBehavior>());
        //    Notify(NotifyFlag.Drones);
        //}
        //if (other.tag == "EnemyStructure")
        //{
        //    enemyStructsInSight.Remove(other.gameObject.GetComponent<SwarmSpawner>());
        //    Notify(NotifyFlag.Structs);
        //}
        if (other.tag == "Enemy" || other.tag == "EnemyStructure")
        {
            enemiesInSight.Remove(other.gameObject.GetComponent<Enemy>());
            Notify();
        }
        
    }


}
