using UnityEngine;
using System.Collections;

public class Clicker : MonoBehaviour {

    public delegate void OnClickEvent(GameObject g);
    public event OnClickEvent OnClick;

    public Camera cam;
    public int layerToCheck=4;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 100f,1<<layerToCheck))
        {
            if(Input.GetMouseButtonUp(0))
            {
                //OnClick(hit.transform.gameObject);
                Debug.Log("s");
            }
        }
	}
    
}
