using UnityEngine;
using System.Collections;

public class Node : Subject {
    Vector3 initialPos;
    void Start()
    {
        initialPos = transform.position;
    }
    void Update()
    {
        if(transform.position!=initialPos)
        {
            initialPos = transform.position;
            base.Notify();
        }
    }
}
