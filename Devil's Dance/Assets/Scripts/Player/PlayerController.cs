using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(Stamina))]
public class PlayerController : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float gravity;
    [Header("Terrain Detection")]
    [SerializeField] private float groundDistance;
    [SerializeField] private LayerMask terrainLayer;
    [Header("Flip Sprite")]
    [SerializeField] private Transform rotationTransform;

    private CharacterController charController;
    private Stamina stamina;

    private bool isGrounded = false;

    private Vector3 velocity;

    private float speed;

    private void Start()
    {
        charController = GetComponent<CharacterController>();
        stamina = GetComponent<Stamina>();
        speed = walkSpeed;
    }

    private void Update()
    {
        Gravity();
        GroundCheck();
        AdjustSpeed();
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
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        if (x != 0 || y != 0)
        {
            Vector3 moveDir = new Vector3(x, 0, y).normalized;
            charController.Move(moveDir * speed * Time.deltaTime);
        }
        FlipSprite(x);
    }

    private void AdjustSpeed()
    {
        if (stamina.GetIsRunning())
        {
            speed = runSpeed;
        }
        else
        {
            speed = walkSpeed;
        }
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

    public float GetPixelArtScale()
    {
        return rotationTransform.localScale.x;
    }

    public float GetCurrentSpeedDifference()
    {
        return (speed / walkSpeed);
    }

    public LayerMask GetFloorLayer()
    {
        return terrainLayer;
    }
}