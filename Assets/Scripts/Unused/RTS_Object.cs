using UnityEngine;
using System.Collections;

public class RTS_Object : MonoBehaviour {

    public string name;
    public bool selectable;

    public enum ObjectType { Building, Unit, GUI}
    public ObjectType type;

	// Use this for initialization
	protected virtual void Start () {
	}
	
	// Update is called once per frame
    protected virtual void Update()
    {
	}
    protected virtual void FixedUpdate()
    {
    }
}
