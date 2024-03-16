using UnityEngine;
using UnityEngine.SceneManagement;

public class KillPlayer : MonoBehaviour
{
    private Transform player;
    public float detectionRadius = 1.5f;

    private void Start()
    {
        player = player = GameObject.Find("Player").transform;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= detectionRadius)
        {
            SceneManager.LoadScene("DeathScene");
        }
    }
}
