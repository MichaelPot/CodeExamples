using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    public float fireRate = 1;
    public GameObject gun, bullet;
    public float damage;

    LineRenderer lr;
    GameObject target = null;
    float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.SetPosition(0, gun.transform.position);
        lr.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= fireRate)
        {
            timer = 0;
            Shoot();
        }
    }

    /// <summary>
    /// add a raycast to make sure target is in LOS
    /// </summary>
    void Shoot()
    {
        RaycastHit hit = new RaycastHit();
        hit.distance = 0;
        Collider[] colliders = Physics.OverlapSphere(transform.position, 25f, LayerMask.GetMask("Enemy"));
        for (int i = 0; i < colliders.Length; i++)
        {
            if (target == null && Physics.Raycast(gun.transform.position, colliders[i].transform.position - gun.transform.position,
                out hit, 1000, ~(1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("IgnoreRaycast"))))
            {
                target = colliders[i].gameObject;
                break;
            }
            else
            {
                if (GameObject.ReferenceEquals(target, colliders[i]) && Physics.Raycast(gun.transform.position, colliders[i].transform.position - gun.transform.position,
                    out hit, 1000, ~(1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("IgnoreRaycast"))))
                {
                    break;
                }
                else
                {
                    if (i == colliders.Length - 1 && Physics.Raycast(gun.transform.position, colliders[i].transform.position - gun.transform.position,
                        out hit, 1000, ~(1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("IgnoreRaycast"))))
                    {
                        target = colliders[i].gameObject;
                    }
                    else
                    {
                        target = null;
                    }
                }
            }
        }
        if (target != null)
        {
            gun.transform.LookAt(target.transform.position);
            StartCoroutine(Lazer(hit));
            /// FOR NOW USING CUBE BULLET BUT SHOULD PROBABLY CHANGE TO OWN BULLET TYPE \\\
            //GameObject b = Instantiate(bullet, gun.transform.position, gun.transform.rotation);
            //b.GetComponent<TurretBullet>().damage = damage;
        }
        /*foreach (Collider c in colliders)
        {
            c.GetComponent<Enemy>()?.TakeDamage(500);
            c.GetComponentInParent<CubeBoss>()?.TakeDamage(500);
        }*/
    }

    IEnumerator Lazer(RaycastHit hit)
    {
        lr.SetPosition(1, hit.point);
        hit.collider.gameObject.GetComponent<Enemy>()?.TakeDamage(damage);
        hit.collider.gameObject.GetComponent<CubeBoss>()?.TakeDamage(damage);
        lr.enabled = true;
        yield return new WaitForSeconds(.1f);
        lr.SetPosition(1, lr.GetPosition(0));
        lr.enabled = false;
        yield return null;
    }


    /*private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 25);
    }*/
}
