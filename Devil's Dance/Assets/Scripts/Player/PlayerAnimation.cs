using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private bool animLocked = false;

    private Animator animator;
    private PlayerController playerController;

    private Vector2 moveInput = Vector2.zero;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponentInParent<PlayerController>();
    }

    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        animator.SetFloat("MoveMagnitude", moveInput.magnitude);

        if (!animLocked && moveInput != Vector2.zero)
        {
            animator.SetFloat("MoveX", moveInput.x);
            animator.SetFloat("MoveY", moveInput.y);
        }
        animator.speed = playerController.GetCurrentSpeedDifference();
    }
}
