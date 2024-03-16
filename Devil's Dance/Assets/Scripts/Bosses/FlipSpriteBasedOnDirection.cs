using UnityEngine;

public class FlipSpriteBasedOnDirection : MonoBehaviour
{
    private Vector3 previousPosition;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        previousPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Vector3 direction = transform.position - previousPosition;
        if (Mathf.Abs(direction.x) > 0.01f)
        {
            spriteRenderer.flipX = direction.x < 0;
        }
        previousPosition = transform.position;
    }
}
