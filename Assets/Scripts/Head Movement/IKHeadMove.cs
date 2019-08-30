using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKHeadMove : MonoBehaviour {

    Animator anim;
    public float minDist = 250f;
    public GameObject lookObject;
    
    public List<PointOfInterest> poiList = new List<PointOfInterest>();

    public float macroLookTimer;
    public float macroSpeed = 2f;
    public Transform macroTarget;

    public float microLookTimer;
    public float microSpeed = 1f;
    public Vector3 microTarget;
    public float microSphere = 3;

    public float microSmall = 2;
    public float microBig = 7;
    public float macroSmall = 2;
    public float macroBig = 20;


    void Start() {
        anim = GetComponent<Animator>();
    }

    private void Update() {

        GetClosestCharacter(Physics.OverlapSphere(transform.position, 1000f), 1000f);

        if (poiList.Count != 0) {

            macroLookTimer -= Time.deltaTime;
            microLookTimer -= Time.deltaTime;

            if (macroLookTimer <= 0f) {
                macroTarget = poiList[Random.Range(0, poiList.Count)].gameObject.transform;
                macroLookTimer = Random.Range(macroSmall, macroBig);
            }

            if (microLookTimer <= 0f) {
                microTarget = (macroTarget.position + macroTarget.GetComponent<PointOfInterest>().offset) + (Random.insideUnitSphere * microSphere);
                microLookTimer = Random.Range(microSmall, microBig);
            }

            lookObject.transform.position = Vector3.Lerp(lookObject.transform.position, microTarget, Time.deltaTime * macroSpeed);
        }
        
    }

    private void GetClosestCharacter(Collider[] characters, float radius) {
        poiList = new List<PointOfInterest>();
        foreach (Collider currentCharacter in characters) {
            if (currentCharacter.gameObject == gameObject)
                continue;

            PointOfInterest comp = currentCharacter.GetComponent<PointOfInterest>();
            if (comp != null) {
                Vector3 t = currentCharacter.transform.position - transform.position;
                float dist = t.x * t.x + t.y * t.y + t.z * t.z;  // Same as "= t.sqrMagnitude;" but faster
                if (dist < minDist) {
                    poiList.Add(comp);
                }
            }
        }
    }

    private void OnAnimatorIK(int layerIndex) {
        if (macroTarget && poiList.Count != 0) {
            anim.SetLookAtPosition(lookObject.transform.position);
            anim.SetLookAtWeight(1f);
        }
    }

    void OnDrawGizmosSelected() {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(lookObject.transform.position, .1f);

      //  Gizmos.color = Color.green;
     //   Gizmos.DrawWireSphere(macroTarget.transform.position, spherewsie);
    }
}
