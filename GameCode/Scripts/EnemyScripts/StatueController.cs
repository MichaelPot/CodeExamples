using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// NEED TO MAKE THE AIMING BETTER FOR WHEN PLAYER IS PARTIALLY BEHIND COVER
/// </summary>
public class StatueController : Enemy
{
    //public GameObject player;
    public GameObject bullet;
    public GameObject shootDir;

    float timeFromTP = 0;
    float fireRate = 0;
    bool onScreen = false;
    bool lastSeen = false;
    bool canTP = false;

    // Start is called before the first frame update
    void Start()
    {
        Init();

        //StartCoroutine(OnOffScreen());
        transform.Translate(transform.up);
        if (hasChest)
            Strengthen();
    }

    // Update is called once per frame
    void Update()
    {
        healthUI.transform.LookAt(player.transform);

        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        if (GeometryUtility.TestPlanesAABB(planes, gameObject.GetComponent<Collider>().bounds))
        {
            canTP = true;
        }
        else
        {
            if (canTP)
            {
                RaycastHit[] hits;
                float r1 = Mathf.Clamp(Random.Range(player.transform.position.x - 50, player.transform.position.x + 50), -90, 90);
                float r2 = Mathf.Clamp(Random.Range(player.transform.position.z - 50, player.transform.position.z + 50), -90, 90);

                hits = Physics.RaycastAll(new Vector3(r1, 100, r2), -Vector3.up, 1000, LayerMask.GetMask(new string[] { "Environment" }));

                for (int i = 0; i < hits.Length; i++)
                {
                    Vector3 temp = new Vector3(hits[i].point.x, hits[i].point.y, hits[i].point.z);
                    if (Vector3.Distance(temp, player.transform.position) >= 10f && Vector3.Distance(temp, player.transform.position) <= 60f
                        && hits[i].collider.gameObject.layer != LayerMask.GetMask(new string[] { "Enemy" }))
                    {
                        transform.position = new Vector3(hits[i].point.x, hits[i].point.y + 1, hits[i].point.z);
                        timeFromTP = 0;
                    }
                }
                //transform.position = new Vector3(Camera.main.transform.position.x - 5, transform.position.y, Camera.main.transform.position.z - 5);
                canTP = false;
            }
        }
        timeFromTP += Time.deltaTime;
        fireRate += Time.deltaTime;

        if (timeFromTP > 1.5f)
        {
            RaycastHit hit;
            if (fireRate > 1.5f && !playerInvis && Physics.Raycast(transform.position, player.transform.position - transform.position, out hit, 1000, ~(1 << LayerMask.NameToLayer("Enemy"))))
            {
                if (hit.collider.tag == "Player")
                {
                    GameObject b = Instantiate(bullet, shootDir.transform.position, shootDir.transform.rotation);
                    b.GetComponent<StatueBullet>().damage = damage;
                    fireRate = 0;
                }
            }
        }
        /*
        lastSeen = onScreen;
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        */
        shootDir.transform.LookAt(player.transform.position);
    }
    /*
    void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, player.transform.position - transform.position, out hit, 1000))
        {
            if (hit.collider.tag == "Player")
               Instantiate(bullet, shootDir.transform.position, shootDir.transform.rotation);
        }
    }*/

    IEnumerator OnOffScreen()
    {
        while (true)
        {
            yield return new WaitUntil(() => onScreen == false && lastSeen == true);

            timeFromTP = 0;
            //transform.position = new Vector3(Camera.main.transform.position.x - 5, transform.position.y, Camera.main.transform.position.z - 5);
        }
    }
    void Strengthen()
    {
        maxHealth *= 1.5f;
        currHealth = maxHealth;
        fireRate -= .25f;
    }
}
