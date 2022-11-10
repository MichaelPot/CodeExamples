using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * THIS CLASS IS TO KEEP TRACK OF ITEMS
 */
public class ItemManager : MonoBehaviour
{
    public GameObject player;
    public Dictionary<string, int> items = new Dictionary<string, int>();

    //public static ItemManager instance = null;

    /*private void Awake()
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
    }*/

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        /*foreach (string key in items.Keys)
        {
            Debug.Log(key);
        }*/
    }

    public void addItem(string iName)
    {
        if (!items.ContainsKey(iName))
        {
            items.Add(iName, 1);
        }
        else
        {
            items[iName] += 1;
        }
        switch (iName)
        {
            case "Gundous Cap": // more hp
                player.GetComponentInChildren<Controller>().maxHealth = player.GetComponentInChildren<Controller>().maxHealth + player.GetComponentInChildren<Controller>().maxHealth * .2f;
                break;
            case "The Spiralister": // low hp = higher dmg
                player.GetComponentInChildren<Controller>().lowHPDmg = .3f + items["The Spiralister"] * .1f;
                break;
            case "Ziry's Propterix": // in air = more dmg
                player.GetComponentInChildren<Controller>().airDmg = .2f + items["Ziry's Propterix"] * .1f;
                break;
            case "Fractal Diffractor": // extra bullets
                player.GetComponentInChildren<Controller>().numBullets += 2;
                break;
            case "A Forgotten Thing": // luckier
                player.GetComponentInChildren<Controller>().luck += 1;
                break;
            case "Hyper-Scoviss Fluid": //move faster
                player.GetComponentInChildren<PlayerMovement>().moveSpeed += 2;
                break;
            case "Ambly Springboard": //more jumps
                player.GetComponentInChildren<PlayerMovement>().jumps += 1;
                break;
            case "Fizzy Pop-Can": // kill = explode
                break;
            case "Aenodyr's Fossil": // kill = heal
                player.GetComponentInChildren<Controller>().healAmount = items[iName] * 35;
                break;
            case "The Brillig Tonemaser": // more regen
                /// GOTTA TWEAK THIS \\\
                player.GetComponentInChildren<Controller>().regen += 1f;
                break;
            case "Old Grayflod MZ": // increased fire rate
                player.GetComponentInChildren<Controller>().fireRate = player.GetComponentInChildren<Controller>().fireRate - player.GetComponentInChildren<Controller>().fireRate * .15f;
                break;
            case "critical": // increased crit chance
                player.GetComponentInChildren<Controller>().critChance += 10;
                break;
            case "lightning":
                player.GetComponentInChildren<Controller>().ChangeAbility("lightning");
                break;
            case "dash":
                player.GetComponentInChildren<Controller>().ChangeAbility("dash");
                break;
            case "invisible":
                player.GetComponentInChildren<Controller>().ChangeAbility("invisible");
                break;
            case "turret":
                player.GetComponentInChildren<Controller>().ChangeAbility("turret");
                break;
            default:
                Debug.Log("OH");
                break;
        }
    }
}
