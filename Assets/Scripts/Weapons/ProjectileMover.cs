using UnityEngine;
using System.Collections;

public class ProjectileMover : MonoBehaviour {
    public float speed;
    public float range;

	void Start () {
        rigidbody.velocity = transform.forward * speed;
        float timeToDestroy = range / speed;
        Destroy(gameObject, timeToDestroy);
	}
	
}
