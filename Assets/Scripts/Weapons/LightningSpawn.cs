using UnityEngine;
using System.Collections;

public class LightningSpawn : MonoBehaviour {

    LineRenderer line;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
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
