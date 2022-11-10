using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SmasherEnemy : Enemy
{
    NavMeshAgent nav;
    bool inRange = false;
    bool attacking = false, moving = false;

    public ParticleSystem ps;
    int attackTime = 1;
    float descentTime = .2f;
    int elapsed = 0;

    // Start is called before the first frame update
    void Start()
    {
        Init();

        nav = GetComponent<NavMeshAgent>();
        if (hasChest)
            Strengthen();
        if (playerInvis)
        {
            nav.enabled = false;
        }
    }

    private void FixedUpdate()
    {
        healthUI.transform.LookAt(player.transform);

        if (!moving && nav.enabled)
            nav.SetDestination(player.transform.position);

        if (Vector3.Distance(transform.position, player.transform.position) <= 10 && !playerInvis)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, player.transform.position - transform.position, out hit, 100))
            {
                if (hit.collider.tag == "Player")
                {
                    StartCoroutine(Attack(transform.position + Vector3.up * 10, transform.position));
                    inRange = false;
                }
            }
            //StartCoroutine(Attack(player.transform.position, transform.position));
            //StartCoroutine(MoveBack(transform.position));
            //Attack(player.transform.position);
            //inRange = false;
        }
    }
    IEnumerator Attack(Vector3 pos, Vector3 pos2)
    {
        /// MAKE IT DO DAMAGE IN THIS COROUTINE \\\
        if (attacking)
        {
            yield break;
        }
        attacking = true;
        moving = true;

        float ratio = 0;
        nav.enabled = false;
        while (ratio < 1)
        {
            ratio = ratio + Time.deltaTime / (float)attackTime;
            if (ratio >= 1 || ratio <= 0)
            {
                ratio = Mathf.Clamp(ratio, 0, 1);
            }
            transform.position = Vector3.Lerp(pos2, pos, ratio);
            
            yield return null;
        }

        yield return new WaitForSeconds(.3f);
        ratio = 1;
        while (ratio > 0)
        {
            ratio = ratio - Time.deltaTime / (float)descentTime;
            //Debug.Log(ratio);

            transform.position = Vector3.Lerp(pos2, pos, ratio);
            yield return null;
        }
        elapsed = 0;
        if (!playerInvis)
            nav.enabled = true;
        moving = false;
        attacking = false;
        ParticleSystem p = GameObject.Instantiate(ps, transform.position - Vector3.up, Quaternion.Euler(90,0,0));
        Destroy(p.gameObject, .5f);
        DamageCheck();
    }

    /*
    private void OnCollisionEnter(Collision collision)
    {
        attacking = false;
        if (collision.gameObject.tag == "Player")
        {
            player.GetComponentInChildren<Controller>().TakeDamage(damage);
            //Destroy(gameObject);
        }
    }*/

    private void OnTriggerEnter(Collider other)
    {
        /// PROBABLY RAYCAST TO SEE IF PLAYER IN LOS \\\
        if (other.tag == "Player")
        {
            inRange = true;
            //Debug.Log("POOOOOOOOOOP");
        }
    }

    void Strengthen()
    {
        maxHealth *= 1.5f;
        currHealth = maxHealth;
        nav.speed += 3;
    }

    void DamageCheck()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 12);
        foreach(Collider c in colliders)
        {
            if (c.tag == "Player")
            {
                c.GetComponentInChildren<Controller>().TakeDamage(250);
                break;
            }
        }
    }
}
