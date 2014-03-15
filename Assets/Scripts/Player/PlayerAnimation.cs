using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour {

	static public PlayerAnimation thisScript;

	// When the program starts
	void Awake () 
	{	
		thisScript = GetComponent<PlayerAnimation>();
		//Set all the animations to the loop mode
		//We have running and idle, and they both need to be looped
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.anyKey)
		{
			//If forward axis is pressed, and is more than 0.2 (for smoother animation transition)
			if (Input.GetAxis("Vertical") > 0.2f)// && Health.Alive)
			{
				//Play the "run" animation in it's default speed
				animation.wrapMode = WrapMode.Loop;
				animation["run"].speed = 1.0f;
				animation.CrossFade ("run");
			}
			//If backward axis is being pressed, and is less then -0.2
			else if(Input.GetAxis("Vertical") < -0.2)// && Health.Alive)
			{
				//Play the same "run" animation, but this time, use a speed below 0 for reverse animation (As running backwards)
				animation.wrapMode = WrapMode.Loop;
				animation["run"].speed = -1.0f;
				animation.CrossFade ("run");
			}
		
		}
		else if(!Input.anyKey)// && Health.Alive)
		{
			animation.wrapMode = WrapMode.Loop;
			animation.CrossFade("idle");
		}
	}
	
	public static void PlayAnim(string anim, int speed, WrapMode mode)
	{
		thisScript.animation.wrapMode = mode;
		thisScript.animation[anim].speed = speed;
		thisScript.animation.CrossFadeQueued(anim);
	}
}
