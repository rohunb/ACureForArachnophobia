using UnityEngine;
using System.Collections;

public class LightningSpawn : MonoBehaviour {

    LineRenderer line;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
    }
	// Use this for initialization
	void Start () {
	
	}
	void Update()
    {
        //CreateBeamEffect(Mathf.RoundToInt(20.0f) + 1, .5f);
    }
    public void CreateBeamEffect(int length, float lineNoise)
    {
        line.SetVertexCount(length);
        float noise = 0.1f;
        float noiseIncrement = lineNoise / (float)length;
        noiseIncrement *= 2f;
        line.SetPosition(0, transform.position);
        for (int i = 1; i < length - 1; i++)
        {
            Vector3 newPos = transform.position;
            //Vector3 offset = Vector3.zero;
            //offset.x = newPos.x + i * transform.forward.x + Random.Range(-noise, noise);
            ////offset.x = newPos.x + i * shootPoint.forward.x + Random.Range(-lineNoise, lineNoise);
            //offset.y = newPos.y + i * transform.forward.y;// +Random.Range(-lineNoise, lineNoise);
            //offset.z = newPos.z + i * transform.forward.z;// +Random.Range(-lineNoise, lineNoise);
            //newPos = offset;
            //line.SetPosition(i, newPos);

            float offsetX = Random.Range(-noise, noise);
            newPos += transform.right * offsetX;
            newPos += transform.forward * i;
            line.SetPosition(i, newPos);

            if (i > length / 2)
                noise -= noiseIncrement;
            else
                noise += noiseIncrement;
        }
        line.SetPosition(length - 1, transform.position + transform.forward * (length - 1));
    }
}
