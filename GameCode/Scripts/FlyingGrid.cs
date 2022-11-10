using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingGrid : MonoBehaviour
{
    public List<FlyingGrid> neighbors;
    public Vector3 pos;
    public float height, width;
    public float cost;
    public int vertex;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("PrintCost", 0, 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GetComponentInParent<FlyGridHandler>().UpdatePlayerPos(this);
            Debug.Log("HEY THER EPAL");
        }
    }

    void PrintCost()
    {
        Debug.Log("VERTEX: " + vertex + "COST: " + cost);
    }

    /*void UpdateCost(float updated)
    {
        cost = updated;
        foreach (FlyingGrid n in neighbors)
        {
            n.UpdateCost(cost + 1);
        }
    }*/
}
