using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private float speed;
    [SerializeField] private float gravity;
    [Header("Terrain Detection")]
    [SerializeField] private float groundDistance;
    [SerializeField] private LayerMask terrainLayer;
    [Header("Flip Sprite")]
    [SerializeField] private Transform rotationTransform;

    private CharacterController charController;

    private bool isGrounded = false;

    private Vector3 velocity;

    private void Start()
    {
        charController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Gravity();
        GroundCheck();
        Movement();
    }

    private void Gravity()
    {
        velocity.y += gravity * Time.deltaTime;
        charController.Move(velocity * Time.deltaTime);
    }

    private void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(transform.position - new Vector3(0f, 0.5f * rotationTransform.localScale.x * charController.height, 0f), groundDistance, terrainLayer);
        if (isGrounded && velocity.y < 0) velocity.y = -2f;
    }

    private void Movement()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector3 moveDir = new Vector3(x, 0, y);
        charController.Move(moveDir * speed * Time.deltaTime);
        FlipSprite(x);
    }

    private void FlipSprite(float x)
    {
        if (x != 0 && x < 0) rotationTransform.rotation = Quaternion.Euler(rotationTransform.rotation.eulerAngles.x, 180f, rotationTransform.rotation.eulerAngles.z);
        else if (x != 0 && x > 0) rotationTransform.rotation = Quaternion.Euler(rotationTransform.rotation.eulerAngles.x, 0f, rotationTransform.rotation.eulerAngles.z);
    }

    public bool GetIsGrounded()
    {
        return isGrounded;
    }

    public LayerMask GetFloorLayer()
    {
        return terrainLayer;
    }

    public float GetPixelArtScale()
    {
        return rotationTransform.localScale.x;
    }
}