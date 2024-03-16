using UnityEngine;

public class AdjustRotationRelativeToParent : MonoBehaviour
{
    private Quaternion globalRotation = Quaternion.identity;

    void LateUpdate()
    {
        transform.rotation = globalRotation;
    }
}
