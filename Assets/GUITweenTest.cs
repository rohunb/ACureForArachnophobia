using UnityEngine;
using System.Collections;

public class GuiTweenTest : MonoBehaviour
{

    private GameObject tweenObject;
    // Use this for initialization
    void Start()
    {
        tweenObject = new GameObject();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnGUI()
    {
        float xPos = tweenObject.transform.localPosition.x;
        float yPos = tweenObject.transform.localPosition.y;

        if (GUI.Button(new Rect(xPos, yPos, 100, 40), new GUIContent("click me")))
        {
            iTween.MoveTo(tweenObject, iTween.Hash("x",250, "y",50, "time", 6, "easeType", iTween.EaseType.easeInOutSine));
            
        }
    }
}