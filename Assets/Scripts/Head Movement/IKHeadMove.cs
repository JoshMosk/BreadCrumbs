using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKHeadMove : MonoBehaviour {


    [Header("Setup")]
    public GameObject lookObject;
    public float searchDistance = 250f;
    public List<PointOfInterest> pointsOfInterest = new List<PointOfInterest>();
    private Animator anim;


    [Header("Speed")]
    public float moveSpeedHigh;
    public float moveSpeedLow;


    [Header("Large Head Movements")]
    public float macroSmall;
    public float macroBig;

    private float macroLookTimer;
    private Transform macroTarget;


    [Header("Small Head Movements")]
    public float microSmall;
    public float microBig;

    public float microSphere;

    private float microLookTimer;
    private Vector3 microTarget;

    DialogueManager dialogueMan;

    void Start() {
        anim = GetComponent<Animator>();
        dialogueMan = DialogueManager.instance;
    }

    private void Update() {

        macroLookTimer -= Time.deltaTime;
        microLookTimer -= Time.deltaTime;

        if (DialogueManager.instance.inConversation) {
            macroTarget = dialogueMan.speakerObject;

        } else if (macroLookTimer <= 0f) {
            GetClosestCharacter(Physics.OverlapSphere(transform.position, 100f), 100f);
            macroTarget = pointsOfInterest[Random.Range(0, pointsOfInterest.Count)].gameObject.transform;
            macroLookTimer = Random.Range(macroSmall, macroBig);
        }

        if (microLookTimer <= 0f) {
            microTarget = (macroTarget.position + macroTarget.GetComponent<PointOfInterest>().offset) + (Random.insideUnitSphere * microSphere);
            microLookTimer = Random.Range(microSmall, microBig);
        }

        if (microTarget.y <= 0f) microTarget.y = 3f;

        lookObject.transform.position = Vector3.Lerp(lookObject.transform.position, microTarget, Time.deltaTime * Random.Range(moveSpeedLow, moveSpeedHigh));
    }

    private void GetClosestCharacter(Collider[] characters, float radius) {
        pointsOfInterest = new List<PointOfInterest>();
        foreach (Collider currentCharacter in characters) {
            if (currentCharacter.gameObject == gameObject)
                continue;

            PointOfInterest comp = currentCharacter.GetComponent<PointOfInterest>();
            if (comp != null) {
                Vector3 t = currentCharacter.transform.position - transform.position;
                float dist = t.x * t.x + t.y * t.y + t.z * t.z;
                if (dist < searchDistance) {
                    pointsOfInterest.Add(comp);
                }
            }
        }
    }

    private void OnAnimatorIK(int layerIndex) {
        anim.SetLookAtWeight(1f);
        if (macroTarget && pointsOfInterest.Count != 0) anim.SetLookAtPosition(lookObject.transform.position);
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(lookObject.transform.position, .1f);

        Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(macroTarget.transform.position + macroTarget.GetComponent<PointOfInterest>().offset, microSphere * 2);
    }
}
