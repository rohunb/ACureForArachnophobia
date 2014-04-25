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
        currentTimer += Time.deltaTime;
	}
    public override void Fire(GameObject origin)
    {
        if(currentTimer>=reloadTimer)
        {
            AudioManager.Instance.PlaySound(AudioManager.Sound.MP5,.4f, false);
            muzzleFlash.gameObject.SetActive(true);
            muzzleFlash.AnimateMuzzleFlash();
            GameObject bulletClone = ObjectPool.instance.GetObjectForType("InstantBullet", false);
            bulletClone.transform.position = shootPoint.position;
            bulletClone.transform.rotation = shootPoint.rotation;
            bulletClone.GetComponent<ProjectileDamager>().Init(origin, damage);
            bulletClone.GetComponent<ProjectileMover>().Init(shootPoint.position, projectileSpeed, range);
            currentTimer = 0f;
        }
    }
    public override void StopFiring()
    {
        muzzleFlash.gameObject.SetActive(false);
    }
}
