using UnityEngine;
using System.Collections;

public class Waypoints : MonoBehaviour {

	//int index = 10;
    //float scale = 50.0f;
public Transform[] wayPoints;
KDTree tree;
public Transform targetPoint;
public int nearest;
Vector3[] posArr;
LineRenderer line;
void Awake () 
{   
    //wayPoints = new Vector3[index];
    line = GetComponent<LineRenderer>();
    posArr = new Vector3[wayPoints.Length];
    
        for (int i = 0; i < wayPoints.Length; i++)
        {
            posArr[i] = wayPoints[i].position;
        }



    tree=KDTree.MakeFromPoints(posArr);
    
    

}
void Update()
{
    nearest = tree.FindNearest(targetPoint.position);
    line.SetPosition(0, targetPoint.position);
    line.SetPosition(1, posArr[nearest]);
}


//void OnDrawGizmos()
//{

//    Gizmos.color = Color.white;

//    for (int i = 0; i < wayPoints.Length; i++)
//    {

//        Gizmos.DrawSphere(wayPoints[i].position, 0.5f);
//        Gizmos.DrawLine(targetPoint.position, posArr[nearest]);
//    }

//}
}
