using UnityEngine;

public class TrapActivated : MonoBehaviour
{
    [Header("Trap Configuration")]
    [SerializeField] private float stunDuration;

    private bool isTrapped = false;

    public bool GetIsTrapped()
    {
        return isTrapped;
    }
}
