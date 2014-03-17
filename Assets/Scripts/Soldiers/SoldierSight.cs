using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoldierSight : Subject {

    public float sightRange;
    SphereCollider col;
    List<DroneBehavior> dronesInSight;
	// Use this for initialization
    void Awake()
    {
        col = GetComponent<SphereCollider>();
    }
	void Start () {
        dronesInSight = new List<DroneBehavior>();
        col.radius = sightRange;
    }
    public override void Notify()
    {
        foreach (Observer obs in observers)
        {
            obs.UpdateDronesInSight(dronesInSight);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Enemy")
        {
            DroneBehavior drone = other.gameObject.GetComponent<DroneBehavior>();
            if (drone)
            {
                dronesInSight.Add(drone);
                Notify();
            }
            
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            dronesInSight.Remove(other.gameObject.GetComponent<DroneBehavior>());
            Notify();
        }
    }


}
