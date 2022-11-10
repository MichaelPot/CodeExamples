using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Mutator
{
    public GameObject rings, particles, center;

    bool shot = false, landed = false;
    GameObject player;
    float cooldown = 5;
    float timer = 0;

    // ADD A LIGHT TO RINGS SO WHEN ON GROUND OR AOE MODE IT IS CLEARER -- MAYBE NOT ACTUALLY
    // NEED TO MAKE IT STACK DAMAGE OVER TIME

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        timer += Time.deltaTime;

        // shoots a fireball if it is attached to enemy and off cooldown
        if (onEnemy)
        {
            if (!shot && timer >= cooldown)
            {
                timer = 0;

                Shoot();
            }
        }
        // changes fireball appearance and enables collider to player can pick it up
        else if (onGround)
        {
            rings.SetActive(true);
            particles.SetActive(true);
            GetComponent<SphereCollider>().enabled = true;
        }
        else if (onPlayer)
        {
            // CHANGE THIS PART SO IT GETS SUMMONED ON ABILITY USE
            transform.position = transform.parent.position;
            /*if (timer >= cooldown)
            {
                timer = 0;
                Summon();
            }*/
            /*rings.SetActive(false);
            particles.SetActive(false);
            GetComponent<SphereCollider>().enabled = false;*/
        }
        // checks if the fireball hits the ground after being summoned. if it hits the ground, then change the alignment so player can see area of effect.
        else if (AllFalse() && !landed)
        {
            RaycastHit hit;

            ///// MAYBE ROTATE THE GAME OBJECT BASED ON HIT TO MAKE IT LOOK MORE NATURAL \\\\\
            if (Physics.Raycast(transform.position, -transform.up, out hit, 2f))//, LayerMask.NameToLayer("Environment")))
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Environment"))
                {
                    landed = true;

                    //center.SetActive(false);

                    GetComponent<Rigidbody>().useGravity = false;
                    GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);

                    transform.position = hit.point + Vector3.up * .25f;
                    rings.GetComponent<ParticleSystemRenderer>().alignment = ParticleSystemRenderSpace.Local;

                    Destroy(gameObject, 5);
                }
            }
        }
    }

    public override IEnumerator Activate(float duration)
    {
        float time = 0;
        
        while (time <= duration)
        {
            Summon();
            yield return new WaitForSeconds(5);
            time += 5;
        }
        yield return null;
    }

    /// <summary>
    /// Summons a fireball above a random enemy. Fireball falls down and does damage to any enemy.
    /// </summary>
    void Summon()
    {
        GameObject target = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemySpawner>().GetRandomEnemy();
        GameObject fb = Instantiate(gameObject, new Vector3(target.transform.position.x, target.transform.position.y + 10, target.transform.position.z), Quaternion.identity);

        fb.GetComponent<Fireball>().ChangeState(false, false, false);
        fb.GetComponent<Fireball>().rings.SetActive(true);
        fb.GetComponent<Fireball>().rings.transform.localScale = new Vector3(3, 3, 3);

        fb.GetComponent<Fireball>().center.transform.localScale = new Vector3(3, 3, 3);

        fb.GetComponent<Fireball>().particles.SetActive(true);
        var x = fb.GetComponent<Fireball>().particles.GetComponent<ParticleSystem>().shape;
        x.scale = new Vector3(3, 3, 3);

        fb.GetComponent<SphereCollider>().enabled = true;
        fb.GetComponent<SphereCollider>().radius = 2;

        fb.GetComponent<Rigidbody>().useGravity = true;
    }

    /// <summary>
    /// Shoots a fireball when it is attached to an enemy
    /// </summary>
    void Shoot()
    {
        GameObject ball = Instantiate(gameObject, transform.position, Quaternion.identity);
        ball.transform.LookAt(player.transform);

        ball.GetComponent<Fireball>().onEnemy = true;
        ball.GetComponent<Fireball>().shot = true;

        ball.GetComponent<SphereCollider>().enabled = true;

        ball.GetComponent<Rigidbody>().velocity = ball.transform.forward * 20;
    }

    private void OnTriggerEnter(Collider other)
    {
        // for when the ball is launched from an enemy
        if (onEnemy)
        {
            // if ball hits the player
            if (other.tag == "Player")
            {
                other.GetComponentInChildren<Controller>().TakeDamage(175);
                Destroy(gameObject);
            }
            // to make sure the ball doesn't collide with enemies or void field
            else if (other.gameObject.layer != LayerMask.NameToLayer("Enemy") && other.gameObject.layer != LayerMask.NameToLayer("Ignore Raycast"))
                Destroy(gameObject);
        }
        // for when dropped by an enemy and picked up by player
        else if (onGround && other.tag == "Player")
        {
            rings.SetActive(false);
            particles.SetActive(false);

            GetComponent<SphereCollider>().enabled = false;
            transform.parent = other.gameObject.GetComponentInChildren<Controller>().mutatorPos.transform;
            //transform.localPosition = new Vector3(1, 1, 1);
            ChangeState(false, false, true);
        }
        // for when the ball is summoned above an enemy and falls down
        else if (AllFalse())
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                other.gameObject.GetComponent<Enemy>()?.TakeDamage(175);
                other.gameObject.GetComponent<CubeBoss>()?.TakeDamage(175);
            }
        }
    }
}
