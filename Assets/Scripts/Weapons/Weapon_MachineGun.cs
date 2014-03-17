using UnityEngine;
using System.Collections;

public class Weapon_MachineGun : Weapon {
    MuzzleFlash muzzleFlash;
	// Use this for initialization
	void Awake () {
        muzzleFlash = GetComponentInChildren<MuzzleFlash>();
	}
	void Start()
    {
        muzzleFlash.gameObject.SetActive(false);
    }
	// Update is called once per frame
	void Update () {
        //Fire(gameObject);
        currentTimer += Time.deltaTime;
	}
    public override void Fire(GameObject origin)
    {
        if(currentTimer>=reloadTimer)
        {
            muzzleFlash.gameObject.SetActive(true);
            muzzleFlash.AnimateMuzzleFlash();
            GameObject bulletClone = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation) as GameObject;
            ProjectileDamager damager = bulletClone.GetComponent<ProjectileDamager>();
            damager.origin = origin;
            damager.damage = damage;

            ProjectileMover mover = bulletClone.GetComponent<ProjectileMover>();
            mover.speed = projectileSpeed;
            mover.range = range;

            currentTimer = 0f;
        }
    }
    public override void StopFiring()
    {
        muzzleFlash.gameObject.SetActive(false);
    }
}
