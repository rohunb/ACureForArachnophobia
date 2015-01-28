using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DroneBehavior : Enemy
{

    public float closeToTargetRange = 7f;
    public float attackRange = 2f;

    // the overall speed of the simulation
    public float speed = 10f;
    // max speed any particular drone can move at
    public float maxSpeed = 20f;
    public float turnSpeed = 200f;
    // maximum steering power
    public float maxSteer = .05f;

    // weights: used to modify the drone's movement
    public float separationWeight = 1f;
    public float alignmentWeight = 1f;
    public float cohesionWeight = 1f;
    public float boundsWeight = 0f;
    public float moveToWeight = 1f;

    public float neighborRadius = 50f;
    public float desiredSeparation = 6f;

    // velocity influences
    public Vector3 _separation;
    public Vector3 _alignment;
    public Vector3 _cohesion;
    public Vector3 _bounds;
    public Vector3 _moveToDest;

    // other members of my swarm
    public List<GameObject> drones;
    public SwarmSpawner swarm;
    private Transform trans;
    private Rigidbody rbody;

    public Transform destination;

    public enum AI_State { Swarming, GoingToAttack, CloseToTarget, Attacking }
    public AI_State currentState = AI_State.Swarming;

    public bool soldierInSight = false;
    float dist;

    public Transform attackPoint;
    public float meleeCheckDist = 2.0f;
    public int playerLayer = 9;
    public float attackTimer = 19; //animation frames for attack
    public int damage = 10;
    float currentTimer = 0f;

    public Renderer _renderer;

    void FixedUpdate()
    {
        if (_renderer.isVisible || soldierInSight)
        {
            if (currentState != AI_State.Attacking)
                Flock();
            if (rbody.velocity.magnitude > 0f && rbody.velocity.normalized != Vector3.zero)
                trans.rotation = Quaternion.Slerp(trans.rotation, Quaternion.LookRotation(rbody.velocity.normalized), Time.deltaTime * turnSpeed);
        }
        else
        {
            rbody.velocity = Vector3.zero;
            rbody.angularVelocity = Vector3.zero;
        }

    }

    private void Awake()
    {
        trans = transform;
        rbody= rigidbody;
    }
    protected virtual void Start()
    {
        moveToWeight = 0f;
        currentTimer = attackTimer * Time.deltaTime;
    }

    void Update()
    {

        if (destination)
            dist = Vector3.Distance(destination.position, trans.position);



        switch (currentState)
        {
            case AI_State.Swarming:
                if (soldierInSight)
                {
                    currentState = AI_State.GoingToAttack;
                }
                separationWeight = 1f;
                alignmentWeight = 1f;
                cohesionWeight = 1f;
                boundsWeight = 1f;
                moveToWeight = 0f;


                break;
            case AI_State.GoingToAttack:
                if (dist <= attackRange)
                {
                    currentState = AI_State.Attacking;
                }
                else if (dist <= closeToTargetRange)
                {
                    currentState = AI_State.CloseToTarget;
                }
                separationWeight = 1f;
                alignmentWeight = 1f;
                cohesionWeight = 1f;
                boundsWeight = 0f;
                moveToWeight = 1f;
                break;
            case AI_State.CloseToTarget:
                if (dist > closeToTargetRange)
                {
                    if (soldierInSight)
                        currentState = AI_State.GoingToAttack;
                    else
                        currentState = AI_State.Swarming;
                }
                if (dist <= attackRange)
                {
                    currentState = AI_State.Attacking;
                }
                separationWeight = .2f;
                alignmentWeight = .2f;
                cohesionWeight = .2f;
                boundsWeight = 0f;
                moveToWeight = 1f;
                break;
            case AI_State.Attacking:
                if (!destination)
                {
                    currentState = AI_State.Swarming;
                    break;
                }
                rbody.velocity = Vector3.zero;
                rbody.angularVelocity = Vector3.zero;

                if (soldierInSight)
                {
                    if (dist > closeToTargetRange)
                    {
                        currentState = AI_State.GoingToAttack;
                    }
                    else if (dist > attackRange)
                    {
                        currentState = AI_State.CloseToTarget;
                    }
                }
                else
                {
                    currentState = AI_State.Swarming;
                    break;
                }
                Vector3 targetPostition = new Vector3(destination.position.x, trans.position.y, destination.position.z);
                trans.LookAt(targetPostition);
                Attack();
                separationWeight = 0f;
                alignmentWeight = 0f;
                cohesionWeight = 0f;
                boundsWeight = 0f;
                moveToWeight = 0f;

                break;
            default:
                break;
        }

        currentTimer += Time.deltaTime;
    }
    void Attack()
    {
        Ray ray = new Ray(attackPoint.position, attackPoint.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, meleeCheckDist, 1 << playerLayer))
        {
            GameObject hitObj = hit.collider.gameObject;
            if (hit.collider.gameObject.tag == "Soldier")
            {
                if (currentTimer >= attackTimer * Time.deltaTime)
                {
                    AudioManager.Instance.PlaySound(AudioManager.Sound.SpiderAttack, false);
                    hitObj.GetComponent<Health>().UpdateHealth(-damage);
                    currentTimer = 0f;
                }
            }
        }
        if (Random.value > 0.5f)
            animation.CrossFade("attack1");
        else
            animation.CrossFade("attack2");

    }

    public virtual void Flock()
    {

        Vector3 newVelocity = Vector3.zero;

        CalculateVelocities();

        newVelocity += _separation * separationWeight;
        newVelocity += _alignment * alignmentWeight;
        newVelocity += _cohesion * cohesionWeight;
        newVelocity += _bounds * boundsWeight;
        newVelocity += _moveToDest * moveToWeight;

        newVelocity = newVelocity * speed;
        newVelocity = rbody.velocity + newVelocity;
        newVelocity.y = 0f;

        rbody.velocity = Limit(newVelocity, maxSpeed);

    }

    protected virtual void CalculateVelocities()
    {
        
        Vector3 separationSum = Vector3.zero;
        Vector3 alignmentSum = Vector3.zero;
        Vector3 cohesionSum = Vector3.zero;
        Vector3 boundsSum = Vector3.zero;
        Vector3 moveToSum = Vector3.zero;

        int separationCount = 0;
        int alignmentCount = 0;
        int cohesionCount = 0;
        int boundsCount = 0;
        int moveToCount = 0;

        for (int i = 0; i < this.drones.Count; i++)
        {
            if (drones[i] == null) continue;

            Vector3 dronePos = drones[i].transform.position;
            Vector3 droneVel = drones[i].rigidbody.velocity;
            Vector3 swarmPos = swarm.transform.position;
            float distance = Vector3.Distance(trans.position, dronePos);


            // separation
            if (distance > 0 && distance < desiredSeparation)
            {
                // calculate vector headed away from myself
                Vector3 direction = trans.position - dronePos;
                direction.Normalize();
                direction = direction / distance; // weight by distance
                separationSum += direction;
                separationCount++;
            }

            // alignment & cohesion
            if (distance > 0 && distance < neighborRadius)
            {
                alignmentSum += droneVel;
                alignmentCount++;

                cohesionSum += dronePos;
                cohesionCount++;
            }

            // bounds
            Bounds bounds = new Bounds(swarmPos, new Vector3(swarm.swarmBounds.x, 10000f, swarm.swarmBounds.y));
            if (distance > 0 && !bounds.Contains(dronePos))
            {
                Vector3 diff = trans.position - swarmPos;
                if (diff.magnitude > 0)
                {
                    boundsSum += swarmPos;
                    boundsCount++;
                }
            }

            //move to
            if (destination && Vector3.Distance(destination.position, swarmPos) > 4f)
            {
                moveToSum += destination.position;
                moveToCount++;
            }
        }

        // end
        _separation = separationCount > 0 ? separationSum / separationCount : separationSum;
        _alignment = alignmentCount > 0 ? Limit(alignmentSum / alignmentCount, maxSteer) : alignmentSum;
        _cohesion = cohesionCount > 0 ? Steer(cohesionSum / cohesionCount, false) : cohesionSum;
        _bounds = boundsCount > 0 ? Steer(boundsSum / boundsCount, false) : boundsSum;
        _moveToDest = moveToCount > 0 ? Steer(moveToSum / moveToCount, false) : moveToSum;
    }

    
    protected virtual Vector3 Steer(Vector3 target, bool slowDown)
    {
        // the steering vector
        Vector3 steer = Vector3.zero;
        Vector3 targetDirection = target - trans.position;
        float targetDistance = targetDirection.magnitude;

        trans.LookAt(target);

        if (targetDistance > 0)
        {
            // move towards the target
            targetDirection.Normalize();

            // we have two options for speed
            if (slowDown && targetDistance < 100f * speed)
            {
                targetDirection *= (maxSpeed * targetDistance / (100f * speed));
                targetDirection *= speed;
            }
            else
            {
                targetDirection *= maxSpeed;
            }

            // set steering vector
            steer = targetDirection - rbody.velocity;
            steer = Limit(steer, maxSteer);
        }

        return steer;
    }

    protected virtual Vector3 Limit(Vector3 v, float max)
    {
        if (v.magnitude > max)
        {
            return v.normalized * max;
        }
        else
        {
            return v;
        }
    }
}

