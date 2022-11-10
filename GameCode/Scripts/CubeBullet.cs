using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBullet : Bullet
{
    GameObject player;
    Rigidbody rb;
    bool hit;
    bool crit = false;
    Vector3 frwd;
    Vector3 oldPos = new Vector3(Mathf.Infinity, 0, 0);
    /*
    Rigidbody rb;
    Vector3 frwd, startPoint;
    */
    // Start is called before the first frame update
    void Start()
    {
        /*
        startPoint = transform.position;
        frwd = Camera.main.transform.forward;
        rb = GetComponent<Rigidbody>();
        */
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        //Physics.IgnoreCollision(player.GetComponent<Collider>(), GetComponent<Collider>());
        dir = (endPoint - startPoint).normalized;
        //frwd = bulletPos.transform.forward;
        //transform.LookAt(endPoint);
        //frwd = transform.forward;
        /*if (!rand)
        {
            
            //frwd = bulletPos.transform.forward;

            //transform.forward = bulletPos.transform.forward;
        }
        else
        {
            frwd = dir;
        }*/
        /*
        Vector3 crossed = Vector3.Cross(dir, Vector3.up);
        Vector3 crossed2 = Vector3.Cross(crossed, dir);
        Quaternion look = Quaternion.LookRotation(dir, crossed2);
        transform.Rotate(look.eulerAngles);*/
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

    // Update is called once per frame
    void Update()
    {
        //TODO: MAKE HIT REG BETTER -- ACTUALLY MIGHT  BE FIXED /////////////////////////
        //MAYBE USE SOME PREFAB THAT IS ALWAYS LOOKING AT MOUSE CURSOR /////////////////
        //transform.position += (endPoint - transform.position).normalized * speed * Time.deltaTime;
        //transform.position = Vector3.MoveTowards(transform.position, endPoint, speed * Time.deltaTime);
        /*if (Vector3.Distance(transform.position, endPoint) < .01f && !hit)
        {
            hit = true;
            Debug.Log("IM IN UPD");
            Destroy(gameObject);
        }*/
        /*
        RaycastHit hit;
        /// MAKE THE LAYER CHECK INSIDE SO IT GETS DESTROYED ON ANY IMPACT EXCEPT WITH OTHER ENEMY \\\
        if (Physics.Raycast(oldPos, transform.forward, out hit, (transform.position - oldPos).magnitude, 
            ~(1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Ignore Raycast") | 1 << LayerMask.NameToLayer("Chest"))))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                bool crit = false;
                for (int i = 0; i < luck; i++)
                {
                    int rand = Random.Range(1, 101);
                    if (rand >= 1 && rand <= critChance)
                    {
                        damage *= 2;
                        crit = true;
                        critSound.Play();
                    }
                }
                if (crit == false)
                {
                    hitSound.Play();
                }
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
        */
    }

    private void FixedUpdate()
    {
        //rb.velocity = endPoint * speed * Time.deltaTime;
        //rb.velocity = transform.forward * speed;
        //rb.velocity = frwd * speed; //* Time.deltaTime;
        /*transform.position = Vector3.MoveTowards(transform.position, endPoint, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, endPoint) < .01f && !hit)
        {
            hit = true;
            Debug.Log("IM IN UPD");
            Destroy(gameObject);
        }*/
        //rb.velocity = transform.forward * 1000 * Time.deltaTime;

        //transform.Translate(transform.forward * Time.deltaTime);
        //oldPos = transform.position;
    }

    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Enemy>() != null) 
        {
            hit = true;
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
        else if (collision.gameObject.GetComponent<CubeBoss>() != null)
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
        Destroy(gameObject);
    }
}
