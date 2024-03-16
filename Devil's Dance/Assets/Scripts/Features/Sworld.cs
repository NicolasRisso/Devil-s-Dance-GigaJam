using UnityEngine;

public class Sworld : MonoBehaviour
{
    [SerializeField] private Material lightMaterial;

    private Material oldMaterial;

    private Win win;

    public bool isRight = false;

    private void Start()
    {
        oldMaterial = GetComponent<MeshRenderer>().material;
        win = FindObjectOfType<Win>();
    }

    public void paintMaterial(bool value)
    {
        if (value)
        {
            GetComponent<MeshRenderer>().material = lightMaterial;
            isRight = true;
        }
        else 
        {
            GetComponent<MeshRenderer>().material = oldMaterial;
            isRight = false;
        }
        win.Verify();
    }
}
