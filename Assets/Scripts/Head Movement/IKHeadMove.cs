using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKHeadMove : MonoBehaviour {

    public GameObject lookObject;
    public float speed = 1f;

    public Transform currentTarget;
    Animator anim;

    public float minDist = 100f;

    public float lookTimer;
    public List<PointOfInterest> poiList = new List<PointOfInterest>();

    void Start() {
        anim = GetComponent<Animator>();
    }

    private void Update() {

        GetClosestCharacter(Physics.OverlapSphere(transform.position, 1000f), 1000f);

        lookTimer -= Time.deltaTime;
        if (lookTimer <= 0f) {
            currentTarget = poiList[Random.Range(0, poiList.Count)].gameObject.transform;

            lookTimer = Random.Range(1f, 10f);
        }

        lookObject.transform.position = Vector3.Lerp(lookObject.transform.position, currentTarget.position + currentTarget.GetComponent<PointOfInterest>().offset, Time.deltaTime * speed);

        
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
        if (currentTarget) {
            anim.SetLookAtPosition(lookObject.transform.position);
            anim.SetLookAtWeight(1f);
        }
    }
}
