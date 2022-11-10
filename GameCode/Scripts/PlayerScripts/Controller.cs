using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.UI;

/*
 * Parent class for controller scripts for different player classes
 */
public class Controller : MonoBehaviour
{
    public float maxHealth, currHealth;
    public float damage;
    public float regen;
    public int numBullets;
    public int luck;
    public float fireRate;
    public float critChance = 5;
    public ItemManager iMan;
    public float healAmount = 0;
    public VisualEffect lightning;
    public UIElements ui;
    public AudioSource critSound, hitSound;
    public float airDmg, lowHPDmg;
    public GameObject turret;
    public GameObject mutatorPos;
    public float shootForce;
    public GameObject bulletPos;

    protected float lightningCd = 18, dashCd = 10;
    protected GameObject gameOver;
    protected bool alive = true;
    protected PlayerMovement pm;
    protected float damageMult = 1;

    string currAbility = "";

    // NEED TO MAKE A VARIABLE FOR LIGHTNING AND TURRET DAMAGE SO IT ISNT DEPENDANT ON PLAYER DAMAGE \\

    /// <summary>
    /// Makes the player take damage
    /// </summary>
    /// <param name="dmg"> amount of health to lose </param>
    public virtual void TakeDamage(float dmg)
    {
        currHealth -= dmg;
        if (currHealth <= 0 && alive)
        {
            gameOver.GetComponent<GameOver>().Died();
            alive = false;
        }
    }

    /// <summary>
    /// Heals the player on enemy kill
    /// </summary>
    public virtual void Heal()
    {
        if (iMan.items.ContainsKey("Aenodyr's Fossil"))
        {
            currHealth = Mathf.Clamp(currHealth + healAmount, 0, maxHealth);
        }
    }

    /// <summary>
    /// Scales the player health and damage up when GameManager tells it
    /// </summary>
    public void LevelUp()
    {
        maxHealth = maxHealth + maxHealth * .25f;
        damage = damage + damage * .3f;
        damageMult += .3f;
    }

    /// <summary>
    /// changes the ability item that the player has
    /// </summary>
    /// <param name="name"> The new item that the player picked up </param>
    public void ChangeAbility(string name)
    {
        // clears the ability item if the player switches them out
        if (gameObject.GetComponent<AbilityItem>() != null && currAbility != name)
            Destroy(gameObject.GetComponent<AbilityItem>());

        // adds the correct ability to the player and initializes it
        if (currAbility != name)
        {
            if (name == "lightning")
            {
                currAbility = name;
                gameObject.AddComponent<LightningAbility>();
                gameObject.GetComponent<LightningAbility>().lightning = lightning;
                gameObject.GetComponent<LightningAbility>().ui = ui;
                gameObject.GetComponent<LightningAbility>().damage = 500 * damageMult;
                gameObject.GetComponent<LightningAbility>().mutatorPos = mutatorPos;
            }
            else if (name == "dash")
            {
                currAbility = name;
                gameObject.AddComponent<DashAbility>();
                gameObject.GetComponent<DashAbility>().pm = pm;
                gameObject.GetComponent<DashAbility>().ui = ui;
                gameObject.GetComponent<DashAbility>().mutatorPos = mutatorPos;
            }
            else if (name == "invisible")
            {
                currAbility = name;
                gameObject.AddComponent<InvisibleAbility>();
                gameObject.GetComponent<InvisibleAbility>().ui = ui;
                gameObject.GetComponent<InvisibleAbility>().mutatorPos = mutatorPos;
            }
            else if (name == "turret")
            {
                currAbility = name;
                gameObject.AddComponent<TurretAbility>();
                gameObject.GetComponent<TurretAbility>().ui = ui;
                gameObject.GetComponent<TurretAbility>().turret = turret;
                gameObject.GetComponent<TurretAbility>().damage = 75 * damageMult;
                gameObject.GetComponent<TurretAbility>().mutatorPos = mutatorPos;
            }
        }
    }

    protected void Shoot(GameObject bulletType)
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
            GameObject b = Instantiate(bulletType, transform.position, Quaternion.identity);

            Vector3 direction = hit.point - bulletPos.transform.position;
            b.transform.forward = direction.normalized;
            b.GetComponent<Rigidbody>().AddForce(direction.normalized * shootForce, ForceMode.Impulse);

            b.GetComponent<Bullet>().damage = tempDamage;
            b.GetComponent<Bullet>().hitSound = hitSound;
            b.GetComponent<Bullet>().critSound = critSound;
            b.GetComponent<Bullet>().critChance = critChance;
            b.GetComponent<Bullet>().luck++;
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
                GameObject b = Instantiate(bulletType, transform.position, Quaternion.identity);

                Vector3 direction = hit.point - bulletPos.transform.position;
                b.transform.forward = direction.normalized;
                b.GetComponent<Rigidbody>().AddForce(direction.normalized * shootForce, ForceMode.Impulse);

                b.GetComponent<Bullet>().damage = tempDamage;
                b.GetComponent<Bullet>().rand = true;
                b.GetComponent<Bullet>().hitSound = hitSound;
                b.GetComponent<Bullet>().critSound = critSound;
                b.GetComponent<Bullet>().critChance = critChance;
                b.GetComponent<Bullet>().luck++;
            }
        }
    }

    /*
    /// <summary>
    /// MAKE ABILITY ITEMS EMPTY OBJECT AS CHILD ON PLAYER AND THEY HAVE A SCRIPT THAT HANDLES ACTIVATING ABILITIES
    /// </summary>

    // dash handled in player movement
    public void Ability()
    {
        if (Input.GetKeyDown("q"))
        {
            if (hasLightning && lightningCd >= 18)
            {
                lightningCd = 0;
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 10000, ~(1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Chest"))))
                {
                    VisualEffect v = GameObject.Instantiate(lightning, hit.point + (Vector3.up * 28), Quaternion.identity);
                    StartCoroutine(LightningDmg(hit));
                    GameObject.Destroy(v.gameObject, 1.5f);
                }
            }
            else if (hasDash && dashCd >= 10)
            {
                dashCd = 0;
                StartCoroutine(pm.Dash());
            }
            else if (hasInvis)
            {

            }
        }
    }

    IEnumerator LightningDmg(RaycastHit hit)
    {
        /// MAYBE JUST MAKE IT INSTANT ??? BUT THEN IT WONT LINE UP WITH WHEN LIGHTNING HITS GROUND \\\
        yield return new WaitForSeconds(.15f);
        Collider[] colliders = Physics.OverlapSphere(hit.point, 5f, LayerMask.GetMask("Enemy"));
        foreach (Collider c in colliders)
        {
            c.GetComponent<Enemy>()?.TakeDamage(500);
            c.GetComponentInParent<CubeBoss>()?.TakeDamage(500);
        }
        yield return null;
    }
    
    protected IEnumerator Cooldowns()
    {
        while (true)
        {
            lightningCd += Time.deltaTime;
            dashCd += Time.deltaTime;
            if (AbilityReady())
            {
                ui.readyColor.GetComponent<Image>().color = new Color(1, 1, 1);
            }
            else
            {
                ui.readyColor.GetComponent<Image>().color = new Color(0, 0, 0);
            }
            yield return null;
        }
    }

    public bool AbilityReady()
    {
        if (hasAbility)
        {
            if (hasLightning && lightningCd >= 18)
            {
                return true;
            }
            else if (hasDash && dashCd >= 10)
            {
                return true;
            }
        }
        return false;
    }
    */

    /// <summary>
    /// Gives the player health regen
    /// </summary>
    /// <returns></returns>
    protected IEnumerator Regen()
    {
        while (true)
        {
            yield return new WaitForSeconds(.5f);
            if (currHealth < maxHealth - regen)
            {
                currHealth += regen;
            }
        }
    }
    /*
    protected void ChestCheck()
    {
        Collider[] c = Physics.OverlapSphere(transform.position, 3, LayerMask.GetMask("Chest"));
        for (int i = 0; i < c.Length; i++)
        {
            c[i].gameObject.GetComponent<ChestController>().
        }
    }*/
}
