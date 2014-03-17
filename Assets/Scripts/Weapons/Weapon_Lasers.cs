using UnityEngine;
using System.Collections;

public class Weapon_Lasers : Weapon {

    //protected override void Start()
    //{
    //    Debug.Log("canfire " + canFire);
    //    base.Start();
    //    Debug.Log("canfire " + canFire);
    //}
    void Update()
    {
        //Debug.Log("canfire " + canFire);
        currentTimer += Time.deltaTime;
        
    }

    public override void Fire(GameObject origin)
    {
        //Debug.Log("canfire " + canFire);
        if(currentTimer>=reloadTimer)
        {

            GameObject laserClone = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation) as GameObject;
            ProjectileDamager damager = laserClone.GetComponent<ProjectileDamager>();
            damager.origin = origin;
            damager.damage = damage;

            ProjectileMover mover = laserClone.GetComponent<ProjectileMover>();
            mover.speed = projectileSpeed;
            mover.range = range;

            currentTimer = 0f;
        }
    }
    //IEnumerator FireLasers()
    //{
    //    Debug.Log("s");
        
    //    yield return new WaitForSeconds(reloadTimer);
    //    canFire = true;
    //}
}
