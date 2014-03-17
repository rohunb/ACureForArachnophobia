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
        if(Random.value>0.5f)
            animation.CrossFade("death1");
        else
            animation.CrossFade("death2");
        BoxCollider col = gameObject.GetComponent<BoxCollider>();
        col.enabled = false;
        Destroy(gameObject, 1.0f);
    }
}
