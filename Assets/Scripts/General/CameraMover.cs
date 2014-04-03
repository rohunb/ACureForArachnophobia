using UnityEngine;
using System.Collections;

public class CameraMover : MonoBehaviour {
    
    public float camerMoveSpeed=35f;

    public float zoomSpeed = 8f;
    public float minFov = 20.0f;
    public float maxFov = 85.0f;

    public int terrainLayer = 8;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
       
        
	}
    public void Pan(Vector2 panVector)
    {
        bool canPan=false;
        RaycastHit hit;
        
        Ray topLeft = Camera.main.ScreenPointToRay(new Vector3(0, 0, 0));
        Ray topRight = Camera.main.ScreenPointToRay(new Vector3(Screen.width, 0, 0));
        Ray botRight = Camera.main.ScreenPointToRay(new Vector3(Screen.width, Screen.height, 0));
        Ray botLeft = Camera.main.ScreenPointToRay(new Vector3(0, Screen.height, 0));

        Vector3 cameraOldPos = transform.position;
        transform.Translate(new Vector3(panVector.x * camerMoveSpeed * Time.deltaTime, panVector.y * camerMoveSpeed * Time.deltaTime, 0.0f));

        bool topRightOnTerrain = Physics.Raycast(topRight, out hit, Mathf.Infinity, 1 << terrainLayer);
        bool topLeftOnTerrain = Physics.Raycast(topLeft, out hit, Mathf.Infinity, 1 << terrainLayer);
        bool botRightOnTerrain = Physics.Raycast(botRight, out hit, Mathf.Infinity, 1 << terrainLayer);
        bool botLeftOnTerrain = Physics.Raycast(botLeft, out hit, Mathf.Infinity, 1 << terrainLayer);

        if(topRightOnTerrain && topLeftOnTerrain && botLeftOnTerrain && botRightOnTerrain)
        {
            canPan = true;
        }

        //if (panVector.x > 0f)
        //{
        //    if (topRightOnTerrain && botRightOnTerrain)
        //        canPan = true;
        //    else if (topRightOnTerrain && !botRightOnTerrain)
        //    {

        //    }
        //}
        //if(panVector.x<0f)
        //{

        //}
        //if (panVector.y < 0f)
        //{
            
        //}
        //if (panVector.y > 0f)
        //{
            
        //}
        if (!canPan)
        {

           // transform.position = cameraOldPos;
            transform.Translate(new Vector3(-panVector.x * camerMoveSpeed * Time.deltaTime, -panVector.y * camerMoveSpeed * Time.deltaTime, 0.0f));
        }
    }
    public void Zoom(float zoomAmount)
    {
        camera.fieldOfView -= zoomAmount * zoomSpeed;
        camera.fieldOfView = Mathf.Clamp(camera.fieldOfView, minFov, maxFov);
    }
}
