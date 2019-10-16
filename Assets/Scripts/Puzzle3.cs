using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle3 : MonoBehaviour
{
	//TODO
	//new system
	//when player is in the light while in corrupt unlocks puzzle
	//puzzle triggers lamp post to turn green and the lamp will tell puzzle master that puzzle 3 is complete

	public CorruptPlayer corrupt;
	public PuzzleComplete puzzleCompleteLamp;

	private void Start()
	{
		corrupt = FindObjectOfType<CorruptPlayer>();
	}

	private void Update()
	{
		
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.name == "Lylah")
		{
			Debug.Log("well you kinda got it");
			if (corrupt.m_isCorrupt)
			{
				Debug.Log("yay you actually got it");
				puzzleCompleteLamp.CompletePuzzle();//.GetComponent<Light>().color = Color.green;
			}
		}
	}

	//CorruptPlayer m_corruptPlayer;		//the player

	//public GameObject m_gate;      //used to block lylah
	//public GameObject m_holeInTheWall;          //the hole that will disppear once puzzle is solved
	//public Material m_buildingSolvedMat;

	//[SerializeField]
	//bool m_puzzleComplete = false;

	//void Start()
	//   {
	//	m_corruptPlayer = FindObjectOfType<CorruptPlayer>();
	//   }

	//   void Update()
	//   {

	//   }

	//private void OnTriggerEnter(Collider other)
	//{
	//	if (m_corruptPlayer.m_isCorrupt == false)
	//	{
	//		if (other.gameObject.name == "Lylah")
	//		{
	//			if(m_puzzleComplete == false)
	//			{
	//				m_holeInTheWall.GetComponent<Renderer>().material = m_buildingSolvedMat;

	//				//make building disappear when in dark
	//				m_holeInTheWall.AddComponent<MajorVRProj.CorruptBuilding>();
	//				FindObjectOfType<MajorVRProj.CorruptBuildingController>().corruptBuildings.Add(m_holeInTheWall.GetComponent<MajorVRProj.CorruptBuilding>());

	//				//disable gate
	//				m_gate.GetComponent<Collider>().enabled = false;

	//				//puzzle is complete
	//				m_puzzleComplete = true;
	//			}




	//			//need to show shadow only in light mode// DONE
	//			//once triggered make sure inly lylah// DONE
	//			//disable trigger
	//			//allow the wall collider to be disabled while on dark

	//			//you know what im just gonna write swhit, so whebn she walks in light then we swap the texture, also have two meshes that 
	//			//allows for lylah to walk between the building that means have a coll that is disable when stand in light
	//		}
	//	}
	//}
}
