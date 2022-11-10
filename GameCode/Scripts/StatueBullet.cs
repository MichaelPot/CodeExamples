using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueBullet : MonoBehaviour
{
    public float speed = 150;
    public float damage = 50;
    Vector3 oldPos = new Vector3(Mathf.Infinity,0,0);
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, 7f);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(transform.forward);
        //transform.position += transform.forward;
        RaycastHit hit;
        /// MAKE THE LAYER CHECK INSIDE SO IT GETS DESTROYED ON ANY IMPACT EXCEPT WITH OTHER ENEMY \\\
        if (Physics.Raycast(oldPos, transform.forward, out hit, (transform.position - oldPos).magnitude, ~(1 << LayerMask.NameToLayer("Enemy"))))
        {
            if (hit.collider.tag == "Player")
            {
                hit.collider.gameObject.GetComponentInChildren<Controller>().TakeDamage(damage);
                //else if (hit.collider.gameObject.layer != LayerMask.NameToLayer("Enemy"))
                Destroy(gameObject);
            }
            else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Environment"))
            {
                Destroy(gameObject);
            }
        }
    }

    private void FixedUpdate()
    {
        //transform.position += transform.forward * 5;
        //rb.AddForce(transform.forward * speed);
        rb.velocity = transform.forward * speed;
        oldPos = transform.position;
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("LOLOOLOL");
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponentInChildren<Controller>().TakeDamage(50);
        }
        Destroy(gameObject);
    }*/
}
