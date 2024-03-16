using UnityEngine;

public class Sworld : MonoBehaviour
{
    [SerializeField] private Material lightMaterial;

    private Material oldMaterial;

    private void Start()
    {
        oldMaterial = GetComponent<MeshRenderer>().material;
    }

    public void paintMaterial(bool value)
    {
        if (value) GetComponent<MeshRenderer>().material = lightMaterial;
        else GetComponent<MeshRenderer>().material = oldMaterial;
    }
}
