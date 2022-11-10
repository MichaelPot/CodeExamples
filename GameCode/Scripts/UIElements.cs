using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIElements : MonoBehaviour
{
    public Text survivalTime;
    public Text money;
    public GameObject chestText, spawnText, abilityChestText, readyColor;
    public int moneyAmount = 0;

    float timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 60)
        {
            survivalTime.text = "Time: " + ((int)(timer / 60)).ToString() + ":" + Mathf.FloorToInt(timer % 60).ToString();
        }
        else
        {
            survivalTime.text = "Time: " + Mathf.FloorToInt(timer).ToString();
        }
        money.text = moneyAmount.ToString();
    }
}
