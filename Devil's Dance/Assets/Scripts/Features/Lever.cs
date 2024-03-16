using System.Collections;
using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] private MoonAltar moon;

    private Animator animator;
    private AudioSource audioSource;

    private bool inUse = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void RotateMoon()
    {
        if (moon is { } && !inUse)
        {
            moon.RotateObject();
            animator.SetBool("Interacting", true);
            audioSource.Play();
            StartCoroutine(EndAnim());
            inUse = true;
        }
    }

    private IEnumerator EndAnim()
    {
        yield return new WaitForSeconds(1.1f);
        animator.SetBool("Interacting", false);
        inUse = false;
    }
}
