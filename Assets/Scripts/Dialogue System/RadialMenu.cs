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
    public DialogueManager dialogue;

    [Header("Events")]
    public RadialSection top = null;
    public RadialSection right = null;
    public RadialSection bottom = null;
    public RadialSection left = null;

    private Vector2 touchPos = Vector2.zero;
    private List<RadialSection> radialSections = null;
    private RadialSection highlightedSection = null;

    private float degreeIncrement = 90.0f;
    bool m_isBinary;

    private void Awake()
    {
        CreateSections();
    }

    private void Start()
    {
		dialogue = DialogueManager.instance;
        m_input = GetComponent<IInput>();
    }

    private void CreateSections()
    {
        radialSections = new List<RadialSection>()
        {
            top, right, bottom, left
        };

        foreach (RadialSection s in radialSections)
        {
            if (s.iconRenderer != null && s.icon != null)
                s.iconRenderer.sprite = s.icon;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            m_isBinary = !m_isBinary;
            SetBinaryOption(m_isBinary);
        }

        SetTouchPos(m_input.TouchPadPos);       //get touch pad pos

        Vector2 direction = Vector2.zero + touchPos;        //idk
        float rot = GetDegree(direction);       //gets the degree of rotation

        SetCursorPos();     //sets cursor pos
        SetArrowRot();      //sets arrow pos

        SetSelectionRotation(rot);      //finds out which option is selected
        SetSelectedEvent(rot);      //sets the event of said option

        if (m_input.NPCInteractDown && DialogueManager.instance.isMultipleChoice)        //if input is down and mchoice is active in convo
        {

			if (dialogue.cooldownTimerNextNode <= 0.0f)      //if cooldown is done
            {
				dialogue.cooldownTimerNextNode = 0.5f;      //reset cooldown
                ActivateHighlightedSections();      //press button
            }
        }
    }

    private float GetDegree(Vector2 dir)
    {
        float value = Mathf.Atan2(dir.x, dir.y);
        value *= Mathf.Rad2Deg;

        if (value < 0)      //so instead of -180 to 180 is 0 to 360
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
        Vector2 direction = m_cursorTranform.localPosition;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        m_arrow.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);        //make the forward be the parents forward		//doesnt work

    }

    public void SetTouchPos(Vector2 newValue)
    {
        if (newValue.x != 0 && newValue.y != 0)
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

        //if we want to have yes or no i thing it needs to be done here

        if (m_isBinary)
        {
            if (index == 2)
                index = 0;

            highlightedSection = radialSections[index * 2];
        }
        else
        {
            if (index == 4)
                index = 0;

            highlightedSection = radialSections[index];
        }
        


    }

    public void ActivateHighlightedSections()
    {
        //highlightedSection.onPress.Invoke();
        highlightedSection.button.onClick.Invoke();
    }

    public void SetBinaryOption(bool isBinary)        //sets the dialogue to either act as 4 option or yes and no
    {
		m_isBinary = isBinary;
        //need to set degree increment to be 2
        //need to set objects for option 1 and 3 off
        // or do I?
        if(isBinary == true)
        {
            degreeIncrement = 180.0f;
        }
        else
        {
            degreeIncrement = 90.0f;
        }

		Debug.Log("SET BINARY");
		radialSections[1].button.gameObject.SetActive(!isBinary);
        radialSections[3].button.gameObject.SetActive(!isBinary);
    }
}
