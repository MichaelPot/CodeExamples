using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class JabberEnemy : Enemy
{
    //GameObject player;
    NavMeshAgent nav;
    bool inRange = false;
    bool attacking = false, moving = false;

    float cooldown = 0;
    int attackTime = 1;
    float timer = 0;

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
        cooldown += Time.fixedDeltaTime;
        timer += Time.fixedDeltaTime;

        healthUI.transform.LookAt(player.transform);

        if (nav.enabled)
            nav.SetDestination(player.transform.position);

        if (cooldown >= 1.5f && !playerInvis && Vector3.Distance(transform.position, player.transform.position) <= 15)
        {
            cooldown = 0;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, player.transform.position - transform.position, out hit, 100))
            {
                if (hit.collider.tag == "Player")
                {
                    StartCoroutine(Attack(player.transform.position, transform.position));
                    inRange = false;
                    nav.enabled = false;
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
        yield return new WaitForSeconds(.25f);

        float ratio = 0;
        //nav.isStopped = true;
        while (ratio < 1 && attacking)
        {
            ratio = ratio + Time.deltaTime / (float)attackTime;
            if (ratio >= 1 || ratio <= 0)
            {
                ratio = Mathf.Clamp(ratio, 0, 1);
            }
            transform.position = Vector3.Lerp(transform.position, pos, ratio);
            yield return null;
        }
        
        ratio = 0;
        while (ratio <= 1)
        {
            ratio = ratio + Time.deltaTime / (float)attackTime;
            //Debug.Log(ratio);
            
            transform.position = Vector3.Lerp(transform.position, pos2, ratio);
            yield return null;
        }
        if (!playerInvis)
            nav.enabled = true;
        attacking = false;
        //transform.position = pos2;
        //Debug.Log("HI THERE");
    }

    IEnumerator MoveBack(Vector3 pos)
    {
        if (moving)
        {
            yield break;
        }
        moving = true;
        yield return new WaitUntil(() => attacking == false);

        float ratio = 0;
        nav.isStopped = true;
        while (attacking)
        {
            ratio = ratio + Time.deltaTime / (float)attackTime;
            //Debug.Log(ratio);
            if (ratio >= 1 || ratio <= 0)
            {
                ratio = Mathf.Clamp(ratio, 0, 1);
                attacking = false;
            }
            transform.position = Vector3.Lerp(transform.position, pos, ratio);
            yield return null;
        }
        nav.isStopped = false;
        moving = false;
        //Debug.Log("HI THERE");
    }

    private void OnCollisionEnter(Collision collision)
    {
        attacking = false;
        if (collision.gameObject.tag == "Player" && (timer >= 1.5f || attacking))
        {
            timer = 0;
            player.GetComponentInChildren<Controller>().TakeDamage(damage);
            //Destroy(gameObject);
        }
    }

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
}
