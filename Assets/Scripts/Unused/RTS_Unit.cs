using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]

public class RTS_Unit : RTS_Object {
    
    public float moveSpeed=10f;
    public float turnSpeed=10f;

    public float maxSpeed=20f;
    public float maxSteer = .05f;

    public Vector3 destination;
    
    protected bool movingToDest=false;

    
    InputResolver inputResolver;
    SoldierManager unitController;

    //public enum UnitStates {Moving, Attacking, AttackMove, StopAndAttack}

    //swarming vars

    //swaming works for selected units to make them move in formation
    List<RTS_Unit> selectedUnits;

    //line of sight
    public enum FieldOfView { Wide, Limited, Narrow };
    public FieldOfView fovType;
    public float fovAngleFactor=1f;
    public float viewRadiusFactor=1.5f;

    Vector3 steerForce = Vector3.zero;

    //weights to modify unit movement
    public float separationWeight = 1f;
	public float alignmentWeight = 1f;
	public float cohesionWeight = 1f;
    public float moveToWeight = 1f;
    
    public float neighbourRadius = 50f;
    public float desiredSeparation = 6f;

    // velocity for different rules
    public Vector3 separationVel;
    public Vector3 alignmentVel;
    public Vector3 cohesionVel;
    public Vector3 moveToVel;

	// Use this for initialization
    protected virtual void Start()
    {
        inputResolver = GameObject.FindGameObjectWithTag("GameController").GetComponent<InputResolver>();
        unitController = GameObject.FindGameObjectWithTag("GameController").GetComponent<SoldierManager>();
        selectedUnits=new List<RTS_Unit>();
        base.Start();
	}
	
	// Update is called once per frame
    protected virtual void Update()
    {
        //foreach (RTS_Object obj in unitController.selectableUnits)
        //{
        //    if(obj is RTS_Unit)
        //    {
        //        selectedUnits.Add((RTS_Unit)obj);
        //    }
        //}

        if(movingToDest && Vector3.Distance(destination, transform.position)<1f)
        {
            movingToDest=false;
            transform.position=destination;
            rigidbody.velocity = Vector3.zero;
        }
        else if (movingToDest)
        {
            //TurnTowards(destination);
            MoveTowardsDest();
        }

        base.Update();
	}
    protected virtual void FixedUpdate()
    {
        if(movingToDest)
            Swarm();
        base.FixedUpdate();
    }
    public virtual void MoveToDestination(Vector3 _dest)
    {
        destination = _dest;
        movingToDest = true;
       // rigidbody.velocity = (destination - transform.position).normalized * moveSpeed;
    }
    public virtual void TurnTowards(Vector3 dest)
    {
        transform.LookAt(dest);
    }
    protected virtual void MoveTowardsDest()
    {
        Debug.Log("s");
        
        //transform.Translate((destination - transform.position).normalized * moveSpeed * Time.deltaTime);
        //rigidbody.velocity = (destination - transform.position).normalized * moveSpeed * Time.deltaTime;
        rigidbody.velocity = transform.forward * moveSpeed * Time.deltaTime;
        
    }
    protected virtual void Swarm()
    {
        Vector3 newVelocity = Vector3.zero;
        CalculateVelocities();
        //newVelocity += separationVel * separationWeight;
        //newVelocity += alignmentVel * alignmentWeight;
        //newVelocity += cohesionVel * cohesionWeight;
        ////newVelocity += moveToVel * moveToWeight;
        ////newVelocity *= moveSpeed;
        //newVelocity = rigidbody.velocity + newVelocity;
        //newVelocity.y = 0f;
        
        //rigidbody.velocity = Limit(newVelocity,maxSpeed);
        //Steer(steerForce);
        //transform.rotation = Quaternion.LookRotation(steerForce);

    }
    protected virtual void CalculateVelocities()
    {
        Vector3 _separVel = Vector3.zero;
        Vector3 _alignVel = Vector3.zero;
        Vector3 _cohesVel = Vector3.zero;
        Vector3 _moveToVel = Vector3.zero;
        int separCount = 0;
        int alignCount = 0;
        int cohesCount = 0;
        int moveToCount = 0;

        Vector3 averagePos = Vector3.zero;
        Vector3 averageVel = Vector3.zero;
        int numNeighbours = 0;

        Vector3 currentDisplacement, currentVel, displacementLocal;
        float directionFactor=0f;
        

        bool inLoS = false;
        for (int i = 0; i < selectedUnits.Count; i++)
        {
            if(selectedUnits[i] && gameObject !=selectedUnits[i])
            {
                Transform otherUnit=selectedUnits[i].transform;

                inLoS = false;
                Vector3 distanceVector = otherUnit.position - transform.position;
                Vector3 otherUnitLocalPos = transform.InverseTransformDirection(otherUnit.position);

                switch (fovType)
                {
                    case FieldOfView.Wide:
                        inLoS = ((otherUnitLocalPos.y > 0) || (otherUnitLocalPos.y < 0)
                               && (Mathf.Abs(otherUnitLocalPos.x) > Mathf.Abs(otherUnitLocalPos.y) * fovAngleFactor));
                        break;
                    case FieldOfView.Limited:
                        inLoS = otherUnitLocalPos.y > 0.0f;
                        break;
                    case FieldOfView.Narrow:
                        inLoS = (((otherUnitLocalPos.y > 0f)
                            && (Mathf.Abs(otherUnitLocalPos.x) < Mathf.Abs(otherUnitLocalPos.y) * fovAngleFactor)));
                        break;
                    default:
                        break;
                }

                //keep track of neighbours
                if (inLoS)
                {
                    if (distanceVector.magnitude <= neighbourRadius)
                    {
                        averagePos += otherUnit.position;
                        averageVel += otherUnit.rigidbody.velocity;
                        numNeighbours++;
                    }
                }

                if (inLoS)
                {
                    //separation
                    if (distanceVector.magnitude <= desiredSeparation)
                    {
                        if (otherUnitLocalPos.x < 0) directionFactor = 1f;
                        if (otherUnitLocalPos.x > 0) directionFactor = -1f;
                        steerForce.x += directionFactor * maxSteer * desiredSeparation * distanceVector.magnitude;
                    }
                }
                if (numNeighbours > 0)
                {
                    //cohesion
                    averagePos /= numNeighbours;
                    currentVel = rigidbody.velocity;
                    currentVel.Normalize();
                    currentDisplacement = averagePos - transform.position;
                    currentDisplacement.Normalize();
                    displacementLocal = transform.InverseTransformDirection(currentDisplacement);
                    if (displacementLocal.x < 0) directionFactor = -1f;
                    if (displacementLocal.x > 0) directionFactor = 1f;
                    if (Mathf.Abs(Vector3.Dot(currentVel, currentDisplacement)) < 1f)
                        steerForce.x += directionFactor * maxSteer * Mathf.Acos(Vector3.Dot(currentVel, currentDisplacement)) / Mathf.PI;

                    //alignment
                    averageVel /= numNeighbours;
                    Vector3 avgHeading = averageVel;
                    avgHeading.Normalize();
                    currentVel = rigidbody.velocity;
                    currentVel.Normalize();
                    Vector3 localAvgHeading = transform.InverseTransformDirection(avgHeading);
                    if (localAvgHeading.x < 0.0f) directionFactor = -1f;
                    if (localAvgHeading.x > 0.0f) directionFactor = -1f;
                    if (Mathf.Abs(Vector3.Dot(currentVel, avgHeading)) < 1f)
                        steerForce.x += directionFactor * maxSteer * Mathf.Acos(Vector3.Dot(currentVel, avgHeading)) / Mathf.PI;


                }
                transform.Translate(steerForce * moveSpeed);




                //separation : desire to stay at a particular distance away from others
                //float dist = Vector3.Distance(transform.position, otherUnit.position);
                //if (dist > 0f)
                //{
                //    if (dist < desiredSeparation)
                //    {
                //        Vector3 desiredVel = transform.position - otherUnit.position;
                //        desiredVel.Normalize();
                //        desiredVel /= dist; //reduced vel based on distance
                //        _separVel += desiredVel;
                //        separCount++;
                //    }

                //    if (dist < neighbourRadius)
                //    {//alignment: desire to move in the same direction as others
                //        _alignVel += otherUnit.rigidbody.velocity;
                //        alignCount++;

                //        //cohesion: desire to stay close to others
                //        _cohesVel += otherUnit.position;
                //        cohesCount++;
                //    }

                //}
            }
        }
        //move to destination
        //if (movingToDest)
        //{
        //    _moveToVel += destination - transform.position;
        //    moveToCount++;
        //}

        separationVel = separCount > 0 ? _separVel / separCount : _separVel;
        alignmentVel = alignCount > 0 ? Limit(_alignVel / alignCount, maxSteer) : _alignVel;
        cohesionVel = cohesCount > 0 ? Steer(_cohesVel / cohesCount) : _cohesVel;
        //moveToVel = moveToCount > 0 ? _moveToVel / moveToCount : _moveToVel;

    }
    //vector to move towards destination
    protected virtual Vector3 Steer(Vector3 dest)
    {
        Vector3 steer = Vector3.zero;
        Vector3 destDirection = dest - transform.position;
        float destDistance = destDirection.magnitude;

        TurnTowards(dest);

        if(destDistance>0)
        {
            destDirection.Normalize();
            destDirection *= maxSpeed;
            steer = destDirection - rigidbody.velocity;
            steer = Limit(steer, maxSteer);
        }

        

        return steer;
    }
    Vector3 Limit(Vector3 vec, float max)
    {
        if (vec.magnitude > max)
            return vec.normalized * max;
        else
            return vec;
    }
}
