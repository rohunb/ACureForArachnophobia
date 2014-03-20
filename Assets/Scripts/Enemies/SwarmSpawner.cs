using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwarmSpawner : Enemy {

    public int droneCount = 50;
	public float spawnRadius = 100f;
    public float spawnTimer = 0.05f;
    public List<GameObject> drones;

	public Vector2 swarmBounds = new Vector2(300f, 300f);

	public GameObject prefab;

    public Transform destination;

    bool spawnTimerUp = true;
    public bool canSpawn = true;


    EnemyController enemyController;

    void Awake()
    {
        enemyController = GameObject.FindObjectOfType<EnemyController>();
    }

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
        droneTemp.GetComponent<Health>().Attach(enemyController);
        db.drones = this.drones;
        db.swarm = this;
        db.destination = destination;
        Vector2 pos = new Vector2(transform.position.x, transform.position.z) + Random.insideUnitCircle * spawnRadius;
        droneTemp.transform.position = new Vector3(pos.x, transform.position.y, pos.y);
        droneTemp.transform.parent = transform;
        drones.Add(droneTemp);
        Notify();
        yield return new WaitForSeconds(spawnTimer);
        spawnTimerUp = true;
    }
	// Update is called once per frame
	protected virtual void Update () {
        if (spawnTimerUp && canSpawn)//drones.Count<droneCount)
        {
            StartCoroutine("SpawnDrone");
            spawnTimerUp = false;
        }
	}
    public override void Notify()
    {
        foreach (Observer obs in observers)
        {
            obs.UpdateNumEnemies(1);
        }
    }
    
    //protected virtual void OnDrawGizmosSelected()
    //{
    //    Gizmos.DrawWireCube(transform.position, new Vector3(swarmBounds.x, 0f, swarmBounds.y));
    //    Gizmos.DrawWireSphere(transform.position, spawnRadius);
    //}
}
