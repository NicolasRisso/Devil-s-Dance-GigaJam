using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(PlayerController), typeof(AudioSource))]
public class FootstepsSoundPlayer : MonoBehaviour
{
    [SerializeField] private TextureSound[] textureSounds;
    [SerializeField] private bool BlendTerrainSounds;

    [System.Serializable] private class TextureSound
    {
        public Texture albedo;
        public AudioClip[] clips;
    }

    private CharacterController controller;
    private PlayerController playerController;
    private AudioSource audioSource;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerController = GetComponent<PlayerController>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        StartCoroutine(CheckForGround());
    }

    private IEnumerator CheckForGround()
    {
        while (true)
        {
            if (playerController.GetIsGrounded() && controller.velocity != Vector3.zero &&
                Physics.Raycast(transform.position - new Vector3(0f, 0.5f * playerController.GetPixelArtScale() * controller.height, 0f), Vector3.down, out RaycastHit hit, 1.5f, playerController.GetFloorLayer()))
            {
                if (hit.collider.TryGetComponent<Terrain>(out Terrain terrain))
                {
                    yield return StartCoroutine(PlayFootstepSoundFromTerrain(terrain, hit.point));
                }
                else if (hit.collider.TryGetComponent<Renderer>(out Renderer renderer))
                {
                    yield return StartCoroutine(PlayFootstepSoundFromRenderer(renderer));
                }
            }
            yield return null;
        }
    }

    private IEnumerator PlayFootstepSoundFromTerrain(Terrain terrain, Vector3 hitPoint)
    {
        Vector3 terrainPosition = hitPoint - terrain.transform.position;
        Vector3 splatMapPosition = new Vector3(terrainPosition.x / terrain.terrainData.size.x, 0f, terrainPosition.z / terrain.terrainData.size.z);
        int x = Mathf.FloorToInt(splatMapPosition.x * terrain.terrainData.alphamapWidth);
        int z = Mathf.FloorToInt(splatMapPosition.z * terrain.terrainData.alphamapHeight);

        float[,,] alphaMap = terrain.terrainData.GetAlphamaps(x, z, 1, 1);

        if (!BlendTerrainSounds)
        {
            int primaryIndex = 0;
            for (int i = 1; i < alphaMap.Length; i++)
            {
                if (alphaMap[0, 0, i] > alphaMap[0, 0, primaryIndex]) primaryIndex = i;
            }
            foreach (TextureSound textureSound in textureSounds)
            {
                if (textureSound.albedo == terrain.terrainData.terrainLayers[primaryIndex].diffuseTexture)
                {
                    AudioClip clip = GetClipFromTextureSound(textureSound);
                    audioSource.pitch = playerController.GetCurrentSpeedDifference();
                    audioSource.PlayOneShot(clip);
                    yield return new WaitForSeconds(clip.length / playerController.GetCurrentSpeedDifference());
                }
            }
        }
        else
        {
            List<AudioClip> clips = new List<AudioClip>();
            int clipIndex = 0;
            for (int i = 0; i < alphaMap.Length; i++)
            {
                if (alphaMap[0, 0, i] > 0){
                    foreach(TextureSound textureSound in textureSounds)
                    {
                        if (textureSound.albedo == terrain.terrainData.terrainLayers[i].diffuseTexture)
                        {
                            AudioClip clip = GetClipFromTextureSound(textureSound);
                            audioSource.pitch = playerController.GetCurrentSpeedDifference();
                            audioSource.PlayOneShot(clip, alphaMap[0, 0, i]);
                            clips.Add(clip);
                            clipIndex++;
                            break;
                        }
                    }
                }
            }
            float longestClip = clips.Max(clips => clips.length);
            yield return new WaitForSeconds(longestClip / playerController.GetCurrentSpeedDifference());
        }
    }

    private IEnumerator PlayFootstepSoundFromRenderer(Renderer renderer)
    {
        foreach(TextureSound textureSound in textureSounds)
        {
            if (textureSound.albedo == renderer.material.GetTexture("_MainTex")){
                AudioClip clip = GetClipFromTextureSound(textureSound);
                audioSource.pitch = playerController.GetCurrentSpeedDifference();
                audioSource.PlayOneShot(clip);
                yield return new WaitForSeconds(clip.length / playerController.GetCurrentSpeedDifference());
            }
        }
    }

    private AudioClip GetClipFromTextureSound(TextureSound textureSound)
    {
        int clipIndex = Random.Range(0, textureSound.clips.Length);
        return textureSound.clips[clipIndex];
    }
}
