using UnityEngine;
using System.Collections;

public class Lifebar : MonoBehaviour {

    Health health;
	// Use this for initialization
	void Start () {
        health = transform.parent.GetComponent<Health>();
	}
	
	// Update is called once per frame
	void Update () {
        renderer.material.SetFloat("_Cutoff", Mathf.InverseLerp( health.maxHealth, 0,health.GetHealth )); 
	}
}
