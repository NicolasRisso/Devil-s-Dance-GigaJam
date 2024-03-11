using UnityEngine;

public class FOVVisualizer : MonoBehaviour
{
    [Header("FOV Visualizer Debug Options")]
    [SerializeField] private float viewRadius;
    [SerializeField] [Range(0, 360)] private float viewAngle;

    void OnDrawGizmos()
    {
        // Desenha o raio de visão
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        // Calcula a direção do ângulo de visão à esquerda e à direita
        Vector3 viewAngleA = DirFromAngle(-viewAngle / 2, false);
        Vector3 viewAngleB = DirFromAngle(viewAngle / 2, false);

        // Desenha as linhas do ângulo de visão
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + viewAngleA * viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + viewAngleB * viewRadius);
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
