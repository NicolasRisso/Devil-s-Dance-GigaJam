using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class ScreamMaker : MonoBehaviour
{
    [Header("Scream Configuration")]
    [SerializeField] private List<AudioClip> audioClips;
    //0 - Chase Scream
    //1 - Trap Scream

    private AudioSource audioSource;
    private AgentMovement agentMovement;
    private TrapActivated trapActivated;

    private AgentMovement.State lastState;

    private bool playedOnce = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        agentMovement = GetComponent<AgentMovement>();
        trapActivated = GetComponent<TrapActivated>();
        lastState = agentMovement.GetState();
    }

    private void Update()
    {
        if (trapActivated.GetIsTrapped())
        {
            if (playedOnce) return;
            audioSource.clip = audioClips[1];
            audioSource.Play();
            playedOnce = true;
        }
        else if (lastState != agentMovement.GetState())
        {
            if (agentMovement.GetState() != AgentMovement.State.patroling && !audioSource.isPlaying)
            {
                audioSource.clip = audioClips[1];
                audioSource.Play();
            }
            else if (agentMovement.GetState() == AgentMovement.State.patroling && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            lastState = agentMovement.GetState();
        }
        playedOnce = false;
    }
}
