using System.Collections;
using UnityEngine;

public class TrapPickup : MonoBehaviour
{
    [SerializeField] private AudioClip pickUpClip;
    [SerializeField] private AudioClip fullClip;

    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    public void TrapPicked()
    {
        spriteRenderer.enabled = false;
        meshRenderer.enabled = false;
        audioSource.clip = pickUpClip;
        audioSource.Play();
        StartCoroutine(Destroy());
    }

    public void NoSpace()
    {
        audioSource.clip = fullClip;
        audioSource.Play();
    }

    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(pickUpClip.length);
        Destroy(gameObject);
    }
}
