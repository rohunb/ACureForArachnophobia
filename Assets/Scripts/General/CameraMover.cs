using UnityEngine;
using System.Collections;

public class CameraMover : MonoBehaviour {
    
    public float camerMoveSpeed=35f;

    public float zoomSpeed = 8f;
    public float minFov = 20.0f;
    public float maxFov = 85.0f;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void Pan(Vector2 panVector)
    {
        transform.Translate(new Vector3(panVector.x * camerMoveSpeed * Time.deltaTime, panVector.y * camerMoveSpeed * Time.deltaTime,0.0f));
    }
    public void Zoom(float zoomAmount)
    {
        camera.fieldOfView -= zoomAmount * zoomSpeed;
        camera.fieldOfView = Mathf.Clamp(camera.fieldOfView, minFov, maxFov);
    }
}
