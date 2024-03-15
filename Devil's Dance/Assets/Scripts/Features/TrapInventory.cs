using UnityEngine;

public class TrapInventory : MonoBehaviour
{
    bool hasTrapInInventory = false;

    public bool TryAddTrap()
    {
        switch (hasTrapInInventory)
        {
            case true:
                return false;
            default:
                hasTrapInInventory = true;
                return true;
        }
    }
}
