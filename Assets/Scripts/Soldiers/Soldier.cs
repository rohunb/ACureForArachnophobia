using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Soldier : Observer {

    public float moveSpeed=10f;
    public Vector3 destination;
    public float sightRange;
    
    public GameObject soldierSight;
    public Weapon currentWeapon;
    public Transform shootPoint;
    public GameObject defaultWpn;

    public bool selected;

    bool movingToDest;
    Vector3 moveDirection;
    CharacterController controller;
    Projector selectionBox;
    LineRenderer line;
    //Color lineColour = Color.blue;

    
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

    enum SoldierState { Moving, Guarding, AttackMove, Attacking}
    SoldierState state;
    SoldierState prevState;
    void Awake()
    {
        controller = GetComponent<CharacterController>();
        selectionBox = GetComponentInChildren<Projector>();
        line = GetComponent<LineRenderer>();
        sight = soldierSight.GetComponent<SoldierSight>();
    }
	// Use this for initialization
	void Start () {
        destination = transform.position;
        moveDirection = Vector3.zero;
        state = SoldierState.Guarding;
        prevState = state;
        selected = false;
        line.enabled = false;

        GameObject wpn = Instantiate(defaultWpn) as GameObject;
        EquipWeapon(wpn);
        sight.UpdateSight(currentWeapon.range);
        //testing weapons
        //currentWeapon = weapon;
        
        
	}
	
	// Update is called once per frame
    void Update()
    {
        //Debug.Log(state);
        switch (state)
        {
            case SoldierState.Moving:
                currentWeapon.StopFiring();
                if (Vector3.Distance(destination, transform.position) > .5f)
                {
                    moveDirection = (destination - transform.position).normalized;
                    transform.rotation = Quaternion.LookRotation(moveDirection);
                    //moveDirection = transform.TransformDirection(moveDirection);
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
                   
                if (!CheckCanAttack())
                {
                    currentWeapon.StopFiring();
                    animation.CrossFade("idle");
                    line.enabled = false;
                }
                else
                {
                    prevState = SoldierState.Guarding;
                }
                break;
            case SoldierState.AttackMove:
                prevState = SoldierState.AttackMove;
                DrawLine(destination, Color.red);
                if (!CheckCanAttack())
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
                if(CheckCanAttack())
                    Attack();
                break;
            default:
                currentWeapon.StopFiring();
                break;
        }
        
        selectionBox.enabled = selected;
    }
    bool CheckCanAttack()
    {
        //if (dronesInSight.Count > 0 || structsInSight.Count>0)
        if(enemiesInSight.Count>0)
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
        //DroneBehavior droneTarget=null;
        //SwarmSpawner structTarget=null;

        //float distToDrone=1000f;
        //float distToStruct=1000f;
        //if (dronesInSight.Count > 0)
        //{
        //    droneTarget = NearestDrone();
        //    distToDrone = Vector3.Distance(droneTarget.transform.position, transform.position);
        //}
        //if (structsInSight.Count > 0)
        //{
        //    structTarget = NearestStruct();
        //    distToStruct = Vector3.Distance(structTarget.transform.position, transform.position);
        //}
        ////line.SetPosition(0, transform.position);
        ////line.SetPosition(1, new Vector3(target.transform.position.x, transform.position.y,target.transform.position.z));

        //if(distToDrone<distToStruct)
        //{
        //    AimWeaponAt(droneTarget.transform);
        //}
        //else
        //{
        //    AimWeaponAt(structTarget.transform);

        //}

        Enemy target = NearestEnemy();
        if(target) AimWeaponAt(target.transform);
        animation.CrossFade("attack");
        currentWeapon.Fire(gameObject);

    }
    void AimWeaponAt(Transform target)
    {
        transform.LookAt(target);
    }
    public override void UpdateDronesInSight(List<DroneBehavior> drones)
    {
        //Debug.Log("Drone count: " + drones.Count);
        dronesInSight = drones;
        if(drones.Count>0)
            UpdateDroneKDTree();
    }
    public override void UpdateStructsInSight(List<SwarmSpawner> structs)
    {
        structsInSight = structs;
        if (structs.Count > 0)
            UpdateStructsKDTree();

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
    public void EquipWeapon(GameObject _weapon)
    {
        //Destroy(gameObject.GetComponentInChildren<Weapon>().gameObject);
        _weapon.transform.parent = shootPoint;
        _weapon.transform.position = Vector3.zero;
        _weapon.transform.rotation = Quaternion.identity;
        
        currentWeapon = _weapon.GetComponent<Weapon>();
        currentWeapon.shootPoint = shootPoint;
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
  
}
