using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class CubeController : Controller
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
        iMan = GameObject.FindGameObjectWithTag("ItemManager").GetComponent<ItemManager>();
        ui = GameObject.FindGameObjectWithTag("MainUI").GetComponent<UIElements>();
        currHealth = maxHealth / 2;
        healthBar.value = currHealth;
        StartCoroutine(Rescale());
        //StartCoroutine(Cooldowns());
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
                TakeDamage(-40);
                Shoot(bullet);
                float a = 1 / (maxHealth) * (currHealth - maxHealth) + 2;
                a = Mathf.Clamp(a, 1f, 2);
                transform.localScale = new Vector3(a, a, a);
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
            //b.transform.rotation = bulletPos.transform.rotation;
            b.GetComponent<CubeBullet>().bulletPos = bulletPos;
            b.GetComponent<CubeBullet>().endPoint = hit.point;
            b.GetComponent<CubeBullet>().damage = tempDamage;
            b.GetComponent<CubeBullet>().hitSound = hitSound;
            b.GetComponent<CubeBullet>().critSound = critSound;
            b.GetComponent<CubeBullet>().critChance = critChance;
            b.GetComponent<CubeBullet>().luck++;
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
                b.GetComponent<CubeBullet>().endPoint = hit.point;
                b.GetComponent<CubeBullet>().damage = tempDamage;
                b.GetComponent<CubeBullet>().rand = true;
                b.GetComponent<CubeBullet>().hitSound = hitSound;
                b.GetComponent<CubeBullet>().critSound = critSound;
                b.GetComponent<CubeBullet>().critChance = critChance;
                b.GetComponent<CubeBullet>().luck++;
            }
        }
        /*
        if (iMan.items.ContainsKey("extraBul"))
        {
            
        }*/
    }

    public override void TakeDamage(float dmg)
    {
        currHealth += dmg;
        if (currHealth <= 0 || currHealth > maxHealth && alive)
        {
            gameOver.GetComponent<GameOver>().Died();
            alive = false;
        }
        //Debug.Log("OHLHOHLK");
    }

    IEnumerator Rescale()
    {
        while (true)
        {
            float a = 1 / (maxHealth) * (currHealth - maxHealth) + 2;
            a = Mathf.Clamp(a, 1, 2);
            transform.localScale = new Vector3(a, a, a);

            yield return new WaitForSeconds(1);
        }
    }
}
