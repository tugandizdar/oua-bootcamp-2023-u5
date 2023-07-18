using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CharacterControl : MonoBehaviour
{
    Animator character_animator;
    [SerializeField]
    private float character_speed;
    public float health = 100;
    bool alive;
    // Start is called before the first frame update
    void Start()
    {
        character_animator = this.GetComponent<Animator>();
        alive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            alive = false;
            character_animator.SetBool("alive", alive);
        }
        if(alive == true)
        {
            Movement();
        }
        else 
        {
            GameObject end_menu = GameObject.Find("PauseMenu/Canvas/EndMenu");
            GameObject count_info = GameObject.Find("inGameHud/Info/Count Info");
            GameObject end_count = GameObject.Find("PauseMenu/Canvas/EndMenu/EndCount");
            end_count.GetComponent<Text>().text = count_info.GetComponent<Text>().text;




            end_menu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;
            
        }
    }
    public bool is_alive()
    {
        return alive;
    }
    public void take_damage()
    {
        health -= Random.Range(5, 10);
    }
    void Movement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        character_animator.SetFloat("Horizontal", horizontal);
        character_animator.SetFloat("Vertical", vertical);

        this.gameObject.transform.Translate(horizontal * character_speed * Time.deltaTime, 0, vertical * character_speed * Time.deltaTime);


    }

    public float get_health_info()
    {
        return health;
    }
}
