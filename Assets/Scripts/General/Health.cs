﻿using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{

    public int maxHealth;

    private int health;

    private bool alive;

    public bool Alive
    {
        get { return alive; }
    }

    DroneBehavior drone;
    BoxCollider boxCol;

    SwarmSpawner spawner;
    SphereCollider sphereCol;

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

        //if (gameObject.tag == "Enemy")
        //{
        //    drone = gameObject.GetComponent<DroneBehavior>();
        //    boxCol = gameObject.GetComponent<BoxCollider>();
        //}
        //if(gameObject.tag=="EnemyStructure")
        //{
        //    spawner = gameObject.GetComponent<SwarmSpawner>();
        //    sphereCol = GetComponent<SphereCollider>();
        //}
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
        }
    }
    void Die()
    {
        //death
        alive = false;
        switch (tag)
        {
            case "Enemy":
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
                spawner.enabled = false;
                Invoke("Kill", 0.0f);
                break;
            default:
                break;
        }
        //if (gameObject.tag == "Enemy")
        //{
        //    if (Random.value > 0.5f)
        //        animation.CrossFade("death1");
        //    else
        //        animation.CrossFade("death2");
        //    drone.enabled = false;
        //    rigidbody.velocity = Vector3.zero;
        //    rigidbody.angularVelocity = Vector3.zero;
        //    Invoke("Kill", 1.0f);
        //}
        //if(gameObject.tag=="EnemyStructure")
        //{

        //}
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
                Destroy(gameObject, 2.0f); 
                break;
            default:
                break;
        }

    }
}
