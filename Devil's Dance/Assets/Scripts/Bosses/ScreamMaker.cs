using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class ScreamMaker : MonoBehaviour
{
    [Header("Scream Configuration")]
    [SerializeField] private List<AudioClip> audioClips;
    //0 - Chase Scream
    //1 - Trap Scream
    //2 - Patrol Noise

    private AudioSource audioSource;
    private AgentMovement agentMovement;
    private TrapActivated trapActivated;
    private Music music;

    private AgentMovement.State lastState;

    private bool playedOnce = false;
    private bool lastTrappedState = false;

    private void Awake()
    {
        music = FindObjectOfType<Music>();
        audioSource = GetComponent<AudioSource>();
        agentMovement = GetComponent<AgentMovement>();
        trapActivated = GetComponent<TrapActivated>();
        lastState = agentMovement.GetState();
        audioSource.clip = audioClips[2];
        audioSource.Play();
    }

    private void Update()
    {
        if (trapActivated.GetIsTrapped())
        {
            if (playedOnce) return;
            audioSource.clip = audioClips[1];
            audioSource.Play();
            lastTrappedState = true;
            playedOnce = true;
        }
        else if (lastTrappedState != trapActivated.GetIsTrapped())
        {
            playedOnce = false;
            lastTrappedState = false;
            if (agentMovement.GetState() == AgentMovement.State.seeking)
            {
                audioSource.clip = audioClips[0];
                audioSource.Play();
            }
        }
        else if (lastState != agentMovement.GetState())
        {
            if (agentMovement.GetState() == AgentMovement.State.chasing && lastState != agentMovement.GetState())
            {
                if (!(lastState == AgentMovement.State.seeking))
                {
                    audioSource.clip = audioClips[0];
                    audioSource.Play();
                    music.StartChaseMusic();
                }
            }
            else if (agentMovement.GetState() == AgentMovement.State.patroling && lastState != agentMovement.GetState())
            {
                audioSource.clip = audioClips[2];
                audioSource.Play();
                music.StartAmbientMusic();
            }
            lastState = agentMovement.GetState();
        }
    }
}
