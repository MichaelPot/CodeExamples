using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public float speed = 150;
    public float damage = 50;
    public float deathTime = 10;
    bool collided = false;
    Vector3 oldPos = new Vector3(Mathf.Infinity, 0, 0);
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, deathTime);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        RaycastHit hit;
        /// MAKE THE LAYER CHECK INSIDE SO IT GETS DESTROYED ON ANY IMPACT EXCEPT WITH OTHER ENEMY \\\
        if (Physics.Raycast(oldPos, transform.forward, out hit, (transform.position - oldPos).magnitude, ~(1 << LayerMask.NameToLayer("Enemy"))))
        {
            if (hit.collider.tag == "Player")
            {
                hit.collider.gameObject.GetComponentInChildren<Controller>().TakeDamage(50);
                //else if (hit.collider.gameObject.layer != LayerMask.NameToLayer("Enemy"))
                Destroy(gameObject);
            }
        }*/
    }

    private void FixedUpdate()
    {
        oldPos = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !collided)
        {
            other.gameObject.GetComponentInChildren<Controller>().TakeDamage(damage);
            collided = true;
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && !collided)
        {
            collision.gameObject.GetComponentInChildren<Controller>().TakeDamage(damage);
            collided = true;
            Destroy(gameObject);
        }
    }
}
