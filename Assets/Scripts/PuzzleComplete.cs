using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleComplete : MonoBehaviour
{
	public bool m_puzzleComplete;

	public Color m_puzzleIncompleteColour;
	public Color m_puzzleCompleteColour;

	public Light m_light;

    public float m_transitionTime = 0.5f;
    float m_progress = 0;


    void Start()
    {
		m_light.color = m_puzzleIncompleteColour;
    }

    private void Update()
    {
        if(m_puzzleComplete == true)
        {
            m_progress += Time.deltaTime / m_transitionTime;

            m_light.color = Color.Lerp(m_puzzleIncompleteColour, m_puzzleCompleteColour, m_progress);
        }
    }

    public void CompletePuzzle()
	{
		m_puzzleComplete = true;

		//m_light.color = m_puzzleCompleteColour;
	}
}
