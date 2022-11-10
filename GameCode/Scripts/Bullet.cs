using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10;
    public Vector3 endPoint, dir, startPoint;
    public float damage, critChance;
    public GameObject bulletPos;
    public bool rand = false;
    public AudioSource critSound, hitSound;
    public int luck = 0;
}
