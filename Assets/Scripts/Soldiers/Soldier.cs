using UnityEngine;
using System.Collections;

public class Soldier : MonoBehaviour {

    public float moveSpeed=10f;
    public Vector3 destination;
    public float sightRange;

    public bool selected;

    bool movingToDest;
    
    Vector3 moveDirection;
    CharacterController controller;
    Projector selectionBox;
    LineRenderer line;
    Color lineColour = Color.blue;
    public float lineWidth = 0.2f;
    public int lengthFactor=4;

    enum SoldierState { Moving, Guarding, AttackMove, Attacking}
    SoldierState state;
    void Awake()
    {
        controller = GetComponent<CharacterController>();
        selectionBox = GetComponentInChildren<Projector>();
        line = GetComponent<LineRenderer>();
    }
	// Use this for initialization
	void Start () {
        destination = transform.position;
        moveDirection = Vector3.zero;
        state = SoldierState.Guarding;
        selected = false;
        line.enabled = false;
        line.SetWidth(lineWidth, lineWidth);
	}
	
	// Update is called once per frame
    void Update()
    {

        switch (state)
        {
            case SoldierState.Moving:
                break;
            case SoldierState.Guarding:
                animation.CrossFade("idle");
                line.enabled = false;
                break;
            case SoldierState.AttackMove:
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
                break;
            case SoldierState.Attacking:
                break;
            default:
                break;
        }
        selectionBox.enabled = selected;
    }

    public void SetDestination(Vector3 _dest)
    {
        destination = _dest;
        state = SoldierState.AttackMove;

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
