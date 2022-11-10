using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSelector : MonoBehaviour
{

    /// <summary>
    ///  MAKE Item A CLASS THAT HAS AN ITEM'S NAME, ICON, AND DESCRIPTION AND THEN MAKE A LIST OF ITEMS TO OPTIMIZE THIS
    /// </summary>
    public GameObject iMan, inventory;
    public List<Sprite> icons = new List<Sprite>();
    public List<string> itemNames = new List<string>();
    public List<Image> images = new List<Image>();
    public List<Text> names = new List<Text>();
    private Dictionary<string, string> items = new Dictionary<string, string>();
    private Dictionary<string, string> abilityItems = new Dictionary<string, string>();
    //private List<string> itemDescriptions = new List<string>();
    private List<string> abilityItemNames = new List<string>();
    private List<Text> texts = new List<Text>();
    private string s1, s2, s3;
    private Sprite p1, p2, p3;

    public bool regItem = false, abilityItem = false;

    private void Awake()
    {
        items.Add("The Spiralister", "Increases damage when health is below 30%.");
        items.Add("Ziry's Propterix", "You do more damage while in the air");
        items.Add("Fractal Diffractor", "Every time you shoot, 2 extra bullets are shot in a random direction.");
        items.Add("A Forgotten Thing", "Makes you luckier");
        items.Add("Gundous Cap", "Increases max HP");
        items.Add("Hyper-Scoviss Fluid", "Increases move speed");
        items.Add("Ambly Springboard", "Increases the number of jumps you have.");
        items.Add("Fizzy Pop-Can", "Killing an enemy causes an explosion.");
        items.Add("Aenodyr's Fossil", "Killing an enemy heals you.");
        items.Add("The Brillig Tonemaser", "Increases HP regen.");
        items.Add("Old Grayflod MZ", "Increases fire rate.");
        items.Add("critical", "Increases crit chance.");

        /*itemNames.Add("increasedLowDmg");
        itemNames.Add("increasedAirDmg");
        itemNames.Add("extraBul");
        itemNames.Add("luck");
        itemNames.Add("maxHp");
        itemNames.Add("moveSpeed");
        itemNames.Add("jumps");
        itemNames.Add("killExplosion");
        itemNames.Add("killHeal");
        itemNames.Add("regen");
        itemNames.Add("fireRate");
        itemNames.Add("critical");*/


        abilityItems.Add("lightning", "Summon a lightning strike.");
        abilityItems.Add("dash", "Dash in whatever direction you are looking.");
        abilityItems.Add("invisible", "Become invisible to most enemies, but not bosses.");
        abilityItems.Add("turret", "Place a turret where you are looking..");

        abilityItemNames.Add("lightning");
        abilityItemNames.Add("dash");
        abilityItemNames.Add("invisible");
        abilityItemNames.Add("turret");

        texts.Add(GameObject.Find("Item1").GetComponentInChildren<Text>());
        texts.Add(GameObject.Find("Item2").GetComponentInChildren<Text>());
        texts.Add(GameObject.Find("Item3").GetComponentInChildren<Text>());
        /*
        itemDescriptions.Add("Increases damage by 50% when health is below 30%.");
        itemDescriptions.Add("You do more damage while in the air");
        itemDescriptions.Add("Every time you shoot, an extra bullet is shot in a random direction.");
        itemDescriptions.Add("Makes you luckier");
        itemDescriptions.Add("Increases max HP");
        itemDescriptions.Add("Increases move speed");
        itemDescriptions.Add("Increases the number of jumps you have.");
        itemDescriptions.Add("Killing an enemy causes an explosion.");
        itemDescriptions.Add("Killing an enemy heals you.");
        itemDescriptions.Add("Increases HP regen.");
        itemDescriptions.Add("Increases fire rate.");
        */
    }
    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
        if (regItem)
        {
            GetRegularItem();
        }
        else if (abilityItem)
        {
            GetAbilityItem();
        }
    }

    void GetAbilityItem()
    {
        int rand1 = Random.Range(0, abilityItems.Count);
        texts[0].text = abilityItems[abilityItemNames[rand1]];
        names[0].text = abilityItemNames[rand1];
        s1 = abilityItemNames[rand1];

        int rand2 = Random.Range(0, abilityItems.Count);
        while (rand2 == rand1)
        {
            rand2 = Random.Range(0, abilityItems.Count);
        }
        texts[1].text = abilityItems[abilityItemNames[rand2]];
        names[1].text = abilityItemNames[rand2];
        s2 = abilityItemNames[rand2];

        int rand3 = Random.Range(0, abilityItems.Count);
        while (rand3 == rand2 || rand3 == rand1)
        {
            rand3 = Random.Range(0, abilityItems.Count);
        }
        texts[2].text = abilityItems[abilityItemNames[rand3]];
        names[2].text = abilityItemNames[rand3];
        s3 = abilityItemNames[rand3];

        regItem = false;
        abilityItem = true;
    }

    void GetRegularItem()
    {
        int rand1 = Random.Range(0, items.Count);
        texts[0].text = items[itemNames[rand1]];
        images[0].sprite = icons[rand1];
        names[0].text = itemNames[rand1];
        s1 = itemNames[rand1];
        p1 = icons[rand1];

        int rand2 = Random.Range(0, items.Count);
        while (rand2 == rand1)
        {
            rand2 = Random.Range(0, items.Count);
        }
        texts[1].text = items[itemNames[rand2]];
        images[1].sprite = icons[rand2];
        names[1].text = itemNames[rand2];
        s2 = itemNames[rand2];
        p2 = icons[rand2];

        int rand3 = Random.Range(0, items.Count);
        while (rand3 == rand2 || rand3 == rand1)
        {
            rand3 = Random.Range(0, items.Count);
        }
        texts[2].text = items[itemNames[rand3]];
        images[2].sprite = icons[rand3];
        names[2].text = itemNames[rand3];
        s3 = itemNames[rand3];
        p3 = icons[rand3];

        regItem = true;
        abilityItem = false;
    }

    public void Item1()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        this.gameObject.SetActive(false);
        iMan.GetComponent<ItemManager>().addItem(s1);
        if (regItem)
            inventory.GetComponent<Inventory>().AddItem(p1, s1);
        Clear();
    }

    public void Item2()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        this.gameObject.SetActive(false);
        iMan.GetComponent<ItemManager>().addItem(s2);
        if (regItem)
            inventory.GetComponent<Inventory>().AddItem(p2, s2);
        Clear();
    }

    public void Item3()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        this.gameObject.SetActive(false);
        iMan.GetComponent<ItemManager>().addItem(s3);
        if (regItem)
            inventory.GetComponent<Inventory>().AddItem(p3, s3);
        Clear();
    }

    void Clear()
    {
        regItem = false;
        abilityItem = false;

        for (int i = 0; i < images.Count; i++)
        {
            images[i].sprite = null;
        }
    }
}
