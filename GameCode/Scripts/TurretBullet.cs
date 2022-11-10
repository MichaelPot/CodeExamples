using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBullet : Bullet
{
    Rigidbody rb;
    Vector3 frwd;
    Vector3 oldPos = new Vector3(Mathf.Infinity, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        frwd = transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        /// MAKE THE LAYER CHECK INSIDE SO IT GETS DESTROYED ON ANY IMPACT EXCEPT WITH OTHER ENEMY \\\
        if (Physics.Raycast(oldPos, transform.forward, out hit, (transform.position - oldPos).magnitude, ~(1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("IgnoreRaycast"))))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                if (hit.collider.gameObject.GetComponent<Enemy>() != null)
                {
                    hit.collider.gameObject.GetComponent<Enemy>().TakeDamage(damage);
                }
                else if (hit.collider.gameObject.GetComponentInParent<CubeBoss>() != null)
                {
                    hit.collider.gameObject.GetComponentInParent<CubeBoss>().TakeDamage(damage);
                }
                Destroy(this.gameObject);
            }
            else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Environment"))
            {
                Destroy(gameObject);
            }
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = frwd * speed; 

        oldPos = transform.position;
    }
}
