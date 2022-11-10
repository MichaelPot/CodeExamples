using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectBullet : Bullet
{
    public bool isLong = false;
    int hitCount = 0;
    GameObject player;
    Rigidbody rb;
    Vector3 frwd;
    Vector3 oldPos = new Vector3(Mathf.Infinity, 0, 0);

    bool crit = false;

    List<GameObject> hits = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        Physics.IgnoreCollision(player.GetComponent<Collider>(), GetComponent<Collider>());
        dir = (endPoint - startPoint).normalized;
        //frwd = bulletPos.transform.forward;
        //transform.LookAt(endPoint);
        //frwd = transform.forward;
        /*if (!rand)
        {
            transform.LookAt(endPoint);
            frwd = transform.forward;
            //frwd = bulletPos.transform.forward;

            //transform.forward = bulletPos.transform.forward;
        }
        else
        {
            frwd = dir;
        }*/
        for (int i = 0; i < luck; i++)
        {
            int rand = Random.Range(1, 101);
            if (rand >= 1 && rand <= critChance)
            {
                damage *= 2;
                crit = true;
            }
        }
        Destroy(gameObject, 10f);
    }

    void Update()
    {
        
        if (isLong)
        {
            int count = 0;
            RaycastHit hit;
            /// MAKE THE LAYER CHECK INSIDE SO IT GETS DESTROYED ON ANY IMPACT EXCEPT WITH OTHER ENEMY \\\
            while (Physics.Raycast(oldPos, transform.forward, out hit, (transform.position - oldPos).magnitude,
                ~(1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Ignore Raycast") | 1 << LayerMask.NameToLayer("Chest")))
                && oldPos != transform.position)
            {
                count++;
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    if (crit == false)
                    {
                        hitSound.Play();
                    }
                    else
                    {
                        critSound.Play();
                    }

                    //if (isLong)
                    //{
                        if (hit.collider.gameObject.GetComponent<Enemy>() != null && !hits.Contains(hit.collider.gameObject))
                        {
                            hitCount++;
                            hits.Add(hit.collider.gameObject);
                            hit.collider.gameObject.GetComponent<Enemy>().TakeDamage(damage);
                        }
                        else if (hit.collider.gameObject.tag == "Bounds")
                        {
                            Destroy(this.gameObject);
                            break;
                        }
                        else if (hit.collider.gameObject.GetComponentInParent<CubeBoss>() != null)
                        {
                            hit.collider.gameObject.GetComponentInParent<CubeBoss>().TakeDamage(damage);
                        }
                        if (hitCount >= 3)
                        {
                            Destroy(this.gameObject);
                            break;
                        }
                    //}
                    /*
                    else
                    {
                        if (hit.collider.gameObject.GetComponent<Enemy>() != null)
                            hit.collider.gameObject.GetComponent<Enemy>().TakeDamage(damage);
                        else if (hit.collider.gameObject.GetComponentInParent<CubeBoss>() != null)
                            hit.collider.gameObject.GetComponentInParent<CubeBoss>().TakeDamage(damage);
                        Destroy(gameObject);
                        break;
                    }*/
                }
                else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Environment") || hit.collider.gameObject.layer == LayerMask.NameToLayer("Default"))
                {
                    Destroy(gameObject);
                    break;
                }
                oldPos = hit.collider.transform.position;
            }
        }
        
    }

    /// MAKE BULLET RAYCAST SIMILAR TO STATUE BULLET \\\
    private void FixedUpdate()
    {
        //rb.velocity = frwd * speed;
        oldPos = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Enemy>() != null)
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            if (crit == false)
            {
                hitSound.Play();
            }
            else
            {
                critSound.Play();
            }
        }
        else if (collision.gameObject.GetComponentInParent<CubeBoss>() != null)
        {
            collision.gameObject.GetComponentInParent<CubeBoss>().TakeDamage(damage);
            if (crit == false)
            {
                hitSound.Play();
            }
            else
            {
                critSound.Play();
            }
        }
        Destroy(gameObject);
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("HI");
        if (other.gameObject.GetComponent<Enemy>() != null)// && !hits.Contains(other.gameObject))
        {
            hitCount++;
            other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            hits.Add(other.gameObject);

            if (crit == false)
            {
                hitSound.Play();
            }
            else
            {
                critSound.Play();
            }
        }
        else if (other.gameObject.GetComponentInParent<CubeBoss>() != null && !hits.Contains(other.gameObject))
        {
            hitCount++;
            other.gameObject.GetComponentInParent<CubeBoss>().TakeDamage(damage);
            hits.Add(other.gameObject);

            if (crit == false)
            {
                hitSound.Play();
            }
            else
            {
                critSound.Play();
            }
        }
        else if (other.gameObject.tag == "Bounds" || other.gameObject.layer == LayerMask.NameToLayer("Environment") 
            || other.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            Destroy(this.gameObject);
        }
        if (hitCount >= 3)
        {
            Destroy(this.gameObject);
        }
    }
    */

    /*
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Enemy>() != null)
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
        }
        else if (collision.gameObject.GetComponentInParent<CubeBoss>() != null)
        {
            collision.gameObject.GetComponentInParent<CubeBoss>().TakeDamage(damage);
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("HI");
        if (other.gameObject.GetComponent<Enemy>() != null)
        {
            hitCount++;
            other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
        }
        else if (other.gameObject.tag == "Bounds")
        {
            Destroy(this.gameObject);
        }
        else if (other.gameObject.GetComponentInParent<CubeBoss>() != null)
        {
            other.gameObject.GetComponentInParent<CubeBoss>().TakeDamage(damage);
        }
        if (hitCount >= 2)
        {
            Destroy(this.gameObject);
        }
    }*/
}
