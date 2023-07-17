using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIControl : MonoBehaviour
{
    public Text ammo_info;
    public Text health_info;
    //public GameObject pause_menu;

    bool game_stopped;

    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Swat");
    }

    // Update is called once per frame
    void Update()
    {
        ammo_info.text = player.GetComponent<FiringSystem>().get_ammo_info().ToString() + " / " + player.GetComponent<FiringSystem>().get_extra_ammo_info().ToString();
        health_info.text = "HP = " + player.GetComponent<CharacterControl>().get_health_info().ToString();
        /*if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(game_stopped == true)
            { continue_game(); }
            else if (game_stopped == false)
            { pause_game(); }
        }*/
    }

    /*public void continue_game()
    {
        Debug.Log("asdsad");
        Cursor.lockState = CursorLockMode.Locked;
        game_stopped = false;
        //pause_menu.SetActive(false);
        Time.timeScale = 1;
    }
    public void pause_game()
    {
        Cursor.lockState = CursorLockMode.None;
        game_stopped = true;
        //pause_menu.SetActive(true);
        Time.timeScale = 0;
    }*/

    /*public void scene_main_menu()
    {
        SceneManager.LoadScene("MainMenu");
    }*/
}
