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
    }
}
