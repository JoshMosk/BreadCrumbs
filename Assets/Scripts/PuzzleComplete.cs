using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleComplete : MonoBehaviour
{
	bool m_puzzleComplete;

	public Color m_puzzleIncompleteColour;
	public Color m_puzzleCompleteColour;

	public Light m_light;




    // Start is called before the first frame update
    void Start()
    {
		m_light.color = m_puzzleIncompleteColour;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void CompletePuzzle()
	{
		m_puzzleComplete = true;

		m_light.color = m_puzzleCompleteColour;
	}
}
