using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldFlicker : MonoBehaviour {
    private MeshRenderer meshRenderer;
    private float nextTime;
    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }
    void Update () {
        if (Time.time > nextTime)
        {
            meshRenderer.material.SetColor("_TintColor", new Color(1f, 1f, 1f, Random.Range(0.15f, 0.25f)));
            nextTime = Time.time + Random.Range(0.02f, 0.05f);
        }
    }
}
