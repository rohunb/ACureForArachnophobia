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
    public float abandonTargetRange = 50f;
    bool soldierInSight = false;

	public GameObject prefab;

    public Transform target;
    public Soldier nearestSoldier;

    bool spawnTimerUp = true;
    public bool canSpawn = true;

    EnemyController enemyController;
    UpgradeManager upgradeManager;
    
    void Awake()
    {
        enemyController = GameObject.FindObjectOfType<EnemyController>();
        upgradeManager = GameObject.FindObjectOfType<UpgradeManager>();
    }

	protected virtual void Start () {
		if (prefab == null)
		{
			Debug.Log("Please assign a drone prefab.");
			return;
		}
		drones = new List<GameObject>();
	}
    IEnumerator SpawnDrone()
    {
        GameObject droneTemp = (GameObject)GameObject.Instantiate(prefab);
        DroneBehavior db = droneTemp.GetComponent<DroneBehavior>();
        Health health = droneTemp.GetComponent<Health>();
        health.Attach(enemyController);
        health.Attach(upgradeManager);
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

	protected virtual void Update () {
        if (spawnTimerUp && canSpawn)
        {
            StartCoroutine("SpawnDrone");
            spawnTimerUp = false;
        }
        
	}

    public void UpdateDronesTarget(Transform _target)
    {
        target = _target;
        if (target)
        {
            foreach (GameObject drone in drones)
            {
                DroneBehavior db = drone.GetComponent<DroneBehavior>();
                db.destination = _target;

                float distToTarget = Vector3.Distance(_target.position, transform.position);
                if (distToTarget < droneSightRange)
                {
                    soldierInSight = true;
                }
                if (soldierInSight && distToTarget > abandonTargetRange)
                {
                    soldierInSight = false;
                }

                db.soldierInSight = soldierInSight;
            }
        }
        else
        {
            soldierInSight = false;
        }
    }

    public override void Notify()
    {
        foreach (Observer obs in observers)
        {
            obs.UpdateNumEnemies(1);
        }
    }
}
