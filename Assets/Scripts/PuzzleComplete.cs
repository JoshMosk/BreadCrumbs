using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleComplete : MonoBehaviour
{
	public bool m_puzzleComplete;

	public Color m_puzzleIncompleteColour;
	public Color m_puzzleCompleteColour;

	public Light m_light;

    void Start()
    {
		m_light.color = m_puzzleIncompleteColour;
    }

	public void CompletePuzzle()
	{
		m_puzzleComplete = true;

		m_light.color = m_puzzleCompleteColour;
	}
}
