using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalFlyer : Enemy
{
    public GameObject bulletPos;
    public GameObject crystalBullet;
    float cooldown = 0, inBurst = 0;
    // Start is called before the first frame update
    void Start()
    {
        Init();

        if (hasChest)
            Strengthen();
    }

    // Update is called once per frame
    void Update()
    {
        healthUI.transform.LookAt(player.transform);

        // shoot another burst if off cooldown
        cooldown += Time.deltaTime;
        RaycastHit hit;
        if (cooldown >= 5 && !playerInvis && Physics.Raycast(transform.position, player.transform.position - transform.position, out hit, 1000))
        {
            StartCoroutine(ShootBurst());
        }
    }

    // shoots a burst at the player. First shot is accurate and the rest of the burst has a random spread
    IEnumerator ShootBurst()
    {
        cooldown = 0;
        int count = 0;
        bulletPos.transform.LookAt(player.transform);
        Instantiate(crystalBullet, bulletPos.transform.position, bulletPos.transform.rotation);
        yield return new WaitForSeconds(.15f);
        while (count < 3)
        {
            float x = Random.Range(player.transform.position.x - 4, player.transform.position.x + 4);
            float y = Random.Range(player.transform.position.y - 2, player.transform.position.y + 2);
            float z = Random.Range(player.transform.position.z - 4, player.transform.position.z + 4);
            bulletPos.transform.LookAt(new Vector3(x, y, z));
            GameObject b = Instantiate(crystalBullet, bulletPos.transform.position, bulletPos.transform.rotation);
            b.GetComponent<StatueBullet>().damage = damage;
            count++;
            yield return new WaitForSeconds(.15f);
        }
    }

    void Strengthen()
    {
        cooldown -= 1.5f;
    }
}
