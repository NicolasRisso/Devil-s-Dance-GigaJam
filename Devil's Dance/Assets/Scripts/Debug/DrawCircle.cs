using UnityEngine;

public class DrawCircle : MonoBehaviour
{
    [SerializeField] private float radius = 10f;
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}