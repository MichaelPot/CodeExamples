using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestController : MonoBehaviour
{
    public GameObject itemSelector;
    public UIElements ui;
    bool isIn = false;
    static bool available = true;
    // Start is called before the first frame update
    void Start()
    {
        itemSelector = GameObject.FindGameObjectWithTag("MainUI").GetComponent<PauseMenu>().iSelector;
        ui = GameObject.FindGameObjectWithTag("MainUI").GetComponent<UIElements>();
        //itemSelector.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        // open the item selector UI and destroy chest
        if (isIn && Input.GetKeyDown(KeyCode.E))
        {
            itemSelector.GetComponent<ItemSelector>().regItem = true;
            itemSelector.SetActive(true);
            Time.timeScale = 0;
            available = true;
            ui.chestText.SetActive(false);
            //itemSelector.GetComponent<ItemSelector>().regItem = false;
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && available)
        {
            isIn = true;
            ui.chestText.SetActive(true);
            available = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isIn = false;
            ui.chestText.SetActive(false);
            available = true;
        }
    }
}
