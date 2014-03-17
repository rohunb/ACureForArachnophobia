using UnityEngine;
using System.Collections;

public class ProjectileDamager : MonoBehaviour {

    public int damage;
    public GameObject origin;

    public void OnTriggerEnter(Collider other)
    {
        if (origin)
        {
            switch (origin.tag)
            {
                case "Soldier":
                    if (other.tag == "Enemy" || other.tag == "EnemyStructure")
                    {
                        //Instantiate(explosion, transform.position, Quaternion.identity);
                        other.GetComponent<Health>().UpdateHealth(-damage);
                        Destroy(gameObject);
                    }
                    break;
                //case "EnemyShip":
                //    if (other.tag == "PlayerShip" || other.tag == "Victim")
                //    {
                //        //Instantiate(explosion, transform.position, Quaternion.identity);
                //        other.GetComponent<Health>().TakeDamage(damage);
                //        Destroy(gameObject);
                //    }
                //    break;
            }
        }
    }
}
