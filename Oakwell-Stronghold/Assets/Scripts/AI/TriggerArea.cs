#region 'Using' information
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

public class TriggerArea : MonoBehaviour
{
    private Enemy enemyParent;

    private void Awake()
    { enemyParent = GetComponentInParent<Enemy>(); }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            enemyParent.target = collider.transform;
            enemyParent.inRange = true;
            enemyParent.aggroZone.SetActive(true);
        }
    }
}
