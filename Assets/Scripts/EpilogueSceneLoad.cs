using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EpilogueSceneLoad : MonoBehaviour
{
	public PuzzleComplete puzzle1Complete;
	public PuzzleComplete puzzle2Complete;
	public PuzzleComplete puzzle3Complete;
	public PuzzleComplete puzzle4Complete;

	private void OnTriggerEnter(Collider other)
	{
		if (other.name == "Lylah")
		{
			if (puzzle1Complete.m_puzzleComplete == true || puzzle2Complete.m_puzzleComplete == true || puzzle3Complete.m_puzzleComplete == true || puzzle4Complete.m_puzzleComplete == true)
			{
				SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
			}
		}


	}
}
