using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

    public string name;
    public int damage;
    public GameObject projectilePrefab;
    public float projectileSpeed;
    public float reloadTimer;
    public Transform shootPoint;
    public GameObject origin;
    public float range;
    protected bool canFire=true;
    public float currentTimer = 0f;

	// Use this for initialization
    //protected virtual void Start () {
    //    canFire = true;

    //}
	public virtual void Fire(GameObject origin)
    {
        
    }

}
