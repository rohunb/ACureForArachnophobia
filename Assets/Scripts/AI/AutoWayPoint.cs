using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AutoWayPoint : MonoBehaviour {
	
	//var header;
	static AutoWayPoint[] waypoints = null; //holds all of the WayPoint objects in the scene
	public AutoWayPoint[] connected;//an array showing all the different paths the character can take
	static float kLineOfSightCapsuleRadius = 0.25f;
	
	//FPS Tutorial from the Unity3D site
	//uses the current waypoint that the PatrolBot is at
	static public AutoWayPoint FindClosest (Vector3 pos)
	{	
		// The closer two vectors, the larger the dot product will be.
		AutoWayPoint closestWayPoint = null;
		
		//for each waypoint in the array of waypoints it will find the distance between the current position and the position that we are at "pos"
		foreach (AutoWayPoint cur in waypoints)
		{
			float distance = Vector3.Distance(cur.transform.position, pos);
			
			if (distance < 100.0f) 		//if the bot is less than a 100 unit away
				closestWayPoint = cur;
		}
		return closestWayPoint;
	}
	
	[ContextMenu ("Update Waypoints")]
	void UpdateWaypoints() 
	{
		RebuildWaypointList();
	}
	
	void Awake () 
	{
		RebuildWaypointList();
	}
	
	
	// Draw the waypoint pickable gizmo
	void OnDrawGizmos () 
	{
		Gizmos.DrawIcon (transform.position, "Waypoint.tif");
	}

// Draw the waypoint lines only when you select one of the waypoints
	void OnDrawGizmosSelected () 
	{
		//if (waypoints.Length == 0)
		if (waypoints == null)
			RebuildWaypointList();
		
		foreach (AutoWayPoint p in connected) 
		{
			if (Physics.Linecast(transform.position, p.transform.position)) 
			{
				Gizmos.color = Color.red;
				Gizmos.DrawLine (transform.position, p.transform.position);
			} 
			else 
			{
				Gizmos.color = Color.green;
				Gizmos.DrawLine (transform.position, p.transform.position);
			}
		}
	}
	
	void RebuildWaypointList () 
	{
		//var objects : Object[] = FindObjectsOfType(AutoWayPoint);	//locates all GameObjects with the Type "AutoWayPoint" and puts them in the array
		waypoints = FindObjectsOfType(typeof(AutoWayPoint)) as AutoWayPoint[];	//copies the array into the waypoint array that was created
		
		foreach (AutoWayPoint point in waypoints) 
			point.RecalculateConnectedWaypoints();
	}
	
	void RecalculateConnectedWaypoints ()
	{
		List<AutoWayPoint> connectionList = new List<AutoWayPoint>();
		
		foreach (AutoWayPoint other in waypoints) 
		{
			// Don't connect to ourselves
			if (other == this)
				continue;
			
			// Do we have a clear line of sight?
			if (!Physics.CheckCapsule(transform.position, other.transform.position, kLineOfSightCapsuleRadius)) 
				connectionList.Add(other);
		}	
		
		connected = connectionList.ToArray();
	}
}