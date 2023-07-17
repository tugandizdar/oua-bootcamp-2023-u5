using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class new_game : MonoBehaviour
{
    public void load_scene()
    { 
        SceneManager.LoadScene("Main_Game_Scene"); 
    }
}
