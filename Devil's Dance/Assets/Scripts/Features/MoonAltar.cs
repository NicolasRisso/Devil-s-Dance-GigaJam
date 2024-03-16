using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonAltar : MonoBehaviour
{
    private int rotationIndex = 0;
    private const int totalRotations = 8;

    public void RotateObject()
    {
        transform.Rotate(0, 45f, 0);
        rotationIndex = (rotationIndex + 1) % totalRotations;
        Debug.Log("Nova rotação: " + rotationIndex);
    }
}
