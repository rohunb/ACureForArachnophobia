using UnityEngine;
using System.Collections;

public class MenuButton : MonoBehaviour {

    TextMesh textMesh;
    public bool clickable;
	
    void Awake()
    {
        textMesh = GetComponent<TextMesh>();
    }
	public void SetText(string _text)
    {
        textMesh.text = _text;

    }
    void OnMouseEnter()
    {
        if (clickable)
        {
            renderer.material.color = Color.red;
            AudioManager.Instance.PlaySound(AudioManager.Sound.Click,false);
        }
    }
    void OnMouseExit()
    {
        if (clickable)
        renderer.material.color = Color.white;
    }
    void OnMouseUp()
    {
        if (clickable)
        {
            SceneManager.Instance.OnClick(textMesh.text);
            AudioManager.Instance.PlaySound(AudioManager.Sound.Click,false);
        }
    }
}
