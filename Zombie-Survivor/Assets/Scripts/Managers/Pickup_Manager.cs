using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup_Manager : MonoBehaviour
{
    [SerializeField] private int chanceToDropPickup = 20;
    [SerializeField] private List<Pickup> pickups;
    
    //uses a random num to check if should drop a pickup or not
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
        Pickup pickupToDrop = SelectPickup();
        Instantiate(pickups[0], locationToDrop, Quaternion.identity);
    }

    private Pickup SelectPickup()
    {
        int randomSelection = Random.Range(0, pickups.Count);
        return pickups[randomSelection];
    }

    //removes pickup from list, used for permanent pickups once the max permanent increase is hit
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
