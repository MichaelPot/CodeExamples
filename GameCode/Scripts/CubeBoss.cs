using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// NEED TO MAKE THE ALSER AIM BETTER FOR WHEN PLAYER IS PARTIALLY BEHIND COVER maybe
/// </summary>
public class CubeBoss : MonoBehaviour
{
    public Transform boss;
    public GameObject player, bulletPos, laserPos, lobBullet, ring, plane;
    public float health = 2000;

    int attackTime = 2;
    float descentTime = .2f;
    LineRenderer lr;
    public bool readyToAttack = true;
    float rot = 0;
    bool locked = false, turned = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        lr = GetComponent<LineRenderer>();
        StartCoroutine(PickAttack());
        //StartCoroutine(Rotate());
    }

    // Update is called once per frame
    void Update()
    {
        // turns the boss to look at the player if not locked in rotation
        if (!locked)
        {
            if (turned == false)
                transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
            bulletPos.transform.LookAt(player.transform);
        }
        //boss.transform.localRotation = Quaternion.Euler(boss.transform.localRotation.x + 90, boss.transform.localRotation.y, boss.transform.localRotation.z);
    }

    // lobs a group of cubes at the player which then linger
    IEnumerator LobAttack()
    {
        readyToAttack = false;
        StartCoroutine(Turn(Quaternion.Euler(-90, 0, 180)));
        yield return new WaitUntil(() => turned == true);
        turned = false;
        //boss.transform.localRotation = Quaternion.Euler(-90, 0, 180);
        //boss.transform.localRotation = transform.rotation;
        //boss.transform.localRotation = Quaternion.Euler(boss.transform.localRotation.x + 90, boss.transform.localRotation.y, boss.transform.localRotation.z);
        yield return new WaitForSeconds(1.5f);
        for (int i =0; i < 45; i++)
        {
            float randX = Random.Range(-5f, 5f);
            float randY = Random.Range(-5f, 5f);
            float randZ = Random.Range(0, 5f);
            GameObject b = Instantiate(lobBullet, new Vector3(transform.position.x + randX, transform.position.y + randY, transform.position.z + randZ), transform.rotation);
            randX = Random.Range(-150f, 150f);
            randY = Random.Range(500f, 800f);
            randZ = Random.Range(500f, 900f);
            b.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(randX, randY, randZ));
        }
        readyToAttack = true;
    }

    // fires out a series of rings at the player with a random spread
    IEnumerator RingAttack()
    {
        readyToAttack = false;
        StartCoroutine(Turn(Quaternion.Euler(-90, 0, 0)));
        yield return new WaitUntil(() => turned == true);
        turned = false;
        //boss.transform.localRotation = Quaternion.Euler(-90, 0, 0);
        //boss.transform.localRotation = transform.rotation;
        //boss.transform.localRotation = Quaternion.Euler(boss.transform.localRotation.x + 90, boss.transform.localRotation.y, boss.transform.localRotation.z + 180);
        yield return new WaitForSeconds(1.5f);
        for (int i = 0; i < 20; i++)
        {
            float randX = Random.Range(-2f, 2f);
            float randY = Random.Range(-10f, 10f);
            float randZ = Random.Range(0, 0);
            GameObject r = Instantiate(ring, transform.position, Quaternion.Euler(bulletPos.transform.rotation.eulerAngles.x + randX, bulletPos.transform.rotation.eulerAngles.y + randY, bulletPos.transform.rotation.eulerAngles.z + randZ));
            randX = Random.Range(2.75f, 3.5f);
            randY = Random.Range(2.75f, 3.5f);
            randZ = Random.Range(2.75f, 3.5f);
            r.transform.localScale = new Vector3(randX, randY, randZ);
            r.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0,0, 4200), ForceMode.Force);

            yield return new WaitForSeconds(.45f);
        }
        readyToAttack = true;
    }

    // shoots a laser, which does damage over time, at the player that is blocked by obstacles.
    IEnumerator LaserAttack()
    {
        readyToAttack = false;
        StartCoroutine(Turn(Quaternion.Euler(-90, 0, -90)));
        yield return new WaitUntil(() => turned == true);
        turned = false;
        //boss.transform.localRotation = Quaternion.Euler(-90, 0, -90);
        //boss.transform.localRotation = transform.rotation;
        //boss.transform.localRotation = Quaternion.Euler(boss.transform.localRotation.x + 90, boss.transform.localRotation.y, boss.transform.localRotation.z + 90);
        yield return new WaitForSeconds(1.5f);
        float timer = 0;
        while (timer < 4f)
        {
            laserPos.transform.LookAt(player.transform);
            timer += Time.deltaTime;
            RaycastHit hit;

            if (Physics.Raycast(transform.position, bulletPos.transform.forward, out hit, 1000, ~(1 << LayerMask.NameToLayer("Enemy"))))
            {
                lr.SetPosition(0, laserPos.transform.position);
                if (hit.collider.tag == "Player")
                {
                    lr.SetPosition(1, hit.collider.transform.position);
                    hit.collider.gameObject.GetComponentInChildren<Controller>().TakeDamage(175 * Time.smoothDeltaTime);
                }
                else
                    lr.SetPosition(1, hit.point);
                lr.enabled = true;
            }
            yield return null;
        }
        lr.enabled = false;
        readyToAttack = true;
    }

    // jumps and slams to ground, dealing damage and shooting out cubes
    IEnumerator SlamAttack(Vector3 pos, Vector3 pos2)
    {
        readyToAttack = false;
        float ratio = 0;
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
        DamageCheck();
        //ParticleSystem p = GameObject.Instantiate(ps, transform.position - Vector3.up, Quaternion.Euler(90, 0, 0));
        //Destroy(p.gameObject, .5f);
        
        for (int i = 0; i < 40; i++)
        {
            float randX = Random.Range(0f, 0f);
            float randY = Random.Range(0f, 360f);
            float randZ = Random.Range(0f, 0f);
            GameObject b = Instantiate(lobBullet, transform.position - Vector3.up * 6, Quaternion.Euler(bulletPos.transform.rotation.eulerAngles.x + randX, bulletPos.transform.rotation.eulerAngles.y + randY, bulletPos.transform.rotation.eulerAngles.z + randZ));
            randX = Random.Range(-150f, 150f);
            randY = Random.Range(700f, 800f);
            randZ = Random.Range(700f, 900f);
            b.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 13, 60), ForceMode.Impulse);
        }
        readyToAttack = true;
    }

    // launches planes at player in a fixed pattern and locks the boss's rotation
    IEnumerator PlaneAttack()
    {
        readyToAttack = false;
        StartCoroutine(Turn(Quaternion.Euler(-90, 0, 90)));
        yield return new WaitUntil(() => turned == true);
        turned = false;
        //boss.transform.localRotation = Quaternion.Euler(-90, 0, 90);
        yield return new WaitForSeconds(1.5f);
        locked = true;
        //boss.transform.localRotation = transform.rotation;
        boss.transform.localRotation = Quaternion.Euler(-90, 0, 90);
        for (int i = 0; i < 4; i++)
        {
            if (i == 0)
            {
                GameObject p = Instantiate(plane, transform.position, bulletPos.transform.rotation);
                p.transform.localScale = new Vector3(10, 10, .75f);
                p.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, 2000), ForceMode.Force);
            }
            else if (i == 1)
            {
                GameObject p = Instantiate(plane, transform.position, Quaternion.Euler(bulletPos.transform.rotation.eulerAngles.x, bulletPos.transform.rotation.eulerAngles.y + 12, bulletPos.transform.rotation.eulerAngles.z));
                p.transform.localScale = new Vector3(10, 10, .75f);
                p.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, 2000), ForceMode.Force);
                p = Instantiate(plane, transform.position, Quaternion.Euler(bulletPos.transform.rotation.eulerAngles.x, bulletPos.transform.rotation.eulerAngles.y - 12, bulletPos.transform.rotation.eulerAngles.z));
                p.transform.localScale = new Vector3(10, 10, .75f);
                p.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, 2000), ForceMode.Force);
                p = Instantiate(plane, transform.position, Quaternion.Euler(bulletPos.transform.rotation.eulerAngles.x, bulletPos.transform.rotation.eulerAngles.y + 20, bulletPos.transform.rotation.eulerAngles.z));
                p.transform.localScale = new Vector3(10, 10, .75f);
                p.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, 2000), ForceMode.Force);
                p = Instantiate(plane, transform.position, Quaternion.Euler(bulletPos.transform.rotation.eulerAngles.x, bulletPos.transform.rotation.eulerAngles.y - 20, bulletPos.transform.rotation.eulerAngles.z));
                p.transform.localScale = new Vector3(10, 10, .75f);
                p.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, 2000), ForceMode.Force);
            }
            else if (i == 2)
            {
                GameObject p = Instantiate(plane, transform.position, bulletPos.transform.rotation);
                p.transform.localScale = new Vector3(35, 2, 4f);
                p.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, 2000), ForceMode.Force);
            }
            else if (i == 3)
            {
                GameObject p = Instantiate(plane, transform.position, Quaternion.Euler(bulletPos.transform.rotation.eulerAngles.x, bulletPos.transform.rotation.eulerAngles.y, bulletPos.transform.rotation.eulerAngles.z));
                p.transform.localScale = new Vector3(2, 10, .75f);
                p.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, 2000), ForceMode.Force);
                p = Instantiate(plane, transform.position, Quaternion.Euler(bulletPos.transform.rotation.eulerAngles.x, bulletPos.transform.rotation.eulerAngles.y + 4, bulletPos.transform.rotation.eulerAngles.z));
                p.transform.localScale = new Vector3(2, 10, .75f);
                p.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, 2000), ForceMode.Force);
                p = Instantiate(plane, transform.position, Quaternion.Euler(bulletPos.transform.rotation.eulerAngles.x, bulletPos.transform.rotation.eulerAngles.y + 8, bulletPos.transform.rotation.eulerAngles.z));
                p.transform.localScale = new Vector3(2, 10, .75f);
                p.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, 2000), ForceMode.Force);
                p = Instantiate(plane, transform.position, Quaternion.Euler(bulletPos.transform.rotation.eulerAngles.x, bulletPos.transform.rotation.eulerAngles.y - 4, bulletPos.transform.rotation.eulerAngles.z));
                p.transform.localScale = new Vector3(2, 10, .75f);
                p.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, 2000), ForceMode.Force);
                p = Instantiate(plane, transform.position, Quaternion.Euler(bulletPos.transform.rotation.eulerAngles.x, bulletPos.transform.rotation.eulerAngles.y - 8, bulletPos.transform.rotation.eulerAngles.z));
                p.transform.localScale = new Vector3(2, 10, .75f);
                p.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, 2000), ForceMode.Force);
                p = Instantiate(plane, transform.position, Quaternion.Euler(bulletPos.transform.rotation.eulerAngles.x, bulletPos.transform.rotation.eulerAngles.y + 26, bulletPos.transform.rotation.eulerAngles.z));
                p.transform.localScale = new Vector3(12, 10, .75f);
                p.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, 2500), ForceMode.Force);
                p = Instantiate(plane, transform.position, Quaternion.Euler(bulletPos.transform.rotation.eulerAngles.x, bulletPos.transform.rotation.eulerAngles.y - 26, bulletPos.transform.rotation.eulerAngles.z));
                p.transform.localScale = new Vector3(12, 10, .75f);
                p.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, 2500), ForceMode.Force);
            }
            yield return new WaitForSeconds(.4f);
        }
        locked = false;
        readyToAttack = true;
    }

    // picks which attack to use against the player. does the slam attack if player is in range, otherwise picks randomly from other 4 attacks.
    IEnumerator PickAttack()
    {
        while (true)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < 28)
            {
                StartCoroutine(SlamAttack(transform.position + Vector3.up * 10, transform.position));
            }
            else
            {
                int rand = Random.Range(0, 4);
                if (rand == 0)
                {
                    StartCoroutine(LobAttack());
                }
                else if (rand == 1)
                {
                    StartCoroutine(RingAttack());
                }
                else if (rand == 2)
                {
                    StartCoroutine(LaserAttack());
                }
                else if (rand == 3)
                {
                    StartCoroutine(PlaneAttack());
                }
            }
            yield return new WaitUntil(() => readyToAttack == true);
        }
    }

    // check if player should take damage for ground slam attack
    void DamageCheck()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 28);
        foreach (Collider c in colliders)
        {
            if (c.tag == "Player")
            {
                c.GetComponentInChildren<Controller>().TakeDamage(400);
                break;
            }
        }
    }

    public void TakeDamage(float dmg)
    {
        health -= dmg;
        if (health <= 0)
            Destroy(gameObject);
    }

    // turns the boss to look at the player with the proper face facing them
    IEnumerator Turn(Quaternion target)
    {
        float ratio = 0;
        while (ratio < 1)
        {
            ratio = ratio + Time.deltaTime;
            if (ratio >= 1 || ratio <= 0)
            {
                ratio = Mathf.Clamp(ratio, 0, 1);
            }
            boss.transform.localRotation = Quaternion.Lerp(boss.transform.localRotation, target, ratio);
            yield return null;
        }

        turned = true;
        yield return null;
    }
}
