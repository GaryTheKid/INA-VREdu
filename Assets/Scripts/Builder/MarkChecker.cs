using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkChecker : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mark"))
        {
            other.gameObject.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.green * 1.2f);
            Game_Manager.singleton.CompleteMarker(other);
        }

        if (other.CompareTag("TimerTrigger"))
        {
            Game_Manager.singleton.StartTimer();
        }

        if (other.CompareTag("TrackPart"))
        {
            other.gameObject.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", Color.green * 1.2f);
        }
    }
}
