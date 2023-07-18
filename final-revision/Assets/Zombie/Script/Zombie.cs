using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.UI;


public class Zombie : MonoBehaviour
{
    public float zombie_hp = 100;
    Animator zombie_animator;
    bool zombie_dead;
    float distance;
    public float trigger_distance;
    public float attack_distance;
    NavMeshAgent zombie_nav_mesh;


    GameObject target_player;
    // Start is called before the first frame update
    void Start()
    {
        zombie_animator = this.GetComponent<Animator>();
        target_player = GameObject.Find("Swat");
        zombie_nav_mesh = this.GetComponent<NavMeshAgent>();
        //zombie_nav_mesh.
    }

    // Update is called once per frame
    void Update()
    {
        if(zombie_hp <= 0)
        {
            zombie_dead = true;
        }
        if(zombie_dead)
        {
            zombie_animator.SetBool("died", true);
            StartCoroutine(disappear());
        }
        else
        {
            distance = Vector3.Distance(this.transform.position, target_player.transform.position);
            if(distance < trigger_distance)
            {
                zombie_nav_mesh.isStopped = false;
                zombie_nav_mesh.SetDestination(target_player.transform.position);
                zombie_animator.SetBool("walking", true);
                zombie_animator.SetBool("attacking", false);
                this.transform.LookAt(target_player.transform.position);
            }
            else 
            {
                zombie_nav_mesh.isStopped = true;
                zombie_animator.SetBool("walking", false);
                zombie_animator.SetBool("attacking", false);
                //durma
            }
            if (distance < attack_distance)
            {
                zombie_nav_mesh.isStopped = true;
                zombie_animator.SetBool("walking", false);
                zombie_animator.SetBool("attacking", true);
                this.transform.LookAt(target_player.transform.position);
            }
            //zombi hareket kodu yazilacak
        }
    }

    public void attack()
    {
        target_player.GetComponent<CharacterControl>().take_damage();
    }
    IEnumerator disappear()
    {
        
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
        target_player.GetComponent<FiringSystem>().add_extra_ammo(10);
        GameObject count_info = GameObject.Find("inGameHud/Info/Count Info");
        int counter = int.Parse(count_info.GetComponent<Text>().text);
        count_info.GetComponent<Text>().text = (counter+1).ToString();
    }
    public void take_damage()
    {
        int damage = Random.Range(15, 25);
        zombie_hp -= damage;
        
        this.transform.Find("health_loss").position = this.transform.position + new Vector3(Random.Range(-0.60f, 0.60f), Random.Range(1.75f, 2.00f), 0.0f);
        TextMeshPro textMeshProComponent = this.transform.Find("health_loss").GetComponent<TextMeshPro>();
        textMeshProComponent.text = damage.ToString();
       
    }
}
