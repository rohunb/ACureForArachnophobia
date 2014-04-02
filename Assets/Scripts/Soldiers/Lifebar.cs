using UnityEngine;
using System.Collections;

public class Lifebar : MonoBehaviour {

    Health health;
    public bool manual;
	// Use this for initialization
	void Start () {
        health = transform.parent.GetComponent<Health>();
	}
	
	// Update is called once per frame
	void Update () {
        if (manual)
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                health.UpdateHealth(-10);
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                health.UpdateHealth(10);
            }
        }
        renderer.material.SetFloat("_Cutoff", Mathf.InverseLerp( health.maxHealth, 0,health.GetHealth )); 
	}
}
