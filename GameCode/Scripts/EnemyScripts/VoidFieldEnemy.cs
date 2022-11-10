using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VoidFieldEnemy : Enemy
{
    public GameObject field, area, endPoint1, endPoint2, endPoint3, endPoint4;
    public LineRenderer line1, line2, line3, line4;
    public float size = 100, walkRadius;
    int playerLayer;
    NavMeshAgent nav;
    bool attacking = false;
    int attackTime = 5;
    // Start is called before the first frame update
    void Start()
    {
        Init();

        nav = GetComponent<NavMeshAgent>();
        nav.enabled = true;
        if (hasChest)
            Strengthen();
        StartCoroutine(Path());
        field.SetActive(true);
        line1.gameObject.SetActive(true);
        line2.gameObject.SetActive(true);
        line3.gameObject.SetActive(true);
        line4.gameObject.SetActive(true);
        //StartCoroutine(ShootField());
    }

    void Test()
    {
        field.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        healthUI.transform.LookAt(player.transform);

        line1.SetPosition(0, line1.transform.position);
        line2.SetPosition(0, line2.transform.position);
        line3.SetPosition(0, line3.transform.position);
        line4.SetPosition(0, line4.transform.position);

        line1.SetPosition(1, endPoint1.transform.position);
        line2.SetPosition(1, endPoint2.transform.position);
        line3.SetPosition(1, endPoint3.transform.position);
        line4.SetPosition(1, endPoint4.transform.position);
        ///nav.
    }

    void Strengthen()
    {
        maxHealth *= 1.5f;
        currHealth = maxHealth;
        nav.speed += 3;
    }

    IEnumerator Path()
    {
        Vector3 randomDirection = Random.insideUnitSphere * walkRadius;

        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
        Vector3 finalPosition = hit.position;

        nav.SetDestination(finalPosition);

        yield return new WaitForSeconds(4);
        transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
        StartCoroutine(ShootField());
        yield return null;
    }

    IEnumerator ShootField()
    {
        if (attacking)
        {
            yield break;
        }
        attacking = true;

        Vector3 savedScale = field.transform.localScale;
        var x = area.GetComponent<ParticleSystem>().shape;

        float ratio = 0;
        nav.enabled = false;
        while (ratio < 1)
        {
            ratio = ratio + Time.deltaTime / (float)attackTime;
            if (ratio >= 1 || ratio <= 0)
            {
                ratio = Mathf.Clamp(ratio, 0, 1);
            }
            field.transform.localScale = Vector3.Lerp(field.transform.localScale, field.transform.localScale + (Vector3.forward * size), ratio);
            x.scale = field.transform.localScale;
            /*
            line1.SetPosition(1, endPoint1.transform.position);
            line2.SetPosition(1, endPoint2.transform.position);
            line3.SetPosition(1, endPoint3.transform.position);
            line4.SetPosition(1, endPoint4.transform.position);
            */
            yield return null;
        }
        yield return new WaitForSeconds(5);
        field.transform.localScale = savedScale;
        x.scale = new Vector3(1, 1, 1);
        area.GetComponent<ParticleSystem>().Clear();
        attacking = false;
        nav.enabled = true;
        StartCoroutine(Path());
        yield return null;
    }
}
