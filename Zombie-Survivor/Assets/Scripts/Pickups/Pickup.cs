using UnityEngine;

public class Pickup : MonoBehaviour
{
    public PickupType pickupType;
    [SerializeField] private float value;
    [SerializeField] private float timeToKeepActive;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            switch(pickupType)
            {
                case PickupType.TempHealthIncrease:
                    collision.collider.gameObject.GetComponent<PlayerController>().GetHealth(value, false);
                    break;
                case PickupType.PermanentHealthIncrease:
                    collision.collider.gameObject.GetComponent<PlayerController>().GetHealth(value, true);
                    break;
                case PickupType.TempSpeedBoost:
                    collision.collider.gameObject.GetComponent<PlayerController>().ActivateSpeedBoost(value, timeToKeepActive, false);
                    break;
                case PickupType.PermanentSpeedBoost:
                    collision.collider.gameObject.GetComponent<PlayerController>().ActivateSpeedBoost(value, timeToKeepActive, true);
                    break;
                case PickupType.Invulnerability:
                    collision.collider.gameObject.GetComponent<PlayerController>().ActivateInvulnerability(timeToKeepActive);
                    break;
                case PickupType.TempFiringSpeedBoost:   
                    collision.collider.gameObject.GetComponent<PlayerController>().ActivateFireRateBoost(value, timeToKeepActive, false);
                    break;
                case PickupType.PermanentFiringSpeedBoost:
                    collision.collider.gameObject.GetComponent<PlayerController>().ActivateFireRateBoost(value, timeToKeepActive, true);
                    break;
                case PickupType.ScoreIncrease:
                    FindAnyObjectByType<Score_Manager>().AddScore((int)value);
                    break;
                default:
                    break;
            }
            Destroy(gameObject);
        }
    }
}
