using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwarmSpawner : Enemy {

    public int droneCount = 50;
	public float spawnRadius = 100f;
    public float spawnTimer = 0.05f;
    public List<GameObject> drones;

	public Vector2 swarmBounds = new Vector2(300f, 300f);
    public float droneSightRange = 30f;

	public GameObject prefab;

    public Transform target;
    public Soldier nearestSoldier;

    bool spawnTimerUp = true;
    public bool canSpawn = true;

    EnemyController enemyController;
    LineRenderer line;
    
    void Awake()
    {
        enemyController = GameObject.FindObjectOfType<EnemyController>();
        line = GetComponent<LineRenderer>();
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
        db.destination = target;
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
        if (nearestSoldier)
        {
            line.SetPosition(0, transform.position);
            line.SetPosition(1, nearestSoldier.transform.position);
        }
	}

    public void UpdateDronesTarget(Transform _target)
    {
        target = _target;
        foreach (GameObject drone in drones)
        {
            DroneBehavior db=drone.GetComponent<DroneBehavior>();
            db.destination = _target;
            if(Vector3.Distance(_target.position,transform.position)<droneSightRange)
            {
                db.moveToWeight = 1f;
            }
            else
            {
                db.moveToWeight = 0f;
            }
        }
    }

    public override void Notify()
    {
        foreach (Observer obs in observers)
        {
            obs.UpdateNumEnemies(1);
        }
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(swarmBounds.x, 0f, swarmBounds.y));
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
