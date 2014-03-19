using UnityEngine;
using System.Collections;

public class Weapon_Shotgun : Weapon {
    MuzzleFlash muzzleFlash;
    int numShots=8;
    float shotSpread = 10f;
    void Awake()
    {
        muzzleFlash = GetComponentInChildren<MuzzleFlash>();
    }
    void Start()
    {
        muzzleFlash.gameObject.SetActive(false);
    }
    void Update()
    {
        //Fire(gameObject);
        currentTimer += Time.deltaTime;
    }
    public override void Fire(GameObject origin)
    {
        if (currentTimer >= reloadTimer)
        {
            muzzleFlash.gameObject.SetActive(true);
            muzzleFlash.AnimateMuzzleFlash();
            for (int i = 0; i < numShots; i++)
            {
                GameObject bulletClone = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation) as GameObject;
                bulletClone.transform.Rotate(transform.up, Random.Range(-shotSpread, shotSpread));

                ProjectileDamager damager = bulletClone.GetComponent<ProjectileDamager>();
                damager.origin = origin;
                damager.damage = damage;

                ProjectileMover mover = bulletClone.GetComponent<ProjectileMover>();
                mover.speed = projectileSpeed;
                mover.range = range;
            }
            

            currentTimer = 0f;
        }
    }
    public override void StopFiring()
    {
        muzzleFlash.gameObject.SetActive(false);
    }

}
