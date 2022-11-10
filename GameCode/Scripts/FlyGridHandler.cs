using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyGridHandler : MonoBehaviour
{
    public List<FlyingGrid> grid;
    public FlyingGrid playerPos;
    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform c in transform)
        {
            grid.Add(c.gameObject.GetComponent<FlyingGrid>());
        }
        Debug.Log(grid.Count);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdatePlayerPos(FlyingGrid other)
    {
        playerPos = other;
        Debug.Log(other.transform.position);
    }
}
