#region 'Using' information
using UnityEngine;
using Player;
#endregion

public class HealthPickup : MonoBehaviour
{
    PlayerHealth playerHealth;
    public AudioSource ding;

    private void Awake()
    { playerHealth = FindObjectOfType<PlayerHealth>(); }

    private void OnTriggerEnter2D(Collider2D collision) // when ya touch the pickup
    {
        if(playerHealth.currentHealth < playerHealth.maxHealth) // and if ya need to heal...
        {
            ding.Play();
            RestoreHealth(1);
            Destroy(gameObject); // Removes the pickup
        }
    }

    void RestoreHealth(int health)
    {
        playerHealth.currentHealth += health;
        playerHealth.healthBar.SetHealth(playerHealth.currentHealth);
    }
}
