using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//public abstract class Observer {
public abstract class Observer: MonoBehaviour
{
    public List<Subject> subjects;

    virtual public void UpdateSubject(Subject subject){}
    virtual public void UpdateDronesInSight(List<DroneBehavior> drones){}
}
