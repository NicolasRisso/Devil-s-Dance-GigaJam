using UnityEngine;

public class TrapInventory : MonoBehaviour
{
    [SerializeField] private GameObject trapGO;
    [SerializeField] private float yAdjust;

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

    public bool PlaceTrap()
    {
        if (trapGO is null) return false;
        if (hasTrapInInventory)
        {
            Vector3 playerPosition = transform.position;
            Vector3 spawnPosition = new Vector3(playerPosition.x, playerPosition.y + yAdjust, playerPosition.z);
            Instantiate(trapGO, spawnPosition, trapGO.transform.rotation);
            hasTrapInInventory = false;
            return true;
        }
        return false;
    }
}
