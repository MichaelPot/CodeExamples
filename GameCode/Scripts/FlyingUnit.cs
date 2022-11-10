using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingUnit : MonoBehaviour
{
    public bool playerInvis = false;

    public float speed = 20;
    Transform target;
    Vector3[] path;
    int targetIndex;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine("Path");
        //PathRequestManager.ReuestPath(transform.position, target.position, OnPathFound);
    }

    IEnumerator Path()
    {
        while (true)
        {
            if (!playerInvis)
                PathRequestManager.RequestPath(new PathRequest(transform.position, target.position, OnPathFound));
            yield return new WaitForSeconds(.25f);
        }
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccess)
    {
        if (pathSuccess)
        {
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        int rand = Random.Range(0, path.Length);
        Vector3 currentWaypoint = path[0] + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));

        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length - rand)
                {
                    yield break;
                }
                currentWaypoint = path[targetIndex]+ new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            yield return null;
        }
    }
}
