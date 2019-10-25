using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EpiloguePlayerPosReset : MonoBehaviour
{
	GameObject player;

	//need to reset raypoint in player mover and target in player mover

    // Start is called before the first frame update
    void Start()
    {
		player = GameObject.Find("Player");

		FindObjectOfType<LinePointAutoSetter>().DoTheThing();
		HandAnimController[] hand = FindObjectsOfType<HandAnimController>();
		foreach(HandAnimController h in hand)
		{
			h.DoTheThing();
		}
    }

    // Update is called once per frame
    void Update()
    {
		player.transform.position = this.transform.position;
    }
}
