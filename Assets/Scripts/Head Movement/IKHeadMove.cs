using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKHeadMove : MonoBehaviour {

    public GameObject lookObject;
    public float speed = 1f;

    public Transform currentTarget;
    Animator anim;

    public float lookTimer;
    public PointOfInterest[] poiList;

    void Start() {
        anim = GetComponent<Animator>();
        poiList = (PointOfInterest[])FindObjectsOfType(typeof(PointOfInterest));
    }

    private void Update() {

        

        lookTimer -= Time.deltaTime;
        if (lookTimer <= 0f) {
            currentTarget = poiList[Random.Range(0, poiList.Length)].gameObject.transform;

            lookTimer = Random.Range(1f, 10f);
        }

        lookObject.transform.position = Vector3.Lerp(lookObject.transform.position, currentTarget.position + currentTarget.GetComponent<PointOfInterest>().offset, Time.deltaTime * speed);
    }

    private void OnAnimatorIK(int layerIndex) {
        if (currentTarget) {
            anim.SetLookAtPosition(lookObject.transform.position);
            anim.SetLookAtWeight(1f);
        }
    }
}
