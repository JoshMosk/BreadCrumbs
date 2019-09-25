using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionScript : MonoBehaviour
{
    private int waveNumber;
    public float distanceX, distanceZ;
    public float[] rippleAmplitude;
    public float magnitudeDivider;
    Renderer rend;

    Mesh Mesh;
    // Start is called before the first frame update
    void Start()
    {
        Mesh = GetComponent<MeshFilter>().mesh;
        rend = GetComponent<Renderer>();

    }

    // Update is called once per frame
    void Update()
    {
       

        for (int i = 0; i < 8; i++)
        {

            rippleAmplitude[i] = rend.material.GetFloat("_RippleAmplitude" + (i + 1));
            if (rippleAmplitude[i] > 0)
            {
                rend.material.SetFloat("_RippleAmplitude" + (i + 1), rippleAmplitude[i] * 0.98f);  // over time ripple ruduces by 2% (0.98)
            }
            if (rippleAmplitude[i] < 0.05)
            {
                rend.material.SetFloat("_RippleAmplitude" + (i + 1), 0);
            }
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.rigidbody)
        {
            waveNumber++;
            if (waveNumber == 9)
            {
                waveNumber = 1;
            }
            rippleAmplitude[waveNumber - 1] = 0;

            distanceX = this.transform.position.x - col.gameObject.transform.position.x;
            distanceZ = this.transform.position.z - col.gameObject.transform.position.z;

            rend.material.SetFloat("_RippleOffsetX1" /*+ waveNumber*/, distanceX / Mesh.bounds.size.x * 2.5f);
            rend.material.SetFloat("_RippleOffsetZ1" /*+ waveNumber*/, distanceZ / Mesh.bounds.size.z * 2.5f);

            rend.material.SetFloat("_RippleAmplitude1" /*+ waveNumber*/, col.rigidbody.velocity.magnitude * magnitudeDivider);
        }

    }
}
