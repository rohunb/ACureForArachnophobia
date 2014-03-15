using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour 
{
	// time to destroy bullet
	int seconds = 3;
    public int damage = 20;
	// Keeps track of the Enemy that fire the bullet
	public string playerShooting;
	
	// Use this for initialization
	void Start()
	{
		Destroy(gameObject, seconds); // Destroy Bullet after seconds
	}
	
	void OnCollisionEnter(Collision col)
	{
        //if(col.gameObject.name == "Player")
        //    Destroy(gameObject);	// Destroy Bullet
		
        //// Look for Health Component on Collider
        //if(col.gameObject.GetComponent<Health>()) 
        //{
        //    // Add Enemy name to the Player Health value
        //    Health.enemy = playerShooting;
			
        //    // Decrement Health of Player after collision
        //    Health.updateHealth(-10);
        //}
        //if(col.gameObject.tag=="Enemy")
        //{
        //    col.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
        //    Destroy(gameObject);
        //}
	}
}
