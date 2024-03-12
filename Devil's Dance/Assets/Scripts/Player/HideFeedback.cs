using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class HideFeedback : MonoBehaviour
{
    [Header("Time Definition")]
    [SerializeField] private float animDurationHide;
    [SerializeField] private float animDurationLeave;
    [Header("Vignette Configuration")]
    [SerializeField] [Range(0f, 1f)] private float endIntensity;
    [Header("Audio Muffer Configuration")]
    [SerializeField] private int normalFilter;
    [SerializeField] private int muffedFilter;

    private Vignette vignette;
    private PostProcessVolume postProcessVolume;

    private AudioLowPassFilter audioFilter;

    private Transform parent;

    private void Awake()
    {
        parent = transform.parent.GetComponent<Transform>();
        audioFilter = transform.parent.GetComponent<AudioLowPassFilter>();
        postProcessVolume = GetComponent<PostProcessVolume>();
        if (postProcessVolume.profile.TryGetSettings(out Vignette vignette)) this.vignette = vignette;
    }

    private void Update()
    {
        Debug.Log(parent.tag);
        if (parent.CompareTag("Hidden"))
        {
            if (vignette.intensity.value <= endIntensity) vignette.intensity.value += (endIntensity / animDurationHide) * Time.deltaTime;
            if (audioFilter.cutoffFrequency > muffedFilter) audioFilter.cutoffFrequency -= ((normalFilter - muffedFilter) / animDurationHide) * Time.deltaTime;
        }
        else
        {
            if (vignette.intensity.value > 0f) vignette.intensity.value -= (endIntensity / animDurationLeave) * Time.deltaTime;
            if (audioFilter.cutoffFrequency < normalFilter) audioFilter.cutoffFrequency += ((normalFilter - muffedFilter) / animDurationHide) * Time.deltaTime;
        }
    }

}
