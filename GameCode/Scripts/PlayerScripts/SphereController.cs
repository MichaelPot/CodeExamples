using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SphereController : Controller
{
    public GameObject bullet;
    public Slider healthBar;

    float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        gameOver = GameObject.FindGameObjectWithTag("MainUI");
        healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<Slider>();
        pm = gameObject.GetComponent<PlayerMovement>();
        pm.isSphere = true;
        iMan = GameObject.FindGameObjectWithTag("ItemManager").GetComponent<ItemManager>();
        ui = GameObject.FindGameObjectWithTag("MainUI").GetComponent<UIElements>();
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
                Shoot(bullet);
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
        //currHealth -= 50;

        RaycastHit hit;
        /// MAKE IT IGNORE CHEST \\\
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 10000,
            ~(1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Chest") | 1 << LayerMask.NameToLayer("Ignore Raycast"))))
        {
            bulletPos.transform.LookAt(hit.point);
            GameObject b = Instantiate(bullet, transform.position, Quaternion.identity);
           
            Vector3 direction = hit.point - bulletPos.transform.position;
            b.transform.forward = direction.normalized;
            b.GetComponent<Rigidbody>().AddForce(direction.normalized * shootForce, ForceMode.Impulse);

            b.GetComponent<SphereBullet>().damage = tempDamage;
            b.GetComponent<SphereBullet>().hitSound = hitSound;
            b.GetComponent<SphereBullet>().critSound = critSound;
            b.GetComponent<SphereBullet>().critChance = critChance;
            b.GetComponent<SphereBullet>().luck++;
            //b.transform.rotation = bulletPos.transform.rotation;
            /*b.GetComponent<SphereBullet>().bulletPos = bulletPos;
            b.GetComponent<SphereBullet>().endPoint = hit.point;
            b.GetComponent<SphereBullet>().damage = tempDamage;
            b.GetComponent<SphereBullet>().hitSound = hitSound;
            b.GetComponent<SphereBullet>().critSound = critSound;
            b.GetComponent<SphereBullet>().critChance = critChance;
            b.GetComponent<SphereBullet>().luck++;*/
        }
        //SOMETIMES DOESNT SHOOT MULTIPLE BULLETS ??? OR MAYBE NOT REALLY RANDOM ??? \\\
        for (int i = 0; i < numBullets; i++)
        {
            float r1 = Random.Range(-10f, 10f);
            float r2 = Random.Range(-10f, 10f);
            float r3 = Random.Range(-10f, 10f);
            Vector3 v = new Vector3(r1, r2, r3);
            if (Physics.Raycast(transform.position, v, out hit, 10000, ~(1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Chest") | 1 << LayerMask.NameToLayer("Ignore Raycast"))))//, LayerMask.GetMask(new string[] { "Bounds", "Default", "Environment" })))
            {
                GameObject b = Instantiate(bullet, transform.position, Quaternion.identity);

                Vector3 direction = hit.point - bulletPos.transform.position;
                b.transform.forward = direction.normalized;
                b.GetComponent<Rigidbody>().AddForce(direction.normalized * shootForce, ForceMode.Impulse);

                b.GetComponent<SphereBullet>().damage = tempDamage;
                b.GetComponent<SphereBullet>().rand = true;
                b.GetComponent<SphereBullet>().hitSound = hitSound;
                b.GetComponent<SphereBullet>().critSound = critSound;
                b.GetComponent<SphereBullet>().critChance = critChance;
                b.GetComponent<SphereBullet>().luck++;

                //b.GetComponent<SphereBullet>().endPoint = hit.point;              
            }
        }
    }

    public override void TakeDamage(float dmg)
    {
        currHealth -= dmg;
        if (currHealth <= 0 && alive)
        {
            gameOver.GetComponent<GameOver>().Died();
            alive = false;
        }
    }
}
