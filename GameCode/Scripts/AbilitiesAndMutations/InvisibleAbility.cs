using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.UI;


/// <summary>
/// AS OF NOW, DOES NOT AFFECT VOID FIELD ENEMY OR BOSSES
/// </summary>
public class InvisibleAbility : AbilityItem
{
    float invisTime = 3;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (Input.GetKeyDown("q") && timer >= invisCd)
        {
            OnUse(invisTime);

            GameObject m = null;
            /// might not work for rectangle ? \\\
            if (mutatorPos.GetComponentInChildren<Mutator>() != null)
            {
                m = GetComponentInChildren<Mutator>().gameObject;
                m.SetActive(false);
            }

            timer = 0;
            Outline outline = gameObject.AddComponent<Outline>();

            outline.OutlineMode = Outline.Mode.OutlineVisible;
            outline.OutlineColor = Color.cyan;
            outline.OutlineWidth = 3f;

            GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemySpawner>().PlayerInvisible(invisTime);
            Destroy(outline, invisTime);

            if (m != null)
                m.SetActive(true);
        }
        if (timer >= invisCd)
        {
            ui.readyColor.GetComponent<Image>().color = new Color(1, 1, 1);
        }
        else
        {
            ui.readyColor.GetComponent<Image>().color = new Color(0, 0, 0);
        }
    }
}
