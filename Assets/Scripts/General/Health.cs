using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

    public int maxHealth;

    private int health;

    private bool alive;

    public bool Alive
    {
        get { return alive; }
    }

    DroneBehavior drone;
    BoxCollider col;
    void Awake()
    {
        if (gameObject.tag == "Enemy")
        {
            drone = gameObject.GetComponent<DroneBehavior>();
            col = gameObject.GetComponent<BoxCollider>();
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
        }
    }
    void Die()
    {
        //death
        alive = false;
        if (gameObject.tag == "Enemy")
        {
            if (Random.value > 0.5f)
                animation.CrossFade("death1");
            else
                animation.CrossFade("death2");
            drone.enabled = false;
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
            Invoke("Kill", 1.0f);
            
        }
    }
    void Kill()
    {
        col.enabled = false;
        Destroy(gameObject, 1.0f);
    }
}
