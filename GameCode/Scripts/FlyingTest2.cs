using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingTest2 : MonoBehaviour
{
    public FlyGridHandler grid;
    FlyingGrid src, dest;
    List<FlyingGrid> path;
    int count = 0;
    // Start is called before the first frame update
    void Start()
    {
        path = new List<FlyingGrid>();
        src = grid.playerPos;
        //InvokeRepeating("Path", 0, 3);   
        InvokeRepeating("Navigate", 0, 2);
    }

    // Update is called once per frame
    void Update()
    {
        
        /*if (next != null)
        {
            transform.LookAt(next.transform.position);
            if (Vector3.Distance(transform.position, next.transform.position) > 5)
                transform.Translate(Vector3.forward * 10 * Time.deltaTime);
        }*/
        src = grid.playerPos;
        Path();
        //Navigate();
        if (count < path.Count && path[count] != null)
        {
            transform.LookAt(path[count].transform.position);
            if (Vector3.Distance(transform.position, path[count].transform.position) > 5)
                transform.Translate(Vector3.forward * 10 * Time.deltaTime);
            else
                count++;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "FlyingGrid")
        {
            dest = other.gameObject.GetComponent<FlyingGrid>();
            //Debug.Log("IM IN BRUH");
        }
    }

    void Navigate()
    {
        count = 0;
        path.Clear();
        FlyingGrid next = dest;
        FlyingGrid[] l= new FlyingGrid[11];
        for (int i = 0; i < 11; i++)
        {
            if (next == src)
                break;
            float min = Mathf.Infinity;
            FlyingGrid shortest = next;
            foreach (FlyingGrid n in next.neighbors)
            {
                if (n.cost <= min)
                {
                    min = n.cost;
                    shortest = n;
                }
            }
            l[i] = shortest;
            path.Add(shortest);
            next = shortest;
        }
        path.Add(next);
        for (int i = 0; i < l.Length; i++)
        {
            //Debug.Log(l[i].vertex);
        }
        /*
        float min = Mathf.Infinity;
        foreach (FlyingGrid n in dest.neighbors)
        {
            if (n.cost <= min)
            {
                min = n.cost;
                next = n;
            }
        }
        Debug.Log(next.vertex);
        //transform.position = next.transform.position;
        */
    }

    void Path()
    {
        if (src != null)
        {
            int count = 0;
            foreach (FlyingGrid g in grid.grid)
            {
                g.cost = Mathf.Infinity;
            }
            src.cost = 0;

            Queue<FlyingGrid> q = new Queue<FlyingGrid>();
            q.Enqueue(src);

            while (q.Count > 0)
            {
                FlyingGrid curr = q.Dequeue();
                foreach (FlyingGrid n in curr.neighbors)
                {
                    if (Vector3.Distance(curr.transform.position, n.transform.position) + curr.cost < n.cost)
                    {
                        n.cost = Vector3.Distance(curr.transform.position, n.transform.position) + curr.cost;
                        q.Enqueue(n);
                    }
                }
            }

            foreach (FlyingGrid g in grid.grid)
            {
                //Debug.Log(count++ + ": " + g.cost);
            }
        }
    }
}
