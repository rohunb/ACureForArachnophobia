using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwarmSpawner : MonoBehaviour {

    public int droneCount = 50;
	public float spawnRadius = 100f;
    public float spawnTimer = 0.05f;
    public List<GameObject> drones;

	public Vector2 swarmBounds = new Vector2(300f, 300f);

	public GameObject prefab;

    public Transform destination;

    bool canSpawn = true;
    

	protected virtual void Start () {
		if (prefab == null)
		{
			// end early
			Debug.Log("Please assign a drone prefab.");
			return;
		}

		// instantiate the drones
		GameObject droneTemp;
		drones = new List<GameObject>();

        
        //for (int i = 0; i < droneCount; i++)
        //{
        //    droneTemp = (GameObject) GameObject.Instantiate(prefab);
        //    DroneBehavior db = droneTemp.GetComponent<DroneBehavior>();
        //    db.drones = this.drones;
        //    db.swarm = this;
        //    db.destination = destination;

        //    // spawn inside circle
        //    Vector2 pos = new Vector2(transform.position.x, transform.position.z) + Random.insideUnitCircle * spawnRadius;
        //    droneTemp.transform.position = new Vector3(pos.x, transform.position.y, pos.y);
        //    droneTemp.transform.parent = transform;
			
        //    drones.Add(droneTemp);
        //}
	}
    IEnumerator SpawnDrone()
    {
        GameObject droneTemp;
        droneTemp = (GameObject)GameObject.Instantiate(prefab);
        DroneBehavior db = droneTemp.GetComponent<DroneBehavior>();
        db.drones = this.drones;
        db.swarm = this;
        db.destination = destination;
        Vector2 pos = new Vector2(transform.position.x, transform.position.z) + Random.insideUnitCircle * spawnRadius;
        droneTemp.transform.position = new Vector3(pos.x, transform.position.y, pos.y);
        droneTemp.transform.parent = transform;
        drones.Add(droneTemp);

        yield return new WaitForSeconds(spawnTimer);
        canSpawn = true;
    }
	// Update is called once per frame
	protected virtual void Update () {
        if (canSpawn && drones.Count<droneCount)
        {
            StartCoroutine("SpawnDrone");
            canSpawn = false;
        }
	}
    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 150, 150));
        GUILayout.BeginVertical();
        GUILayout.Label("Num Spiders: " + drones.Count);
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
	protected virtual void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireCube(transform.position, new Vector3(swarmBounds.x, 0f, swarmBounds.y));
		Gizmos.DrawWireSphere(transform.position, spawnRadius);
	}
}
