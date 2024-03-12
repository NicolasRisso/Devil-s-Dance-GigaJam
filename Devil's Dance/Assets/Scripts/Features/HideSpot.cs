using System.Collections.Generic;
using UnityEngine;

public class HideSpot : MonoBehaviour
{
    private Dictionary<GameObject, int> triggerCount = new Dictionary<GameObject, int>();

    private void OnTriggerEnter(Collider other)
    {
        other.tag = "Hidden";
    }

    private void OnTriggerExit(Collider other)
    {
        other.tag = "Untagged";
    }
}
