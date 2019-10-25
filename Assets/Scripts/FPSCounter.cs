using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
	int frameCounter = 0;
	float timeCounter = 0f;
	float lastFramerate = 0f;
	public float refreshTime = 1f;

	public Text displayText;

    // Update is called once per frame
    void Update()
    {
        if( timeCounter <= refreshTime)
		{
			timeCounter += Time.deltaTime;
			frameCounter++;
		}
		else
		{
			lastFramerate = (float)frameCounter / timeCounter;
			frameCounter = 0;
			timeCounter = 0f;

			displayText.text = ((int)lastFramerate).ToString();
		}
    }
}
