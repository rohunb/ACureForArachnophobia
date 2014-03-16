using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Subject:MonoBehaviour {

    private List<Observer> observers = new List<Observer>();
    
    public void Attach(Observer observer)
    {
        observers.Add(observer);
    }

    public void Detach(Observer observer)
    {
        observers.Remove(observer);
    }

    public void Notify()
    {
        foreach (Observer obs in observers)
        {
            obs.UpdateSubjectList(this);
        }
    }
}
