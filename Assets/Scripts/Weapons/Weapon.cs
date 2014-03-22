using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

    public string wpnName;
    public int damage;
    public GameObject projectilePrefab;
    public float projectileSpeed;
    public float reloadTimer;
    public Transform shootPoint;
    public GameObject origin;
    public float range;
    protected bool canFire=true;
    public float currentTimer;
    public int cost;

	// Use this for initialization
    protected virtual void Start()
    {
        currentTimer = reloadTimer;

    }
	public virtual void Fire(GameObject origin)
    {
        
    }
    public virtual void StopFiring() { }

}
