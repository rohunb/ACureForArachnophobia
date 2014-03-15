using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class InputResolver : MonoBehaviour {

    public int terrainLayer=8;
    public int playerUnitsLayer=9;
    public int playerBuildingsLayer;
    public int enemyUnitsLayer;
    public int enemyBuildingsLayer;
    public int GUILayer;

    public enum InputResponse { Select, DragSelect, Execute, ViewUp, ViewDown, ViewLeft,ViewRight,Zoom,Cancel,BuildMenu,BuildUnit,BuildTank}

    public List<Soldier> selectedSoldiers;
    
    //scripts that we need for input handing
    private SoldierController soldierController;
    private CameraMover cameraMover;
    
	// Use this for initialization
	void Start () {
        soldierController = gameObject.GetComponent<SoldierController>();
        cameraMover = Camera.main.GetComponent<CameraMover>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ResolveInput(InputResponse response)
    {
        switch (response)
        {
            case InputResponse.Select:
                if (selectedSoldiers.Count > 0)
                {
                    Deselect();
                }
                CalculateSelectOrder();
                break;
            case InputResponse.Execute:

                Vector3 destination;
                if (CalculateRightClickOrder(out destination))
                {
                    foreach (Soldier soldier in selectedSoldiers)
                    {
                        soldier.SetDestination(new Vector3(destination.x, soldier.transform.position.y, destination.z));
                    }
                }
                break;
            case InputResponse.ViewUp:
                cameraMover.Pan(new Vector2(0.0f, 1.0f));
                break;
            case InputResponse.ViewDown:
                cameraMover.Pan(new Vector2(0.0f, -1.0f));
                break;
            case InputResponse.ViewLeft:
                cameraMover.Pan(new Vector2(-1.0f, 0.0f));
                break;
            case InputResponse.ViewRight:
                cameraMover.Pan(new Vector2(1.0f, 0.0f));
                break;
            case InputResponse.Cancel:
                break;
            case InputResponse.BuildMenu:
                break;
            case InputResponse.BuildUnit:
                break;
            case InputResponse.BuildTank:
                break;
            default:
                break;
        }
    }
    public void ResolveInput(InputResponse response, Rect selectBox)
    {
        if (response == InputResponse.DragSelect)
        {
            foreach (Soldier soldier in soldierController.soldiers)
            {
                Vector3 unitScreenPos = Camera.main.WorldToScreenPoint(soldier.transform.position);
                unitScreenPos.y = Screen.height - unitScreenPos.y;
                if (!selectedSoldiers.Contains(soldier) && selectBox.Contains(unitScreenPos))
                {
                    selectedSoldiers.Add(soldier);
                    soldier.selected = true;
                }
            }
        }
    }
    
    public void ResolveInput(InputResponse response, float zoomAmount)
    {
        cameraMover.Zoom(zoomAmount);
    }
    void CalculateSelectOrder()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, Mathf.Infinity, 1<<playerUnitsLayer))
        {
            Soldier soldier = hit.collider.gameObject.GetComponent<Soldier>();
            if (soldier)
            {
                soldier.selected = true;
                selectedSoldiers.Add(soldier);
            }
        }
    }
    void Deselect()
    {
        foreach (Soldier soldier in selectedSoldiers)
        {
            soldier.selected = false;
        }
        selectedSoldiers.Clear();
    }
    bool CalculateRightClickOrder(out Vector3 dest)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, Mathf.Infinity, 1<<terrainLayer))
        {
            dest=hit.point;
            return true;
        }
        else
        {
            dest = Vector3.zero;
            return false;
        }
    }
}
