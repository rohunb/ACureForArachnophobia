using UnityEngine;
using System.Collections;

public class ProjectileDamager : MonoBehaviour {

    public int damage;
    public GameObject origin;
    public virtual void Init(GameObject _origin, int _damage)
    {
        damage = _damage;
        origin = _origin;
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (origin)
        {
            switch (origin.tag)
            {
                case "Soldier":
                    if (other.tag == "Enemy" || other.tag == "EnemyStructure")
                    {
                        other.GetComponent<Health>().UpdateHealth(-damage);
                        rigidbody.velocity = Vector3.zero;
                        rigidbody.angularVelocity = Vector3.zero;
                        ObjectPool.instance.PoolObject(gameObject);
                    }
                    break;
            }
        }
    }
}
