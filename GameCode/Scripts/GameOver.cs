using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public GameObject gameOver;
    public Text deathTime;

    Animator anim;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        anim = gameOver.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
    }

    public void Died()
    {
        gameOver.SetActive(true);
        anim.SetTrigger("Dead");
        deathTime.text += "\nTime survived: " + Mathf.Round(timer).ToString();
        Cursor.lockState = CursorLockMode.Confined;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
