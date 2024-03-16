using UnityEngine;

public class MoonAltar : MonoBehaviour
{
    public Sworld sworld;
    [SerializeField] [Range(0, 7)] private int rightRotation; 

    private int rotationIndex = 0;
    private const int totalRotations = 8;

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

    public void RotateObject()
    {
        transform.Rotate(0, 45f, 0);
        rotationIndex = (rotationIndex + 1) % totalRotations;
        paintMaterial(rotationIndex == rightRotation);
        sworld.paintMaterial(rotationIndex == rightRotation);
        Debug.Log("Nova rotação: " + rotationIndex);
    }
}
