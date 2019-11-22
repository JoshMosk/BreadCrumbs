using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinePointAutoSetter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		DoTheThing();
    }

	public void DoTheThing()
	{
		FindObjectOfType<PlayerMover>().m_rayPoint = this.transform;
	}
}
