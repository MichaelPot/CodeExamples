using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject charSelect;
    public GameObject charDesc;
    public GameObject description;
    public GameObject startButton;
    public GameObject settings;
    public Slider x, y;
    public Toggle accel;
    
    GameObject manager;
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameManager");
        x.value = GameManager.instance.xSens;//manager.GetComponent<GameManager>().xSens;
        y.value = GameManager.instance.ySens;//manager.GetComponent<GameManager>().ySens;
        x.onValueChanged.AddListener(delegate { XChanged(); });
        y.onValueChanged.AddListener(delegate { YChanged(); });

        accel.isOn = GameManager.instance.accel;
        accel.onValueChanged.AddListener(delegate { Accel(); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void XChanged()
    {
        GameManager.instance.xSens = x.value;
    }
    void YChanged()
    {
        GameManager.instance.ySens = y.value;
    }

    void Accel()
    {
        GameManager.instance.accel = accel.isOn;
    }
    public void startGame()
    {
        charSelect.SetActive(true);
        charDesc.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void toSettings()
    {
        mainMenu.SetActive(false);
        settings.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void backToMainMenu()
    {
        mainMenu.SetActive(true);
        settings.SetActive(false);
        charSelect.SetActive(false);
    }
    public void pickCube()
    {
        manager.GetComponent<GameManager>().select(0);
        description.GetComponent<Text>().text = "THE CUBE:\n\n The Cube is the foot soldier of The Order. All who are inducted into The Order start as a Cube--the lowest rank. It is from here that the shape must " +
            "prove its worth to climb the ranks.\n\n Every time you shoot, you lose health. Every time you get hit, you gain health. Lose too much or gain too much " +
            "and you die.\n";
        startButton.SetActive(true);
    }

    public void pickRect()
    {
        manager.GetComponent<GameManager>().select(1);
        description.GetComponent<Text>().text = "THE RECTANGULAR PRISM:\n\n The Rectangular Prism is a step above the Cube. It has proven itself worthy of being promoted and having some responsibilities " +
            "in The Order. The Rectangular Prism acts as a field commander and is in charge of Cubes.\n\n When in long form, the bullet can penetrate through enemies. " +
            "When in tall form, the bullet does higher damage to one target.\n";
        startButton.SetActive(true);
    }

    public void begin()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
