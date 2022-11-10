using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.UI;


public class LightningAbility : AbilityItem
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
        if (Input.GetKeyDown("q") && timer >= lightningCd)
        {
            OnUse(0);

            timer = 0;
            
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 10000, ~(1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Chest"))))
            {
                VisualEffect v = GameObject.Instantiate(lightning, hit.point + (Vector3.up * 28), Quaternion.identity);
                StartCoroutine(LightningDmg(hit));
                GameObject.Destroy(v.gameObject, 1.5f);
            }
        }
        if (timer >= lightningCd)
        {
            ui.readyColor.GetComponent<Image>().color = new Color(1, 1, 1);
        }
        else
        {
            ui.readyColor.GetComponent<Image>().color = new Color(0, 0, 0);
        }
    }
    IEnumerator LightningDmg(RaycastHit hit)
    {
        /// MAYBE JUST MAKE IT INSTANT ??? BUT THEN IT WONT LINE UP WITH WHEN LIGHTNING HITS GROUND \\\
        yield return new WaitForSeconds(.15f);
        Collider[] colliders = Physics.OverlapSphere(hit.point, 5f, LayerMask.GetMask("Enemy"));
        foreach (Collider c in colliders)
        {
            c.GetComponent<Enemy>()?.TakeDamage(damage);
            c.GetComponentInParent<CubeBoss>()?.TakeDamage(damage);
        }
        yield return null;
    }
}
