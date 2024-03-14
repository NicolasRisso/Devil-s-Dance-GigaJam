using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class Trap : MonoBehaviour
{
    [Header("Trap Configuration")]
    [SerializeField] private LayerMask playerMask;

    private AudioSource audioSource;
    private BoxCollider boxCollider;
    private Animator animator;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        boxCollider = GetComponent<BoxCollider>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (!(((1 << other.gameObject.layer) & playerMask) != 0))
        {
            TrapActivated trapActivated = other.GetComponent<TrapActivated>();
            if (trapActivated != null)
            {
                boxCollider.enabled = false;
                trapActivated.Stun();
                StartCoroutine(TrapAudio(trapActivated));
            }
            else Debug.Log("No Trap Activator Script Found in GO");
        }
    }

    private IEnumerator TrapAudio(TrapActivated trapActivated)
    {
        animator.SetBool("IsTrapped", true);
        audioSource.Play();
        yield return new WaitForSeconds(Mathf.Max(audioSource.clip.length, trapActivated.GetStunDuration()));
        Destroy(gameObject);
    }
}
