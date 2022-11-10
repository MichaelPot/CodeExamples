using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingTest : MonoBehaviour
{
    public Transform target;
    public float speed = 5;
    public float rotationalDamp = .5f;
    public float rayOffset = 2;
    public float detectDist = 15;
   
    // Update is called once per frame
    void Update()
    {
        Pathfinding();
        Turn();
        Move();
    }

    void Turn()
    {
        Vector3 pos = target.position - transform.position;
        Quaternion rot = Quaternion.LookRotation(pos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, rotationalDamp * Time.deltaTime);
    }

    void Move()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    void Pathfinding()
    {
        RaycastHit hit;
        Vector3 rayCastOffset = Vector3.zero;

        Vector3 left = transform.position - Vector3.right * rayOffset;
        Vector3 right = transform.position + Vector3.right * rayOffset;
        Vector3 up = transform.position + Vector3.up * rayOffset;
        Vector3 down = transform.position - Vector3.up * rayOffset;

        if (Physics.Raycast(left, transform.forward, out hit, detectDist))
        {
            rayCastOffset += Vector3.right;
        }
        else if (Physics.Raycast(right, transform.forward, out hit, detectDist))
        {
            rayCastOffset -= Vector3.right;
        }
        if (Physics.Raycast(up, transform.forward, out hit, detectDist))
        {
            rayCastOffset -= Vector3.up;
        }
        else if (Physics.Raycast(down, transform.forward, out hit, detectDist))
        {
            rayCastOffset += Vector3.up;
        }

        if (rayCastOffset != Vector3.zero)
        {
            transform.Rotate(rayCastOffset * 5f * Time.deltaTime);
        }
        else
        {
            Turn();
        }
    }
}
