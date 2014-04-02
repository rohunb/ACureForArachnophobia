using UnityEngine;
using System.Collections;

public class Health : Subject
{

    public int maxHealth;

    [SerializeField]
    private int health;

    public int GetHealth
    {
        get { return health; }
    }

    private bool alive;

    public bool Alive
    {
        get { return alive; }
    }

    DroneBehavior drone;
    BoxCollider boxCol;

    SwarmSpawner spawner;
    SphereCollider sphereCol;

    EnemyController enemyController;

    void Awake()
    {
        switch (tag)
        {
            case "Enemy":
                drone = gameObject.GetComponent<DroneBehavior>();
                boxCol = gameObject.GetComponent<BoxCollider>();
                break;
            case "EnemyStructure":
                spawner = gameObject.GetComponent<SwarmSpawner>();
                boxCol = GetComponent<BoxCollider>();
                break;
            default:
                break;
        }

    }
    void Start()
    {
        health = maxHealth;
        alive = true;
    }
    public void UpdateHealth(int amount)
    {
        if (alive)
        {
            health += amount;
            if (health <= 0)
            {
                Die();
            }
            health = Mathf.Clamp(health, 0, maxHealth);
            NotifyHealthUpdate();
        }
    }
    void Die()
    {
        //death
        alive = false;
        switch (tag)
        {
            case "Enemy":
                Notify();
                drone.swarm.drones.Remove(drone.gameObject);
                if (Random.value > 0.5f)
                    animation.CrossFade("death1");
                else
                    animation.CrossFade("death2");
                drone.enabled = false;
                rigidbody.velocity = Vector3.zero;
                rigidbody.angularVelocity = Vector3.zero;
                Invoke("Kill", 1.0f);
                break;
            case "EnemyStructure":
                NotifyDead();
                spawner.enabled = false;
                Invoke("Kill", 0.0f);
                break;
            default:
                break;
        }
    }
    void Kill()
    {
        switch (tag)
        {
            case "Enemy":
                boxCol.enabled = false;
                //Invoke("ReturnToPool", 1.0f);
                //ReturnToPool();
                Destroy(gameObject, 1.0f);
                break;
            case "EnemyStructure":
                boxCol.enabled = false;
                foreach (GameObject drone in spawner.drones.ToArray())
                {
                    //drone.GetComponent<Health>().ReturnToPool();
                    drone.GetComponent<Health>().UpdateHealth(-500);
                    
                }
                Destroy(gameObject, 2.0f);
                break;
            default:
                break;
        }

    }
    public void NotifyHealthUpdate()
    {
        foreach (Observer obs in observers)
        {
            obs.UpdateLowestHPSoldier();
        }
    }
    public void NotifyDead()
    {
        foreach (Observer obs in observers)
        {
            obs.UpdateSpawnerDeath(spawner);
        }
    }
    public override void Notify()
    {
        foreach (Observer obs in observers)
        {
            obs.UpdateNumEnemies(-1);
        }
    }
    void ReturnToPool()
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        ObjectPool.instance.PoolObject(gameObject);
    }
    
}
