using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretAbility : AbilityItem
{
    float upTime = 20;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (Input.GetKeyDown("q") && timer >= turretCd)
        {
            OnUse(upTime);

            timer = 0;
            
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 10000, ~(1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Chest"))))
            {
                GameObject t = GameObject.Instantiate(turret, hit.point, Quaternion.identity);
                t.GetComponent<TurretController>().damage = damage;
                GameObject.Destroy(t.gameObject, upTime);
            }
        }
        if (timer >= turretCd)
        {
            ui.readyColor.GetComponent<Image>().color = new Color(1, 1, 1);
        }
        else
        {
            ui.readyColor.GetComponent<Image>().color = new Color(0, 0, 0);
        }
    }
}
