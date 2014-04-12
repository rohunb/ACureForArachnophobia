using UnityEngine;
using System.Collections;

public class Weapon_MachineGun : Weapon {
    MuzzleFlash muzzleFlash;
	// Use this for initialization
	void Awake () {
        muzzleFlash = GetComponentInChildren<MuzzleFlash>();
	}
    protected override void Start()
    {
        muzzleFlash.gameObject.SetActive(false);
        base.Start();
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
            AudioManager.Instance.PlaySound(AudioManager.Sound.MP5, false);
            muzzleFlash.gameObject.SetActive(true);
            muzzleFlash.AnimateMuzzleFlash();
            //GameObject bulletClone = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation) as GameObject;
            GameObject bulletClone = ObjectPool.instance.GetObjectForType("InstantBullet", false);
            bulletClone.transform.position = shootPoint.position;
            bulletClone.transform.rotation = shootPoint.rotation;
            //ProjectileDamager damager = bulletClone.GetComponent<ProjectileDamager>();
            //damager.origin = origin;
            //damager.damage = damage;
            bulletClone.GetComponent<ProjectileDamager>().Init(origin, damage);
            bulletClone.GetComponent<ProjectileMover>().Init(shootPoint.position, projectileSpeed, range);
            //ProjectileMover mover = bulletClone.GetComponent<ProjectileMover>();
            //mover.speed = projectileSpeed;
            //mover.range = range;
            //mover.originPos = shootPoint.position;


            currentTimer = 0f;
        }
    }
    public override void StopFiring()
    {
        muzzleFlash.gameObject.SetActive(false);
    }
}
