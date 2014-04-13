using UnityEngine;
using System.Collections;

public class CameraMover : MonoBehaviour {
    
    public float camerMoveSpeed=35f;

    public float zoomSpeed = 8f;
    public float minFov = 20.0f;
    public float maxFov = 85.0f;

    public int terrainLayer = 8;

    public Vector4 bounds;
    Rect boundaries;

	// Use this for initialization
	void Start () {
        boundaries = new Rect();
        boundaries.xMin = bounds.x;
        boundaries.xMax = bounds.y;
        boundaries.yMin = bounds.z;
        boundaries.yMax = bounds.w;
	}
	
	// Update is called once per frame
	void Update () {
        

        
	}
    public void Pan(Vector2 panVector)
    {
        //Ray topLeft = Camera.main.ScreenPointToRay(new Vector3(0, 0, 0));
        //Ray topRight = Camera.main.ScreenPointToRay(new Vector3(Screen.width, 0, 0));
        //Ray botLeft = Camera.main.ScreenPointToRay(new Vector3(0, Screen.height, 0));

        //float left, right, top, bottom;
        //RaycastHit hit;

        //Physics.Raycast(topLeft, out hit, Mathf.Infinity, 1 << terrainLayer);
        //left = hit.point.x;
        //top = hit.point.z;

        //Physics.Raycast(topRight, out hit, Mathf.Infinity, 1 << terrainLayer);
        //right = hit.point.x;

        //Physics.Raycast(botLeft, out hit, Mathf.Infinity, 1 << terrainLayer);
        //bottom = hit.point.z;

        //Debug.Log(new Vector4(left, top, right, bottom));

        //bool canPan=false;
        //RaycastHit hit;
        
        //Ray topLeft = Camera.main.ScreenPointToRay(new Vector3(0, 0, 0));
        //Ray topRight = Camera.main.ScreenPointToRay(new Vector3(Screen.width, 0, 0));
        //Ray botRight = Camera.main.ScreenPointToRay(new Vector3(Screen.width, Screen.height, 0));
        //Ray botLeft = Camera.main.ScreenPointToRay(new Vector3(0, Screen.height, 0));

        //Vector3 cameraOldPos = transform.position;
        //transform.Translate(new Vector3(panVector.x * camerMoveSpeed * Time.deltaTime, panVector.y * camerMoveSpeed * Time.deltaTime, 0.0f));

        //bool topRightOnTerrain = Physics.Raycast(topRight, out hit, Mathf.Infinity, 1 << terrainLayer);
        //bool topLeftOnTerrain = Physics.Raycast(topLeft, out hit, Mathf.Infinity, 1 << terrainLayer);
        //bool botRightOnTerrain = Physics.Raycast(botRight, out hit, Mathf.Infinity, 1 << terrainLayer);
        //bool botLeftOnTerrain = Physics.Raycast(botLeft, out hit, Mathf.Infinity, 1 << terrainLayer);

        //if(topRightOnTerrain && topLeftOnTerrain && botLeftOnTerrain && botRightOnTerrain)
        //{
        //    canPan = true;
        //}

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
        
           // transform.position = cameraOldPos;
        transform.Translate(new Vector3(panVector.x * camerMoveSpeed * Time.deltaTime, panVector.y * camerMoveSpeed * Time.deltaTime, 0.0f));
        //Debug.Log(transform.position);
        if (transform.position.x < boundaries.xMin)
        {
            transform.position = new Vector3(boundaries.xMin, transform.position.y, transform.position.z);
        }
        else if (transform.position.x > boundaries.xMax)
        {
            transform.position = new Vector3(boundaries.xMax, transform.position.y, transform.position.z);
        }

        if (transform.position.z < boundaries.yMin)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, boundaries.yMin);
        }
        else if (transform.position.z > boundaries.yMax)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, boundaries.yMax);
        }
        CheckIfBeyondEdge();

    }
    void CheckIfBeyondEdge()
    {
        //checking if camera could possible be close to the boundary
        if (Mathf.Abs(transform.position.x) > 56.9f ||
            Mathf.Abs(transform.position.y) > 94.2f)
        {
            Ray topLeft = Camera.main.ViewportPointToRay(new Vector3(0, 1, 0));
            Ray topRight = Camera.main.ScreenPointToRay(new Vector3(Screen.width, Screen.height - 1, 0));
            Ray botLeft = Camera.main.ViewportPointToRay(new Vector3(0, 0, 0));

            float left, right, top, bottom;
            RaycastHit hit;

            Physics.Raycast(topLeft, out hit, Mathf.Infinity, 1 << terrainLayer);
            left = hit.point.x;
            top = hit.point.z;

            Physics.Raycast(topRight, out hit, Mathf.Infinity, 1 << terrainLayer);
            right = hit.point.x;

            Physics.Raycast(botLeft, out hit, Mathf.Infinity, 1 << terrainLayer);
            bottom = hit.point.z;

            //Debug.Log("left right top, botton: " + new Vector4(left, right, top, bottom));
            //Debug.Log("Camera pos: " + transform.position);

            if (left < boundaries.xMin)
            {
                transform.Translate(new Vector3(boundaries.xMin - left, 0f, 0f));

                //Debug.Log("Left over");
            }
            else if (right > boundaries.xMax)
            {
                transform.Translate(new Vector3(boundaries.xMax - right, 0, 0), Space.World);
                //Debug.Log(new Vector4(left, right, top, bottom));

                //Debug.Log("right over");
            }

            if (bottom < boundaries.yMin)
            {
                transform.Translate(new Vector3(0, 0, boundaries.yMin - bottom), Space.World);
                //Debug.Log("bottom " + bottom);
                //Debug.Log("boundaries ymin: " + boundaries.yMin);
                //Debug.Log("left right top, botton: "+new Vector4(left, right, top, bottom));
                //Debug.Log("Top over");
            }
            else if (top > boundaries.yMax)
            {
                transform.Translate(new Vector3(0, 0, boundaries.yMax - top), Space.World);
                //Debug.Log("top " + top);
                //Debug.Log("boundaries ymax: " + boundaries.yMax);
                //Debug.Log("left right top, botton: " + new Vector4(left, right, top, bottom));
                //Debug.Log("bottom over");


            }
        }

    }

    public void Zoom(float zoomAmount)
    {
        camera.fieldOfView -= zoomAmount * zoomSpeed*Time.deltaTime;
        camera.fieldOfView = Mathf.Clamp(camera.fieldOfView, minFov, maxFov);
        CheckIfBeyondEdge();
    }
}
