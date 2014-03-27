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
    public GameObject posMarker;
    public enum InputResponse { Select, DragSelect, AttackMove,Move, ViewUp, ViewDown, ViewLeft,ViewRight,Zoom,Cancel,BuildMenu,WeaponMenu}

    public List<Soldier> selectedSoldiers;
    
    //scripts that we need for input handing
    private SoldierManager soldierController;
    private CameraMover cameraMover;
    UpgradeManager upgradeManager;
    
	// Use this for initialization
	void Awake () {
        soldierController = gameObject.GetComponent<SoldierManager>();
        cameraMover = Camera.main.GetComponent<CameraMover>();
        upgradeManager = GameObject.FindObjectOfType<UpgradeManager>().GetComponent<UpgradeManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ResolveInput(InputResponse response)
    {
        Vector3 destination;
        Vector3[] groupMoveDest;
        switch (response)
        {
            case InputResponse.Select:

                if (!upgradeManager.CheckIfGuiCick(Input.mousePosition))
                {
                    if (selectedSoldiers.Count > 0)
                    {
                        Deselect();
                    }
                    CalculateSelectOrder();
                }
                break;
            case InputResponse.AttackMove:
                if (CalculateMoveDestination(out destination))
                {
                    if (selectedSoldiers.Count > 1)
                    {
                        groupMoveDest = CalculateGroupMove(destination);
                        for (int i = 0; i < selectedSoldiers.Count; i++)
                        {
                            selectedSoldiers[i].SetAttackMove(new Vector3(groupMoveDest[i].x, selectedSoldiers[i].transform.position.y, groupMoveDest[i].z));
                        }

                    }
                    else
                        selectedSoldiers[0].SetAttackMove(new Vector3(destination.x, selectedSoldiers[0].transform.position.y, destination.z));
                    //foreach (Soldier soldier in selectedSoldiers)
                    //{
                    //    soldier.SetMove(new Vector3(destination.x, soldier.transform.position.y, destination.z));
                    //}
                }
                break;
            case InputResponse.Move:
                if (CalculateMoveDestination(out destination))
                {
                    if (selectedSoldiers.Count > 1)
                    {
                        groupMoveDest = CalculateGroupMove(destination);
                        for (int i = 0; i < selectedSoldiers.Count; i++)
                        {
                            selectedSoldiers[i].SetMove(new Vector3(groupMoveDest[i].x, selectedSoldiers[i].transform.position.y, groupMoveDest[i].z));
                        }

                    }
                    else
                        selectedSoldiers[0].SetMove(new Vector3(destination.x, selectedSoldiers[0].transform.position.y, destination.z));

                    //foreach (Soldier soldier in selectedSoldiers)
                    //{
                    //    soldier.SetMove(new Vector3(destination.x, soldier.transform.position.y, destination.z));
                    //}
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
    public void EquipWeapon(GameObject _weapon)
    {
        for (int i = 0; i < selectedSoldiers.Count; i++)
        {
            Weapon weapon = _weapon.GetComponent(typeof(Weapon)) as Weapon;
            if (weapon.wpnName != selectedSoldiers[i].currentWeapon.wpnName)
            {
                if (upgradeManager.CreateTransaction(-weapon.cost))
                    selectedSoldiers[i].EquipWeapon(weapon.wpnName);
                else
                    Debug.Log("not enough credits");
            }
        }
        Destroy(_weapon);
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
    Vector3[] CalculateGroupMove(Vector3 dest)
    {
        Vector3 averagePos = Vector3.zero;
        int numSoldiers = selectedSoldiers.Count;
        Vector3[] posArr = new Vector3[numSoldiers];
        Vector3[] groupMoveDests = new Vector3[numSoldiers];
        //Vector3[] localPos = new Vector3[numSoldiers];
        float formationGap=2.0f;

        for (int i = 0; i < numSoldiers; i++)
        {
            posArr[i]=selectedSoldiers[i].transform.position;
            averagePos += posArr[i];
        }

        averagePos = averagePos / numSoldiers;
        
        KDTree posTree = KDTree.MakeFromPoints(posArr);
        int nearestToAvgPos=posTree.FindNearest(averagePos);
        //calculating local positions
        //for (int i = 0; i < numSoldiers; i++)
        //{
        //    if (i != nearestToAvgPos)
        //    {
        //        localPos[i] = selectedSoldiers[nearestToAvgPos].transform.InverseTransformDirection(selectedSoldiers[i].transform.position);
        //    }
        //    else
        //    {
        //        localPos[i] = Vector3.zero;
        //    }
        //}
        Soldier centralSoldier=selectedSoldiers[nearestToAvgPos];
        //SortPerDistFromCenter(localPos,localPos[nearestToAvgPos]);
        SortPerDistFromCenter(posArr, posArr[nearestToAvgPos]);
        //int centralSoldierIndex=0;
        //for (int i = 0; i < numSoldiers; i++)
        //{
        //    if(selectedSoldiers[i]==centralSoldier)
        //    {
        //        centralSoldierIndex = i;
        //        break;
        //    }
        //}
        groupMoveDests[0] = dest;
        Vector3 vecToDist = dest - centralSoldier.transform.position;
        Vector3 normal;
        for (int i = 1; i < numSoldiers; i++)
        {
            if(i%2==0)
            {
                normal = new Vector3(-vecToDist.z, vecToDist.y, vecToDist.x).normalized;
                //groupMoveDests[i] = new Vector3(-vecToDist.z, vecToDist.y, vecToDist.x) * formationGap * i / 2;
                groupMoveDests[i] = dest+normal * formationGap * i / 2;
                //Debug.Log("i: " + i + " gap: " + formationGap * i / 2);
            }
            else
            {
                normal = new Vector3(vecToDist.z, vecToDist.y, -vecToDist.x).normalized;
                //groupMoveDests[i] = new Vector3(vecToDist.z, vecToDist.y, -vecToDist.x) * formationGap * (i - i / 2);
                groupMoveDests[i] = dest+normal * formationGap * (i-i / 2);
                //Debug.Log("i: " + i + " gap: " + formationGap * (i - i / 2));
            }
        }
        
        return groupMoveDests;
    }
    void Deselect()
    {
        
        foreach (Soldier soldier in selectedSoldiers)
        {
            soldier.selected = false;
        }
        selectedSoldiers.Clear();
    }
    bool CalculateMoveDestination(out Vector3 dest)
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
    void SortPerDistFromCenter(Vector3[] arr, Vector3 center)
    {
        float[] distances=new float[arr.Length];
        for (int i = 0; i < arr.Length; i++)
        {
            distances[i] = Vector3.Distance(arr[i], center);
        }

        for (int i = distances.Length - 1; i >= 0; i--)
        {
            for (int j = 1; j <= i; j++)
            {
                if (distances[j - 1] > distances[j])
                {
                    float temp = distances[j];
                    distances[j] = distances[j - 1];
                    distances[j - 1] = temp;
                    Soldier tmpSld = selectedSoldiers[j];
                    selectedSoldiers[j] = selectedSoldiers[j - 1];
                    selectedSoldiers[j - 1] = tmpSld;
                }
            }
        }
        
    }
}
