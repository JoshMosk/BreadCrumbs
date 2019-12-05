using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LODManager : MonoBehaviour {

    void Start() {
        Camera camera = Camera.main;
        float[] distances = new float[32];
        distances[8] = 15;
        distances[9] = 15;
        distances[10] = 15;
        distances[11] = 15;
        camera.layerCullDistances = distances;
    }

}
