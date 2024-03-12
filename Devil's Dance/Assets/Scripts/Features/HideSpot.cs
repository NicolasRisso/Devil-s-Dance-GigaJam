using System.Collections.Generic;
using UnityEngine;

public class HideSpot : MonoBehaviour
{
    private static Dictionary<GameObject, int> triggerCount = new Dictionary<GameObject, int>();

    private void OnTriggerEnter(Collider other)
    {
        if (!triggerCount.ContainsKey(other.gameObject)) triggerCount[other.gameObject] = 1;
        else triggerCount[other.gameObject]++;
        other.tag = "Hidden";
    }

    private void OnTriggerExit(Collider other)
    {
        if (triggerCount.ContainsKey(other.gameObject)) triggerCount[other.gameObject]--;
        if (triggerCount[other.gameObject] <= 0)
        {
            other.tag = "Untagged";
            triggerCount.Remove(other.gameObject);
        }
    }
}
