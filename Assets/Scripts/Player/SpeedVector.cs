using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeedVector : MonoBehaviour
{
    public GameObject speedVectorPrefab;
    public GameObject parentObject;
    public Rigidbody parentRB;
    public Vector3 parentSpeed;
    public Material vectorMaterial;
    public TextMeshPro speedText;

    public float colorScale;
    public float emissionIntensityScale;
    public float lengthScale;

    private void Start()
    {
        parentRB = parentObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        parentSpeed = GetSpeedVector();
        UpdateVector();
    }

    private Vector3 GetSpeedVector()
    {
        return parentRB.velocity;
    }

    private void UpdateVector()
    {
        // update color
        float speedMode = parentSpeed.x;
        float gb = speedMode * colorScale;
        float r = 255f - gb;
        float intensity = speedMode * emissionIntensityScale;
        Color currentColor = new Color(r, gb, gb);
        vectorMaterial.SetColor("_EmissionColor",currentColor*intensity);

        // update length
        speedVectorPrefab.transform.localScale = 
            new Vector3(speedVectorPrefab.transform.localScale.x,-speedMode,
                speedVectorPrefab.transform.localScale.z);

        speedText.text = Mathf.Abs(speedMode).ToString("F2") + "m/s";
    }
    
    
    
}
