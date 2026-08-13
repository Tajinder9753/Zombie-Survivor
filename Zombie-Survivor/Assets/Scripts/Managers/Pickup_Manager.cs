using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup_Manager : MonoBehaviour
{
    [SerializeField] private int chanceToDropPickup = 20;
    [SerializeField] private List<Pickup> pickups;
    
    public void CheckDropPickup(Vector3 locationToDrop)
    {
        int randomNum = Random.Range(0, 100);

        if (randomNum <= chanceToDropPickup)
        {
            DropPickup(locationToDrop);
        }
    }

   private void DropPickup(Vector3 locationToDrop)
    {
        Instantiate(pickups[0], locationToDrop, Quaternion.identity);
    }

    public void RemovePickup(PickupType typeToRemove)
    {
        foreach (Pickup pickup in pickups)
        {
            if (pickup.pickupType == typeToRemove)
            {
                pickups.Remove(pickup);
            }
        }
    }
}
