using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashAbility : AbilityItem
{
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (Input.GetKeyDown("q") && timer >= dashCd)
        {
            OnUse(0);
            timer = 0;
            StartCoroutine(pm.Dash());
        }
        if (timer >= dashCd)
        {
            ui.readyColor.GetComponent<Image>().color = new Color(1, 1, 1);
        }
        else
        {
            ui.readyColor.GetComponent<Image>().color = new Color(0, 0, 0);
        }
    }
}
