using UnityEngine;
using System.Collections;

public class TrapActivated : MonoBehaviour
{
    [Header("Trap Configuration")]
    [SerializeField] private float stunDuration;

    private bool isTrapped = false;

    public void Stun()
    {
        StartCoroutine(Paralize());
    }

    private IEnumerator Paralize()
    {
        isTrapped = true;
        yield return new WaitForSeconds(stunDuration);
        isTrapped = false;
    }

    public bool GetIsTrapped()
    {
        return isTrapped;
    }

    public float GetStunDuration()
    {
        return stunDuration;
    }
}
