using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle3 : MonoBehaviour
{
	CorruptPlayer m_corruptPlayer;

	GameObject m_gate;


	void Start()
    {
		m_corruptPlayer = FindObjectOfType<CorruptPlayer>();
    }

    void Update()
    {
        
    }

	private void OnTriggerEnter(Collider other)
	{
		if (m_corruptPlayer.m_isCorrupt)
		{
			if (other.gameObject.name == "Lylah")
			{
				//need to show shadow only in light mode// DONE
				//once triggered make sure inly lylah// DONE
				//disable trigger
				//allow the wall collider to be disabled while on dark

				//you know what im just gonna write swhit, so whebn she walks in light then we swap the texture, also have two meshes that 
				//allows for lylah to walk between the building that means have a coll that is disable when stand in light
			}
		}
	}
}
