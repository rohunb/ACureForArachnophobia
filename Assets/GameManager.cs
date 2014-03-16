using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public List<Subject> nodes;
    public Observer nodeWatcher;

	// Use this for initialization
	void Awake () {
        nodeWatcher.subjects = nodes;
        foreach (Node node in nodes)
        {
            node.Attach(nodeWatcher);
        }
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
