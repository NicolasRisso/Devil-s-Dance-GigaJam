using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private float speed;
    [SerializeField] private float groundDistance;
    [Header("Terrain Detection")]
    [SerializeField] private LayerMask terrainLayer;
    [Header("Sprite Rendering")]
    [SerializeField] private SpriteRenderer sRenderer;

    private Rigidbody rBody;

    private void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        FloorDetection();
        Movement();
    }

    private void FloorDetection()
    {
        RaycastHit hit;
        Vector3 castPos = transform.position;
        castPos.y += 1;
        if (Physics.Raycast(castPos, -transform.up, out hit, Mathf.Infinity, terrainLayer))
        {
            if (hit.collider != null)
            {
                Vector3 movePos = transform.position;
                movePos.y = hit.point.y + groundDistance;
                transform.position = movePos;
            }
        }
    }

    private void Movement()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector3 moveDir = new Vector3(x, 0, y);
        rBody.velocity = moveDir * speed;
        FlipSprite(x);
    }

    private void FlipSprite(float x)
    {
        if (x != 0 && x < 0) sRenderer.flipX = true;
        else if (x != 0 && x > 0) sRenderer.flipX = false;
    }
}
