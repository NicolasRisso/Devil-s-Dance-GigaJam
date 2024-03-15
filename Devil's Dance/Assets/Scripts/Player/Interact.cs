using System.Collections;
using UnityEngine;

public class Interact : MonoBehaviour
{
    [Header("Interact Configuration")]
    [SerializeField] private float holdThreshold;
    [SerializeField] [Range(0.5f, 5f)] private float holdThresholdLimit;
    [SerializeField] private float interactAnimDuration = 0.82f;
    [SerializeField] private float interactRange = 0.75f;
    [SerializeField] private LayerMask interactableLayer;

    private Animator animator;

    private TrapInventory trapInventory;

    private bool isEKeyHeld = false;
    private float eKeyHoldTime = 0f;

    private bool interacting = false;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        trapInventory = GetComponent<TrapInventory>();
    }

    private void Update()
    {
        Debug.Log(eKeyHoldTime);
        if (Input.GetButtonDown("Interact"))
        {
            isEKeyHeld = true;
            eKeyHoldTime = 0f;
        }
        if (isEKeyHeld)
        {
            eKeyHoldTime += Time.deltaTime;
            if (eKeyHoldTime >= holdThreshold && eKeyHoldTime < holdThresholdLimit)
            {
                interacting = true;
                //Come�a Anim da trap
            }
            else if (eKeyHoldTime >= holdThresholdLimit)
            {
                bool tmp = trapInventory.PlaceTrap();
                isEKeyHeld = false;
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

    private void InteractSphere()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactRange, interactableLayer);
        foreach (Collider collider in hitColliders)
        {
            TrapPickup trapPickup = collider.GetComponent<TrapPickup>();
            if (trapPickup is { })
            {
                if (trapInventory.TryAddTrap())
                {
                    trapPickup.TrapPicked();
                }
                else trapPickup.NoSpace();
                return;
            }
        }
    }

    private IEnumerator StopInteractAnim()
    {
        yield return new WaitForSeconds(interactAnimDuration);
        InteractSphere();
        animator.SetBool("Interacting", false);
        interacting = false;
    }

    public bool GetInteracting()
    {
        return interacting;
    }
}
