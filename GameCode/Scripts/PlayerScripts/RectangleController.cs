using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RectangleController : Controller
{
    public GameObject longBullet, tallBullet;
    public Slider healthBar;
    public GameObject parent;

    GameObject bulletOri;

    float timer = 0;
    int orientation = 0;

    // Start is called before the first frame update
    void Start()
    {
        gameOver = GameObject.FindGameObjectWithTag("MainUI");
        healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<Slider>();
        pm = gameObject.GetComponentInParent<PlayerMovement>();
        iMan = GameObject.FindGameObjectWithTag("ItemManager").GetComponent<ItemManager>();
        ui = GameObject.FindGameObjectWithTag("MainUI").GetComponent<UIElements>();
        currHealth = maxHealth;
        healthBar.value = currHealth;
        StartCoroutine(Regen());
    }

    // Update is called once per frame
    void Update()
    {
        //lightningCd += Time.deltaTime;
        timer += Time.deltaTime;
        healthBar.maxValue = maxHealth;
        if (Time.timeScale != 0)
        {
            if (timer >= fireRate && Input.GetMouseButton(0))
            {
                timer = 0;

                if (orientation == 0)
                {
                    damage = 150;
                    bulletOri = longBullet;
                }
                else
                {
                    damage = 200;
                    bulletOri = tallBullet;
                }

                Reorient();

                Shoot(bulletOri);
            }
            healthBar.value = currHealth;
        }
    }

    private void Shoot()
    {
        float tempDamage = damage;
        if (currHealth <= maxHealth * .3)
        {
            /// MAKES DAMAGE BOOST + 10% FOR EACH OF THIS ITEM \\\ 
            tempDamage = tempDamage + (damage * lowHPDmg);
        }
        if (!pm.isGrounded)
        {
            /// MAKES DAMAGE BOOST + 10% FOR EACH OF THIS ITEM \\\ 
            tempDamage = tempDamage + (damage * airDmg);
        }

        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 10000, 
            ~(1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Chest") | 1 << LayerMask.NameToLayer("Ignore Raycast"))))
        {
            bulletPos.transform.LookAt(hit.point);
            GameObject b;
            if (orientation == 1)
            {
                b = Instantiate(longBullet, bulletPos.transform.position, Quaternion.identity);
                b.GetComponent<RectBullet>().isLong = true;
            }
            else
                b = Instantiate(tallBullet, bulletPos.transform.position, Quaternion.identity);
            //b.transform.rotation = bulletPos.transform.rotation;
            b.GetComponent<RectBullet>().bulletPos = bulletPos;
            b.GetComponent<RectBullet>().endPoint = hit.point;
            b.GetComponent<RectBullet>().damage = tempDamage;
            b.GetComponent<RectBullet>().hitSound = hitSound;
            b.GetComponent<RectBullet>().critSound = critSound;
            b.GetComponent<RectBullet>().critChance = critChance;
            b.GetComponent<RectBullet>().luck++;
        }
        //SOMETIMES DOESNT SHOOT MULTIPLE BULLETS ??? \\\
        for (int i = 0; i < numBullets; i++)
        {
            float r1 = Random.Range(-10f, 10f);
            float r2 = Random.Range(-10f, 10f);
            float r3 = Random.Range(-10f, 10f);
            Vector3 v = new Vector3(r1, r2, r3);
            if (Physics.Raycast(transform.position, v, out hit, 10000, ~(1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Chest") | 1 << LayerMask.NameToLayer("Ignore Raycast"))))//, LayerMask.GetMask(new string[] { "Bounds", "Default", "Environment" })))
            {
                GameObject b;
                if (orientation == 1)
                    b = Instantiate(longBullet, bulletPos.transform.position, Quaternion.identity);
                else
                    b = Instantiate(tallBullet, bulletPos.transform.position, Quaternion.identity);
                b.GetComponent<RectBullet>().endPoint = hit.point;
                b.GetComponent<RectBullet>().damage = tempDamage;
                b.GetComponent<RectBullet>().rand = true;
                b.GetComponent<RectBullet>().hitSound = hitSound;
                b.GetComponent<RectBullet>().critSound = critSound;
                b.GetComponent<RectBullet>().critChance = critChance;
                b.GetComponent<RectBullet>().luck++;
            }
        }
    }

    void Reorient()
    {
        if (orientation == 0)
        {
            transform.Rotate(new Vector3(0, 0, -90));
            transform.position += new Vector3(0, 1, 0);
            parent.GetComponent<BoxCollider>().size = new Vector3(1f, 3f, 1f);
            parent.GetComponent<BoxCollider>().center = new Vector3(0, 1.25f, 0);
            orientation = 1;
        }
        else
        {
            transform.Rotate(new Vector3(0, 0, 90));
            transform.position += new Vector3(0, -1, 0);
            parent.GetComponent<BoxCollider>().size = new Vector3(1f, 1f, 3f);
            parent.GetComponent<BoxCollider>().center = new Vector3(0, .25f, 0);
            orientation = 0;
        }
    }
}