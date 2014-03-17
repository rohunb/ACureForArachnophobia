using UnityEngine;
using System.Collections;

public class MuzzleFlash : MonoBehaviour {

	
	public void AnimateMuzzleFlash()
    {
        transform.localScale = Vector3.one * Random.Range(0.5f, 1.5f);
        Vector3 rot=transform.localEulerAngles;
        rot.z=Random.Range(0, 90.0f);
        transform.localEulerAngles = rot;
    }

}
