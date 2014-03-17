using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class NodeWatcher : Observer 
{

    //public Transform[] wayPoints;
    KDTree tree;
    Vector3[] posArr;
    LineRenderer line;
    public Transform targetPoint;
    public int nearest;
    

    void Awake()
    {
        line = GetComponent<LineRenderer>();
    }
    void Start()
    {
        //subjects = new List<Subject>();

        posArr = new Vector3[subjects.Count];

        for (int i = 0; i < posArr.Length; i++)
        {
            posArr[i] = subjects[i].transform.position;
        }

        tree = KDTree.MakeFromPoints(posArr);
    }
    void Update()
    {
        nearest = tree.FindNearest(transform.position);
        line.SetPosition(0, transform.position);
        line.SetPosition(1, posArr[nearest]);
    }

    override public void UpdateSubject(Subject _subject)
    {
        //foreach (Subject subject in subjects)
        for (int i = 0; i < subjects.Count; i++)
        {
            if(subjects[i]==_subject)
            {
                subjects[i] = _subject;
                posArr[i] = _subject.transform.position;
                tree = KDTree.MakeFromPoints(posArr);
                return;
            }
        }
    }
}
