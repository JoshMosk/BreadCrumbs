using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialMenu : MonoBehaviour
{
	[Header("Scene")]
	public Transform m_selectionTransform = null;
	public Transform m_cursorTranform = null;

	[Header("Events")]
	//public RadialSection top = null;
	//public RadialSection right = null;
	//public RadialSection bottom = null;
	//public RadialSection left = null;

	private Vector2 touchPos = Vector2.zero;
	//private List<RadialSection> radialSection = null;
	//private RadialSection highlighttedSection = null;

	private readonly float degreeIncrement = 90.0f;

	private void Update()
	{
		Vector2 direction = Vector2.zero + touchPos;//TODO: JM START HERE WATCH https://www.youtube.com/watch?v=2SpegTl1wP8 @6:30
	}
}
