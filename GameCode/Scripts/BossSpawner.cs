using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public UIElements ui;
    public GameObject boss, player, explodePos;
    public EnemySpawner spawner;
    public float force, height;
    bool isIn = false;
    // Start is called before the first frame update
    void Start()
    {
        ui = GameObject.FindGameObjectWithTag("MainUI").GetComponent<UIElements>();
        spawner = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemySpawner>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        explodePos.transform.LookAt(player.transform);

        // do stuff when player summons boss
        if (isIn && Input.GetKeyDown(KeyCode.E))
        {
            // push player away
            player.GetComponentInChildren<CharacterController>().enabled = false;
            player.GetComponentInChildren<Rigidbody>().AddExplosionForce(force, explodePos.transform.position, 20, height, ForceMode.Impulse);
            //player.GetComponent<Rigidbody>().AddForce(-explodePos.transform.forward.normalized * 5, ForceMode.Impulse);
            //player.GetComponent<Rigidbody>().AddForce((player.transform.position - new Vector3(transform.position.x, -2, transform.position.z)) * 10, ForceMode.Impulse);

            // make summoner invisible
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<LineRenderer>().enabled = false;
            GetComponent<SphereCollider>().enabled = false;

            // clean up enemies and spawn boss
            spawner.Clear();
            spawner.bossFight = true;
            StartCoroutine(Spawn());
            //Invoke("Spawn", 3);
            ui.spawnText.SetActive(false);
        }
    }

    // spawns the boss and destroys summoner
    IEnumerator Spawn()
    {
        yield return new WaitUntil(() => player.GetComponentInChildren<CharacterController>().enabled == true);
        yield return new WaitForSeconds(.5f);
        GameObject.Instantiate(boss, transform.position + Vector3.up * 3, Quaternion.identity);
        Destroy(gameObject);
        yield return null;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            isIn = true;
            ui.spawnText.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isIn = false;
            ui.spawnText.SetActive(false);
        }
    }
}
