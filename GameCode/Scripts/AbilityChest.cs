using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityChest : MonoBehaviour
{
    public GameObject itemSelector;
    public UIElements ui;

    int money;
    bool isIn = false;
    // Start is called before the first frame update
    void Start()
    {
        itemSelector = GameObject.FindGameObjectWithTag("MainUI").GetComponent<PauseMenu>().iSelector;
        ui = GameObject.FindGameObjectWithTag("MainUI").GetComponent<UIElements>();
        money = ui.moneyAmount;
        //itemSelector.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        money = ui.moneyAmount;

        // do stuff if player is in range of chest and has enough money
        if (isIn && Input.GetKeyDown(KeyCode.E) && money >= 100)
        {
            money -= 100;
            ui.moneyAmount -= 100;
            itemSelector.GetComponent<ItemSelector>().abilityItem = true;
            itemSelector.SetActive(true);
            Time.timeScale = 0;
            ui.abilityChestText.SetActive(false);
            itemSelector.GetComponent<ItemSelector>().abilityItem = false;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            ui.abilityChestText.SetActive(true);
            isIn = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            ui.abilityChestText.SetActive(false);
            isIn = false;
        }
    }
}
