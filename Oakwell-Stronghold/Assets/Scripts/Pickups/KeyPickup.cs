using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    bool holdingKey;

    private void OnTriggerEnter2D(Collider2D collision) // when ya touch the key
    {
        if (!holdingKey) // Only pick up a key if you're not holding one.
        {
            holdingKey = true;
            Destroy(gameObject); // Removes the key
        }
    }
}
