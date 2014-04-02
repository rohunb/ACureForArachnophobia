using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Soldier : Observer
{

    public float moveSpeed = 10f;
    public Vector3 destination;
    public float sightRange;

    public GameObject soldierSight;
    public Transform shootPoint;

    public Weapon currentWeapon;
    public Weapon MP5;
    public Weapon shotgun;
    public Weapon lightningGun;
    public Weapon flamethrower;
    public Weapon healingBeam;

    Soldier lowestHPSoldier;
    Health lowestSoldierHealth;

    public bool selected;

    bool movingToDest;
    Vector3 moveDirection;
    CharacterController controller;
    Projector selectionBox;
    LineRenderer line;
    //Color lineColour = Color.blue;

    SoldierManager soldierManager;
    SoldierSight sight;

    public List<DroneBehavior> dronesInSight;
    Vector3[] dronesInSightPosArr;
    KDTree dronesInSightTree;

    public List<SwarmSpawner> structsInSight;
    Vector3[] structsInSightPosArr;
    KDTree structsInSightTree;

    public List<Enemy> enemiesInSight;
    Vector3[] enemiesInSightPosArr;
    KDTree enemiesInSightTree;

    enum SoldierState { Moving, Guarding, AttackMove, Attacking, Healing, HealMove }
    [SerializeField]
    SoldierState state;
    SoldierState prevState;

    Vector3 prevPos;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        selectionBox = GetComponentInChildren<Projector>();
        line = GetComponent<LineRenderer>();
        sight = soldierSight.GetComponent<SoldierSight>();
        soldierManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<SoldierManager>();
    }
    // Use this for initialization
    void Start()
    {
        destination = transform.position;
        moveDirection = Vector3.zero;
        state = SoldierState.Guarding;
        prevState = state;
        selected = false;
        line.enabled = false;
        EquipWeapon("MP5");
        sight.UpdateSight(currentWeapon.range);
        prevPos = transform.position;
    }
    void Update()
    {
        Weapon_HealingBeam healingWpn = null;
        if (currentWeapon is Weapon_HealingBeam)
        {
            healingWpn = (Weapon_HealingBeam)currentWeapon;
        }
        switch (state)
        {
            case SoldierState.Moving:
                if (healingWpn)
                {
                    healingWpn.StopHealing();
                    healingWpn.StopFiring();
                }
                if (Vector3.Distance(destination, transform.position) > .5f)
                {
                    moveDirection = (destination - transform.position).normalized;
                    transform.rotation = Quaternion.LookRotation(moveDirection);
                    moveDirection = transform.forward * moveSpeed;
                    controller.SimpleMove(moveDirection);
                    DrawLine(destination, Color.cyan);
                    animation.CrossFade("run");
                }
                else
                {
                    state = SoldierState.Guarding;
                }
                break;
            case SoldierState.Guarding:
                
                if (healingWpn)
                {
                    if (CheckCanHeal())
                    {
                        prevState = SoldierState.Guarding;
                        break;
                    }
                    else if(CheckCanAttack())
                    {
                        prevState = SoldierState.Guarding;
                        break;
                    }
                    else
                    {
                        healingWpn.StopHealing();
                        healingWpn.StopFiring();
                        animation.CrossFade("idle");
                        line.enabled = false;
                    }
                }
                else if (CheckCanAttack())
                {
                    prevState = SoldierState.Guarding;
                    break;
                }
                else
                {
                    currentWeapon.StopFiring();
                    animation.CrossFade("idle");
                    line.enabled = false;
                }


                break;
            case SoldierState.AttackMove:
                prevState = SoldierState.AttackMove;
                DrawLine(destination, Color.red);

                if(healingWpn)
                {
                    if(CheckCanHeal())
                    {
                        prevState = SoldierState.AttackMove;
                    }
                    else if(CheckCanAttack())
                    {
                        prevState = SoldierState.AttackMove;
                    }
                    else
                    {
                        healingWpn.StopHealing();
                        healingWpn.StopFiring();
                        if (Vector3.Distance(destination, transform.position) > .5f)
                        {
                            moveDirection = (destination - transform.position).normalized;
                            transform.rotation = Quaternion.LookRotation(moveDirection);
                            //moveDirection = transform.TransformDirection(moveDirection);
                            moveDirection = transform.forward * moveSpeed;
                            controller.SimpleMove(moveDirection);

                            animation.CrossFade("run");
                        }
                        else
                        {
                            state = SoldierState.Guarding;
                        }
                    }
                }
                else if (!CheckCanAttack())
                {
                    currentWeapon.StopFiring();
                    if (Vector3.Distance(destination, transform.position) > .5f)
                    {
                        moveDirection = (destination - transform.position).normalized;
                        transform.rotation = Quaternion.LookRotation(moveDirection);
                        //moveDirection = transform.TransformDirection(moveDirection);
                        moveDirection = transform.forward * moveSpeed;
                        controller.SimpleMove(moveDirection);

                        animation.CrossFade("run");
                    }
                    else
                    {
                        state = SoldierState.Guarding;
                    }
                }
                else
                {
                    prevState = SoldierState.AttackMove;
                }
                break;
            case SoldierState.Attacking:
                if(healingWpn && CheckCanHeal())
                {
                    //if(CheckCanHeal())
                    //{
                    //   //prevState=
                    //}
                    
                }
                else if (CheckCanAttack())
                {
                    if(healingWpn)
                    {
                        AttackHealingWpn();
                    }
                    else
                    {
                        Attack();
                    }

                }
                    
                break;
            case SoldierState.Healing:
                if (CheckCanHeal())
                {
                    Heal();
                }
                break;
            case SoldierState.HealMove:
                break;
            default:
                currentWeapon.StopFiring();
                break;
        }
        selectionBox.enabled = selected;
        //if(prevPos!=transform.position)
        //{
        //    soldierManager.RunInjuredSoldierCheck();
        //}
    }
    bool CheckCanAttack()
    {
        //if (dronesInSight.Count > 0 || structsInSight.Count>0)
        if (enemiesInSight.Count > 0)
        {
            state = SoldierState.Attacking;
            //Debug.Log("state: " + state.ToString());
            //Debug.Log("prev state: " + prevState.ToString());

            return true;
        }
        else
        {
            //Debug.Log("state: " + state.ToString());
            //Debug.Log("prev state: " + prevState.ToString());
            state = prevState;
            return false;
        }
    }
    bool CheckCanHeal()
    {
        lowestHPSoldier = soldierManager.FindNearestInjuredSoldierWithinRange(this, currentWeapon.range);
        if (lowestHPSoldier)
        {
            lowestSoldierHealth = lowestHPSoldier.GetComponent<Health>();
            if (lowestSoldierHealth.GetHealth < lowestSoldierHealth.maxHealth)
            {
                state = SoldierState.Healing;
                return true;
            }
            else
            {
                state = prevState;
                return false;
            }
        }
        else
        {
            state = prevState;
            return false;
        }

    }
    public void SetMove(Vector3 _dest)
    {

        destination = _dest;
        state = SoldierState.Moving;

    }
    public void SetAttackMove(Vector3 _dest)
    {
        destination = _dest;
        state = SoldierState.AttackMove;

    }
    void Attack()
    {
        line.enabled = true;
        
        Enemy target = NearestEnemy();
        if (target) AimWeaponAt(target.transform);
        animation.CrossFade("attack");
        currentWeapon.Fire(gameObject);
    }
    void AttackHealingWpn()
    {
        line.enabled = true;

        Enemy target = NearestEnemy();
        if (target) 
            AimWeaponAt(target.transform);
        animation.CrossFade("attack");
        ((Weapon_HealingBeam)currentWeapon).Fire(target.transform);
    }
    void Heal()
    {
        line.enabled = true;
        if (lowestHPSoldier)
            AimWeaponAt(lowestHPSoldier.transform);
        animation.CrossFade("attack");
        ((Weapon_HealingBeam)currentWeapon).Heal(lowestHPSoldier.transform);
    }
    void AimWeaponAt(Transform target)
    {
        transform.LookAt(target);
    }

    public override void UpdateLowestHPSoldier()
    {
        lowestHPSoldier = soldierManager.FindNearestInjuredSoldierWithinRange(this, currentWeapon.range);
    }
    public override void UpdateEnemiesInSight(List<Enemy> enemies)
    {
        enemiesInSight = enemies;
        if (enemies.Count > 0)
            UpdateEnemyKDTree();
    }
    void UpdateEnemyKDTree()
    {
        enemiesInSightPosArr = new Vector3[enemiesInSight.Count];
        for (int i = 0; i < enemiesInSightPosArr.Length; i++)
        {
            //if (enemiesInSight[i])
            enemiesInSightPosArr[i] = enemiesInSight[i].transform.position;
        }
        enemiesInSightTree = KDTree.MakeFromPoints(enemiesInSightPosArr);
    }
    void UpdateDroneKDTree()
    {
        dronesInSightPosArr = new Vector3[dronesInSight.Count];
        for (int i = 0; i < dronesInSightPosArr.Length; i++)
        {
            dronesInSightPosArr[i] = dronesInSight[i].transform.position;
        }
        dronesInSightTree = KDTree.MakeFromPoints(dronesInSightPosArr);
    }
    void UpdateStructsKDTree()
    {
        structsInSightPosArr = new Vector3[structsInSight.Count];
        for (int i = 0; i < structsInSightPosArr.Length; i++)
        {
            structsInSightPosArr[i] = structsInSight[i].transform.position;
        }
        structsInSightTree = KDTree.MakeFromPoints(structsInSightPosArr);
    }
    Enemy NearestEnemy()
    {
        int nearest = enemiesInSightTree.FindNearest(transform.position);
        return enemiesInSight[nearest];
    }
    //public void EquipWeapon(GameObject _weapon)
    //{
    //    //Destroy(gameObject.GetComponentInChildren<Weapon>().gameObject);
    //    _weapon.transform.parent = shootPoint;
    //    _weapon.transform.position = Vector3.zero;
    //    _weapon.transform.rotation = Quaternion.identity;

    //    currentWeapon = _weapon.GetComponent<Weapon>();
    //    currentWeapon.shootPoint = shootPoint;
    //    sight.UpdateSight(currentWeapon.range);
    //}
    public void EquipWeapon(string wpnName)
    {
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        //for (int i = bullets.Length-1; i >=0; i--)
        for (int i = 0; i < bullets.Length; i++)
        {
            if (bullets[i] && bullets[i].GetComponent<ProjectileDamager>().origin == gameObject)
            {
                bullets[i].GetComponent<ProjectileMover>().ReturnToPool();
            }
        }
        switch (wpnName)
        {
            case "MP5":
                MP5.gameObject.SetActive(true);
                currentWeapon = MP5;
                shotgun.gameObject.SetActive(false);
                lightningGun.gameObject.SetActive(false);
                flamethrower.gameObject.SetActive(false);
                healingBeam.gameObject.SetActive(false);
                break;
            case "Shotgun":
                shotgun.gameObject.SetActive(true);
                currentWeapon = shotgun;
                MP5.gameObject.SetActive(false);
                lightningGun.gameObject.SetActive(false);
                flamethrower.gameObject.SetActive(false);
                healingBeam.gameObject.SetActive(false);
                break;
            case "LightningGun":
                lightningGun.gameObject.SetActive(true);
                currentWeapon = lightningGun;
                MP5.gameObject.SetActive(false);
                shotgun.gameObject.SetActive(false);
                flamethrower.gameObject.SetActive(false);
                healingBeam.gameObject.SetActive(false);
                break;
            case "Flamethrower":
                flamethrower.gameObject.SetActive(true);
                currentWeapon = flamethrower;
                MP5.gameObject.SetActive(false);
                shotgun.gameObject.SetActive(false);
                lightningGun.gameObject.SetActive(false);
                healingBeam.gameObject.SetActive(false);
                break;
            case "HealingBeam":
                healingBeam.gameObject.SetActive(true);
                currentWeapon = healingBeam;
                MP5.gameObject.SetActive(false);
                shotgun.gameObject.SetActive(false);
                lightningGun.gameObject.SetActive(false);
                flamethrower.gameObject.SetActive(false);
                break;
            default:
                MP5.gameObject.SetActive(true);
                currentWeapon = MP5;
                shotgun.gameObject.SetActive(false);
                lightningGun.gameObject.SetActive(false);
                flamethrower.gameObject.SetActive(false);
                healingBeam.gameObject.SetActive(false);
                break;
        }
        sight.UpdateSight(currentWeapon.range);
    }
    void DrawLine(Vector3 destination, Color colour)
    {
        line.enabled = true;
        line.SetColors(colour, colour);
        line.SetPosition(1, transform.position);
        line.SetPosition(0, destination);
    }
    DroneBehavior NearestDrone()
    {
        if (dronesInSight.Count > 0)
        {
            int nearest = dronesInSightTree.FindNearest(transform.position);
            return dronesInSight[nearest];
        }
        else
            return null;
    }
    SwarmSpawner NearestStruct()
    {
        if (structsInSight.Count > 0)
        {
            int nearest = structsInSightTree.FindNearest(transform.position);
            return structsInSight[nearest];
        }
        else
            return null;
    }
    //public override void UpdateDronesInSight(List<DroneBehavior> drones)
    //{
    //    //Debug.Log("Drone count: " + drones.Count);
    //    dronesInSight = drones;
    //    if (drones.Count > 0)
    //        UpdateDroneKDTree();
    //}
    //public override void UpdateStructsInSight(List<SwarmSpawner> structs)
    //{
    //    structsInSight = structs;
    //    if (structs.Count > 0)
    //        UpdateStructsKDTree();

    //}

}
