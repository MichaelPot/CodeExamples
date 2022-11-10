using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> enemyTypes;
    public List<GameObject> mutations;
    //public GameObject statue;
    public GameObject player;
    public int totalEnemies = 1;
    public int chestChance = 5;
    public bool bossFight = false;
    
    float healthMult = 1, dmgMult = 1;
    int count = 0, maxTries = 5;
    List<GameObject> enemies = new List<GameObject>();
    bool playerInvis = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        InvokeRepeating("Spawn", 0, 1);
        /*
        for (int i = 0; i < 5; i++)
        {
            RaycastHit hit;
            float r1 = Random.Range(-50, 50);
            float r2 = Random.Range(-50, 50);

            if (Physics.Raycast(new Vector3(r1, transform.position.y, r2), -transform.up, out hit, 1000, LayerMask.GetMask(new string[] { "Environment" })))
            {
                Debug.Log("HHIII");
                Instantiate(statue, hit.point, Quaternion.identity);
            }
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Spawn()
    {
        // pick which enemy to spawn
        int enemyIndex = Random.Range(0, enemyTypes.Count);
        GameObject eType = enemyTypes[enemyIndex];

        // chance a chest will drop
        int dropChest = Random.Range(1, chestChance);

        // chance enemy spawns with mutation 
        int mutateChance = Random.Range(1, 21); //51 is max


        int tries = 0;
        while (tries < maxTries)
        {
            //for first test map it -80 to 80 for both
            //for second test map:
            //x = 22 to 180
            //z = 21 to 190
            RaycastHit[] hits;
            float r1 = Random.Range(-80, 80);
            float r2 = Random.Range(-80, 80);

            hits = Physics.RaycastAll(new Vector3(r1, transform.position.y, r2), -transform.up, 1000, LayerMask.GetMask(new string[] { "Environment" }));

            for (int i = 0; i < hits.Length; i++)
            {
                Vector3 temp = new Vector3(hits[i].point.x, hits[i].point.y, hits[i].point.z);
                if (Vector3.Distance(temp, player.transform.position) >= 10f && Vector3.Distance(temp, player.transform.position) <= 70f && count < totalEnemies)
                {
                    count++;
                    GameObject e;
                    if (enemyIndex == 3) // Crystal enemy
                    {
                        e = Instantiate(eType, hits[i].point + Vector3.up * 10, Quaternion.identity);
                    }
                    else
                    {
                        e = Instantiate(eType, hits[i].point, Quaternion.identity);
                    }
                    if (dropChest == 1 && !bossFight)
                    {
                        e.GetComponent<Enemy>().hasChest = true;
                        Outline outline = e.AddComponent<Outline>();

                        outline.OutlineMode = Outline.Mode.OutlineVisible;
                        outline.OutlineColor = Color.yellow;
                        outline.OutlineWidth = 3f;
                        //chestChance = chestChance * 3;
                    }
                    ////////// THIS ISNT OUTLINED BECAUSE IT IS ADDED AFTER THE OUTLINE IS -- USE THIS FOR OTHER THINGS \\\\\\\\\
                    if (mutateChance == 1)
                    {
                        int mutation = Random.Range(0, mutations.Count);
                        GameObject m = Instantiate(mutations[mutation], e.GetComponent<Enemy>().mutationLocation.transform);
                        m.GetComponent<Mutator>().ChangeState(true, false, false);
                        e.GetComponent<Enemy>().mutation = m;
                    }

                    e.GetComponent<Enemy>().LevelUp(healthMult, dmgMult);
                    enemies.Add(e);
                    return;
                }
            }
            tries++;
        }
    }

    // disables enemies while player is invisible
    public void PlayerInvisible(float time)
    {
        foreach (GameObject e in enemies)
        {
            e.GetComponent<Enemy>().PauseActions(time);
        }
    }

    public void DecreaseCount(GameObject g)
    {
        enemies.Remove(g);
        count--;
    }

    public void Clear()
    {
        foreach(GameObject e in enemies)
        {
            e.GetComponent<Enemy>().forcedDeath = true;
            Destroy(e);
        }
        enemies.Clear();
        count = 0;
    }

    public void LevelUp()
    {
        dmgMult += .3f;
        healthMult += .25f;
    }

    public GameObject GetRandomEnemy()
    {
        return enemies[Random.Range(0, enemies.Count)];
    }
}
