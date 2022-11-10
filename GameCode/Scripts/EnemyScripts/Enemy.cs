using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public GameObject player;
    public bool hasChest = false;
    public float maxHealth, currHealth;
    public GameObject chest;
    public float damage;
    public bool forcedDeath = false;
    public GameObject mutation, mutationLocation;
    
    public GameObject healthUI;
    public Slider slider;
    
    static protected bool playerInvis;
    protected UIElements ui;
    protected GameObject spawner;

    protected void Init()
    {
        spawner = GameObject.FindGameObjectWithTag("EnemySpawner");
        player = GameObject.FindGameObjectWithTag("Player");
        ui = GameObject.FindGameObjectWithTag("MainUI").GetComponent<UIElements>();
        currHealth = maxHealth;

        CalcHealth();
    }
    // makes the enemy take damage and update the spawner, amount of money the player has, and drop a chest if it has one
    public void TakeDamage(float damage)
    {
        healthUI.SetActive(true);

        currHealth -= damage;
        if (currHealth <= 0)
        {
            player.GetComponentInChildren<Controller>().Heal();
            if (!forcedDeath) // forcedDeath is when enemies are cleared at start of boss fight
            {
                if (hasChest)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, Vector3.down, out hit, 50, LayerMask.GetMask("Environment")))
                        Instantiate(chest, hit.point + (Vector3.up * .5f), Quaternion.LookRotation(Vector3.forward, hit.normal));
                    ui.moneyAmount += Random.Range(13, 18);
                }
                else
                {
                    ui.moneyAmount += Random.Range(10, 15);
                }

                /// MAKE SURE NOT STILL REFERENCING AFTER BEING DESTROYED \\\
                if (mutation != null)
                {
                    mutation.transform.parent = null;
                    mutation.GetComponent<Mutator>().ChangeState(false, true, false);
                }
                spawner.GetComponent<EnemySpawner>().DecreaseCount(this.gameObject);
            }
            Destroy(gameObject);
        }

        CalcHealth();
    }

    // levels up the enemy based on how long the player has been alive
    public void LevelUp(float hm, float dm)
    {
        maxHealth *= hm;
        currHealth = maxHealth;
        damage *= dm;
    }

    /// <summary>
    /// MAYBE HAVE STATIC TIME VARIABLE AND DECREMENT THAT IN ENEMYSPAWNER OR SOME OTHER METHOD SO THAT THIS WORKS WITH STACKS OF INVISIBILITY
    /// </summary>
    /// <param name="time"></param>
    public void PauseActions(float time)
    {
        playerInvis = true;
        if (gameObject.GetComponent<NavMeshAgent>() != null)
        {
            gameObject.GetComponent<NavMeshAgent>().enabled = false;
        }
        else if (gameObject.GetComponent<FlyingUnit>() != null)
        {
            gameObject.GetComponent<FlyingUnit>().playerInvis = playerInvis;
        }
        Invoke("UnpauseActions", time);
    }

    public void UnpauseActions()
    {
        playerInvis = false;
        if (gameObject.GetComponent<NavMeshAgent>() != null)
        {
            gameObject.GetComponent<NavMeshAgent>().enabled = true;
        }
        else if (gameObject.GetComponent<FlyingUnit>() != null)
        {
            gameObject.GetComponent<FlyingUnit>().playerInvis = playerInvis;
        }
    }

    protected void CalcHealth()
    {
        slider.value = currHealth / maxHealth;
    }
    /*
    private void OnDestroy()
    {
        if (!forcedDeath)
        {
            if (hasChest)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, Vector3.down, out hit, 50, LayerMask.GetMask("Environment")))
                    Instantiate(chest, hit.point + (Vector3.up * .5f), Quaternion.LookRotation(Vector3.forward, hit.normal));
                ui.moneyAmount += Random.Range(13, 18);
            }
            else
            {
                ui.moneyAmount += Random.Range(10, 15);
            }
            spawner.GetComponent<EnemySpawner>().DecreaseCount(this.gameObject);
        }
    }
    */
}
