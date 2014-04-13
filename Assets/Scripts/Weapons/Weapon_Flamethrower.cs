using UnityEngine;
using System.Collections;

public class Weapon_Flamethrower : Weapon {

    public float firePulseTimer = 0.3f;
    public float flameLifeTimer = 2.0f;
	// Update is called once per frame
	void Update () {
        //Fire(gameObject);
        currentTimer += Time.deltaTime;
	}
    public override void Fire(GameObject origin)
    {
        if(currentTimer>=reloadTimer)
        {
            AudioManager.Instance.PlaySound(AudioManager.Sound.Flamethrower, .4f,false);
            GameObject flamesClone = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation) as GameObject;
            //GameObject flamesClone = ObjectPool.instance.GetObjectForType("Flames", false);
            //flamesClone.transform.position = shootPoint.position;
            //flamesClone.transform.rotation = shootPoint.rotation;
            flamesClone.GetComponent<FlameDamager>().Init(origin, damage,firePulseTimer,flameLifeTimer);
            flamesClone.GetComponent<ProjectileMover>().Init(shootPoint.position, projectileSpeed, range);
            currentTimer = 0f;
        }
    }

}
