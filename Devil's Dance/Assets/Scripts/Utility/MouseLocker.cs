using UnityEngine;

public class MouseLocker : MonoBehaviour
{
    [SerializeField] private bool shouldAddMousePosition = false;

    void Awake()
    {
        if (shouldAddMousePosition)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
