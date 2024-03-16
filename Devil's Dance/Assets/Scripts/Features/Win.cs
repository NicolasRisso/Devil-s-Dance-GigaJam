using UnityEngine;
using UnityEngine.SceneManagement;

public class Win : MonoBehaviour
{
    private Sworld[] sworlds;

    private void Start()
    {
        sworlds = FindObjectsOfType<Sworld>();
    }

    public void Verify()
    {
        foreach (Sworld sworld in sworlds)
        {
            if (sworld.isRight == false) return;
        }
        SceneManager.LoadScene("Win");
    }
}
