using UnityEngine;

public class CameraSway : MonoBehaviour
{
    [Header("Sway Configuration")]
    [SerializeField] private float angle = 1.0f;
    [SerializeField] private float verticalAngle = 1.0f;
    [SerializeField] private float speed = 1.0f;

    private Quaternion startRotation;

    void Start()
    {
        startRotation = transform.rotation;
    }

    void Update()
    {
        float horizontalAngleChange = Mathf.Sin(Time.time * speed) * angle;
        float verticalAngleChange = Mathf.Cos(Time.time * speed) * verticalAngle;

        transform.rotation = startRotation * Quaternion.Euler(verticalAngleChange, horizontalAngleChange, 0);
    }
}
