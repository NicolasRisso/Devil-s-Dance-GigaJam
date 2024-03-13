using UnityEngine;

public class CameraLookahead : MonoBehaviour
{
    [Header("Lookahead Configuration")]
    [SerializeField] private float maxLookaheadDistance;
    [SerializeField] private float forwardLookaheadDistance;
    [SerializeField] private float backwardLookaheadDistance;
    [SerializeField] private float lookaheadSpeed;
    [SerializeField] private float directionChangeSpeedMultiplier;

    private Vector2 movementInput;
    private Vector2 lastMovementInput;
    private Vector3 offset;

    void Start()
    {
        offset = transform.position - transform.parent.position;
        lastMovementInput = Vector2.zero;
    }

    void Update()
    {
        movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        float speed = lookaheadSpeed;

        if (Mathf.Abs(movementInput.x - lastMovementInput.x) > 0.1f)
        {
            speed *= directionChangeSpeedMultiplier;
        }

        if (movementInput.sqrMagnitude > 0.01f && Vector3.Distance(transform.parent.position + offset, transform.position) <= maxLookaheadDistance)
        {
            bool isMovingForward = Input.GetAxis("Vertical") > 0;
            float lookaheadDistance = isMovingForward ? forwardLookaheadDistance : backwardLookaheadDistance;

            Vector3 lookaheadDirection = new Vector3(movementInput.x, movementInput.y, 0).normalized;
            Vector3 lookaheadTarget = transform.parent.position + lookaheadDirection * lookaheadDistance + offset;

            Vector3 newPosition = Vector3.Lerp(transform.position, lookaheadTarget, speed * Time.deltaTime);
            transform.position = newPosition;
        }
        else
        {
            Vector3 originalPosition = transform.parent.position + offset;
            transform.position = Vector3.Lerp(transform.position, originalPosition, speed * Time.deltaTime * 10f);
        }
    }
}