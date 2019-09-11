using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialMenu : MonoBehaviour
{
	[Header("Scene")]
	public Transform m_selectionTransform = null;
	public Transform m_cursorTranform = null;
	public Transform m_arrow;

	public IInput m_input;

	[Header("Events")]
	public RadialSection top = null;
	public RadialSection right = null;
	public RadialSection bottom = null;
	public RadialSection left = null;

	private Vector2 touchPos = Vector2.zero;
	private List<RadialSection> radialSections = null;
	private RadialSection highlightedSection = null;

	private readonly float degreeIncrement = 90.0f;

	private void Awake()
	{
		CreateSections();
	}

	private void Start()
	{
		m_input = GetComponent<IInput>();
	}

	private void CreateSections()
	{
		radialSections = new List<RadialSection>()
		{
			top, right, bottom, left
		};

		foreach(RadialSection s in radialSections)
		{
            if(s.iconRenderer != null && s.icon != null)
			s.iconRenderer.sprite = s.icon;
		}
	}

	private void Update()
	{
		SetTouchPos(m_input.TouchPadPos);

		Vector2 direction = Vector2.zero + touchPos;
		float rot = GetDegree(direction);

		SetCursorPos();
		SetArrowRot();

		SetSelectionRotation(rot);
		SetSelectedEvent(rot);

		if(m_input.NPCInteractUp)
		{
			ActivateHighlightedSections();
		}
	}

	private float GetDegree(Vector2 dir)
	{
		float value = Mathf.Atan2(dir.x, dir.y);

		value *= Mathf.Rad2Deg;

		if(value < 0)		//so instead of -180 to 180 is 0 to 360
		{
			value += 360.0f;
		}

		return value;
	}

	private void SetCursorPos()
	{
		m_cursorTranform.localPosition = touchPos;
	}

	private void SetArrowRot()
	{
		m_arrow.LookAt(m_cursorTranform, new Vector3(0, 0, -1));
		
        //m_arrow.rotation = Quaternion.LookRotation(m_cursorTranform.position - m_arrow.position);
    }

	public void SetTouchPos(Vector2 newValue)
	{
        if(newValue.x != 0 && newValue.y != 0)
        {
		    touchPos = newValue;

        }
	}

	private void SetSelectionRotation(float newRotation)
	{
		float snappedRot = SnapRotation(newRotation);
		m_selectionTransform.localEulerAngles = new Vector3(0, 0, -snappedRot);
	}

	private float SnapRotation(float rotation)
	{
		return GetNearestIncrement(rotation) * degreeIncrement;
	}

	private int GetNearestIncrement(float rotation)
	{
		return Mathf.RoundToInt(rotation / degreeIncrement);
	}

	private void SetSelectedEvent(float currentRotation)
	{
		int index = GetNearestIncrement(currentRotation);

		if (index == 4)
			index = 0;

		highlightedSection = radialSections[index];
	}

	public void ActivateHighlightedSections()
	{
		//highlightedSection.onPress.Invoke();
		highlightedSection.button.onClick.Invoke();
	}
}
