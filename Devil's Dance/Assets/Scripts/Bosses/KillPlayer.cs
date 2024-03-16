using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == playerLayer)
        {
            Debug.Log("O monstro tocou no player!");
        }
    }
}
