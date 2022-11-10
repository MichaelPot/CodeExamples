using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDoT : MonoBehaviour
{
    private float damage = 15;
    private float elapsed = 0;

    private void Start()
    {
        damage = GetComponentInParent<Enemy>() == null ? damage : GetComponentInParent<Enemy>().damage;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            /// USE SMOOTHDELTATIME INSTEAD OF A TIMER \\\
            elapsed += Time.deltaTime;
            if (elapsed >= .1f)
            {
                elapsed = 0;
                other.gameObject.GetComponentInChildren<Controller>().TakeDamage(damage);
            }
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log(other.tag);
    }
}
