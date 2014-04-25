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
    protected override void Start()
    {
        muzzleFlash.gameObject.SetActive(false);
        base.Start();
    }
    void Update()
    {
        currentTimer += Time.deltaTime;
    }
    public override void Fire(GameObject origin)
    {
        if (currentTimer >= reloadTimer)
        {
            AudioManager.Instance.PlaySound(AudioManager.Sound.Shotgun,.35f, false);
            muzzleFlash.gameObject.SetActive(true);
            muzzleFlash.AnimateMuzzleFlash();
            for (int i = 0; i < numShots; i++)
            {
                GameObject bulletClone = ObjectPool.instance.GetObjectForType("InstantBullet", false);
                bulletClone.transform.position = shootPoint.position;
                bulletClone.transform.rotation = shootPoint.rotation;

                bulletClone.transform.Rotate(transform.up, Random.Range(-shotSpread, shotSpread));
                bulletClone.GetComponent<ProjectileDamager>().Init(origin, damage);
                bulletClone.GetComponent<ProjectileMover>().Init(shootPoint.position, projectileSpeed, range);
            }
            currentTimer = 0f;
        }
    }
    public override void StopFiring()
    {
        muzzleFlash.gameObject.SetActive(false);
    }

}
