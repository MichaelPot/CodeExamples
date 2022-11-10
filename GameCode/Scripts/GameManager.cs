using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject[] characters;
    public GameObject abilityChest;
    public Slider x, y;

    public static GameManager instance = null;

    public float xSens = 500, ySens = 5;
    public bool accel = true;
    int index = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
    }

    /// <summary>
    /// Sets up the game when a new level is loaded after main menu
    /// </summary>
    /// <param name="level"></param>
    private void OnLevelWasLoaded(int level)
    {
        if (SceneManager.GetActiveScene().buildIndex > 0)
        {
            // spawn chosen character
            GameObject.Instantiate(characters[index], new Vector3(0, 1, 0), Quaternion.identity);

            // spawn an ability chest randomly -- NEED TO FIX SO IT DOESNT SPAWN IN TERRAIN
            RaycastHit hit;
            bool notDone = true;
            while (notDone)
            {
                float r1 = Random.Range(-85, 80);
                float r2 = Random.Range(-83, 84);
                if (Physics.Raycast(new Vector3(r1, transform.position.y, r2), -transform.up, out hit, 1000, LayerMask.GetMask("Environment")))
                {
                    if (hit.point.y < 20)
                    {
                        notDone = false;
                        GameObject.Instantiate(abilityChest, hit.point + Vector3.up * .25f, Quaternion.LookRotation(Vector3.forward, hit.normal));
                    }
                }
            }
            StartCoroutine(DifficultyScaling());
        }
    }


    /// <summary>
    /// Scales the player and enemy damage and hp up over time
    /// </summary>
    /// <returns></returns>
    IEnumerator DifficultyScaling()
    {
        EnemySpawner spawner = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemySpawner>();
        Controller cont = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Controller>();

        float timer = 0;
        while (true)
        {
            if (Mathf.FloorToInt(timer) % 180 == 0 && Mathf.FloorToInt(timer) != 0)
            {
                timer = 0;
                spawner.LevelUp();
                cont.LevelUp();
            }
            timer += Time.deltaTime;
            yield return null;
        }
    }

    /// <summary>
    /// selects the character you want in character select
    /// </summary>
    /// <param name="index"></param>
    public void select(int index)
    {
        this.index = index;
    }

    /// <summary>
    /// changes X sensitivity from settings
    /// </summary>
    public void ChangeXSens()
    {
        xSens = x.value;
    }

    /// <summary>
    /// changes Y sensitivity from settings
    /// </summary>
    public void ChangeYSens()
    {
        ySens = y.value;
    }
}
