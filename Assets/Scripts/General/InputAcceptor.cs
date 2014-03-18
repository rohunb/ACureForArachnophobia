using UnityEngine;
using System.Collections;

public class InputAcceptor : MonoBehaviour {
    
    
    //drag select
    private Vector2 dragStartPos;
    private Vector2 dragEndPos;
    private bool dragging = false;

    InputResolver inputResolver;
    
    void Awake()
    {
        inputResolver = gameObject.GetComponent<InputResolver>();
    }
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetMouseButtonDown(1))
        {
            inputResolver.ResolveInput(InputResolver.InputResponse.AttackMove);
        }
        //shift click for force move
        if((Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift)) && Input.GetMouseButtonDown(1))
        {
            inputResolver.ResolveInput(InputResolver.InputResponse.Move);
        }
        if(Input.GetMouseButtonDown(0))
        {
            inputResolver.ResolveInput(InputResolver.InputResponse.Select);
            dragStartPos = Input.mousePosition;
            dragging = true;
        }
        if(Input.GetMouseButtonUp(0))
        {
            dragging = false;
            dragEndPos = Input.mousePosition;
        }
        if(Input.GetKey(KeyCode.A))
        {
            inputResolver.ResolveInput(InputResolver.InputResponse.ViewLeft);
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputResolver.ResolveInput(InputResolver.InputResponse.ViewRight);
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputResolver.ResolveInput(InputResolver.InputResponse.ViewDown);
        }
        if (Input.GetKey(KeyCode.W))
        {
            inputResolver.ResolveInput(InputResolver.InputResponse.ViewUp);
        }
        if(Input.GetAxis ("Mouse ScrollWheel")!=0)
        {
            inputResolver.ResolveInput(InputResolver.InputResponse.Zoom, Input.GetAxis("Mouse ScrollWheel"));
        }

	}
    void OnGUI()
    {
        if(dragging)
        {
            dragEndPos = Input.mousePosition;
            DragBox();
        }
    }
    void DragBox()
    {
        float minX = Mathf.Min(dragStartPos.x, dragEndPos.x);
        float maxX = Mathf.Max(dragStartPos.x, dragEndPos.x);

        float minY = Mathf.Min(Screen.height - dragStartPos.y, Screen.height - dragEndPos.y);
        float maxY = Mathf.Max(Screen.height - dragStartPos.y, Screen.height - dragEndPos.y);
        Rect selectionBox=new Rect(minX,minY,maxX-minX,maxY-minY);
        GUI.Box(selectionBox, "");
        inputResolver.ResolveInput(InputResolver.InputResponse.DragSelect, selectionBox);
        //GUI.Box(new Rect(dragStartPos.x,dragStartPos.y,dragEndPos.x-dragStartPos.x,dragEndPos.y-dragStartPos.y), "");
    }
    
}
