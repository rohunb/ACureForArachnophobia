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

    public bool selected;

    Weapon_Lasers weapon;
    
    bool movingToDest;
    Vector3 moveDirection;
    CharacterController controller;
    Projector selectionBox;
    LineRenderer line;
    Color lineColour = Color.blue;

    KDTree dronesInSightTree;
    SoldierSight sight;
    public List<DroneBehavior> dronesInSight;
    Vector3[] dronesInSightPosArr;

    enum SoldierState { Moving, Guarding, AttackMove, Attacking}
    SoldierState state;
    SoldierState prevState;
    void Awake()
    {
        controller = GetComponent<CharacterController>();
        selectionBox = GetComponentInChildren<Projector>();
        line = GetComponent<LineRenderer>();
        sight = soldierSight.GetComponent<SoldierSight>();
        sight.sightRange = sightRange;

    }
	// Use this for initialization
	void Start () {
        destination = transform.position;
        moveDirection = Vector3.zero;
        state = SoldierState.Guarding;
        prevState = state;
        selected = false;
        line.enabled = false;
        
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
                    DrawLine(destination, Color.blue);
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
                        DrawLine(destination, Color.red);
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
        if (dronesInSight.Count > 0)
        {
            state = SoldierState.Attacking;
            //Debug.Log("state: " + state.ToString());
            //Debug.Log("prev state: " + prevState.ToString());
            
            return true;
        }
        else
        {
            Debug.Log("state: " + state.ToString());
            Debug.Log("prev state: " + prevState.ToString());
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
        DroneBehavior target = NearestDrone();
        //line.SetPosition(0, transform.position);
        //line.SetPosition(1, new Vector3(target.transform.position.x, transform.position.y,target.transform.position.z));
        AimWeaponAt(target.transform);
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
            UpdateKDTree();
    }
    void UpdateKDTree()
    {
        dronesInSightPosArr = new Vector3[dronesInSight.Count];
        for (int i = 0; i < dronesInSightPosArr.Length; i++)
        {
            dronesInSightPosArr[i] = dronesInSight[i].transform.position;
        }
        dronesInSightTree = KDTree.MakeFromPoints(dronesInSightPosArr);
    }
    DroneBehavior NearestDrone()
    {
        int nearest = dronesInSightTree.FindNearest(transform.position);
        return dronesInSight[nearest];
    }

    void DrawLine(Vector3 destination, Color colour)
    {
        line.enabled = true;
        line.SetColors(colour, colour);
        line.SetPosition(1, transform.position);
        line.SetPosition(0, destination);
        //float dist = Vector3.Distance(destination, transform.position);
        //int length = Mathf.RoundToInt(dist / lengthFactor);
        //line.SetVertexCount(length);
        //for (int i = 0; i < length -1; i++)
        //{
        //    Vector3 newPos = transform.position;
        //    newPos += transform.forward * i * lengthFactor;
        //    line.SetPosition(i, newPos);
        //}
        //line.SetPosition(length-1, destination);
    }
}
