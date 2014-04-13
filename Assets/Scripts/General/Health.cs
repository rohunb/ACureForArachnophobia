using UnityEngine;
using System.Collections;

public class Health : Subject
{
    public GameObject bloodSplatterPrefab;
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

    Soldier soldier;
    SoldierManager soldierManager;

    public float particleTimer = .5f;
    float currentTimer = 0f;

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
            case "Soldier":
                soldier = GetComponent<Soldier>();
                soldierManager = GameObject.FindObjectOfType<SoldierManager>();
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
    void Update()
    {
        currentTimer += Time.deltaTime;
    }
    public void UpdateHealth(int amount)
    {
        if (alive)
        {
            health += amount;
            if (amount < 0)
            {
                switch (tag)
                {
                    case "Enemy":
                        AudioManager.Instance.PlaySound(AudioManager.Sound.SpiderDeath, false);
                        break;
                    case "EnemyStructure":
                        if (currentTimer >= particleTimer)
                        {
                            GameObject blood = Instantiate(bloodSplatterPrefab, transform.position + transform.up * 7f, Quaternion.identity) as GameObject;
                            GameObject.Destroy(blood, 2.0f);
                            currentTimer = 0f;
                        }
                        break;
                    case "Soldier":
                        AudioManager.Instance.PlaySound(AudioManager.Sound.SoldierHurt, false);
                        break;
                    default:
                        break;
                }
            }
            if (health <= 0)
            {
                StartDying();
            }
            health = Mathf.Clamp(health, 0, maxHealth);
            NotifyHealthUpdate();
        }
    }
    void StartDying()
    {
        //death
        
        alive = false;
        switch (tag)
        {
            case "Enemy":
                NotifyGiveCredits(drone.resoureValue);
                GameObject bloodSplatter = Instantiate(bloodSplatterPrefab, transform.position + transform.up*.5f, transform.rotation) as GameObject;
                GameObject.Destroy(bloodSplatter, 2.0f);
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
                //GameObject blood = Instantiate(bloodSplatterPrefab, transform.position + transform.up*.5f, transform.rotation) as GameObject;
                //GameObject.Destroy(blood, 2.0f);
                NotifyGiveCredits(spawner.resoureValue);
                NotifyDead();
                spawner.enabled = false;
                Invoke("Kill", 1.0f);
                break;
            case "Soldier":
                //Debug.Log("startDying");
                
                soldierManager.SoldierDied(soldier);
                AudioManager.Instance.StopSound(AudioManager.Sound.HealingBeam);
                AudioManager.Instance.StopSound(AudioManager.Sound.LightningGun);
                animation.CrossFade("onGround");
                Invoke("Kill", 1.2f);
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
                Destroy(gameObject, 1.0f);
                break;
            case "EnemyStructure":
                boxCol.enabled = false;
                foreach (GameObject drone in spawner.drones.ToArray())
                {
                    drone.GetComponent<Health>().UpdateHealth(-500);
                }
                Destroy(gameObject);
                break;
            case "Soldier":
                Destroy(gameObject);
                break;
            default:
                break;
        }

    }
    public void NotifyGiveCredits(int amount)
    {
        foreach (Observer obs in observers)
        {
            obs.UpdateCredits(amount);
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
