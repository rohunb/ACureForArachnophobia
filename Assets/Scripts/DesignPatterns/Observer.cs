using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//public abstract class Observer {
public abstract class Observer: MonoBehaviour
{
    public List<Subject> subjects;

    abstract public void UpdateSubjectList(Subject subject);
}
