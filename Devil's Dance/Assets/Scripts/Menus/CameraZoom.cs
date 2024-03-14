using UnityEngine;
using System.Collections;

public class CameraZoom : MonoBehaviour
{
    [Header("Camera Zoom Configuration")]
    [SerializeField] private float distance;
    [SerializeField] private float duration;

    public void StartZoom()
    {
        StartCoroutine(MoveForward(distance, duration));
    }

    private IEnumerator MoveForward(float distanceToMove, float timeToMove)
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + transform.forward * distanceToMove;

        float elapsedTime = 0;

        while (elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = endPosition;
    }
}
