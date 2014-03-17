using UnityEngine;
using System.Collections;

public class UpgradeManager : MonoBehaviour {


    
    
    private int credits;
    public int GetCredits()
    {
        return credits;
    }
    public bool CreateTransaction(int amount)
    {
        if (credits + amount >= 0)
        {
            credits += amount;
            return true;
        }
        else
            return false;
    }

    void OnGUI()
    {
        DisplayUpgradeButtons();

    }
    void DisplayUpgradeButtons()
    {
         
    }


}
