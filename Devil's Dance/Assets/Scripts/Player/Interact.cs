using System.Collections;
using UnityEngine;

public class Interact : MonoBehaviour
{
    [Header("Interact Configuration")]
    [SerializeField] private float holdThreshold = 0.5f;
    [SerializeField] private float interactAnimDuration = 0.82f;

    private Animator animator;

    private bool isEKeyHeld = false;
    private float eKeyHoldTime = 0f;

    private bool interacting = false;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            isEKeyHeld = true;
            eKeyHoldTime = 0f;
        }
        if (isEKeyHeld)
        {
            eKeyHoldTime += Time.deltaTime;
            if (eKeyHoldTime >= holdThreshold)
            {
                interacting = true;
            }
        }
        if (Input.GetButtonUp("Interact"))
        {
            if (eKeyHoldTime >= holdThreshold)
            {
                interacting = false;
            }
            else
            {
                animator.SetBool("Interacting", true);
                interacting = true;
                StartCoroutine(StopInteractAnim());
            }
            isEKeyHeld = false;
            eKeyHoldTime = 0f;
        }
    }

    private IEnumerator StopInteractAnim()
    {
        yield return new WaitForSeconds(interactAnimDuration);
        animator.SetBool("Interacting", false);
        interacting = false;
    }

    public bool GetInteracting()
    {
        return interacting;
    }
}
