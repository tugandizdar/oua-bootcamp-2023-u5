using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.AudioSource;




public class FiringSystem : MonoBehaviour
{
    Camera camera;
    public LayerMask zombie_layer;
    CharacterControl health_check;
    public ParticleSystem muzzle_flash;
    Animator character_animator;

    private float ammo = 30;
    private float extra_ammo = 300;
    private float magazine_capacity = 30;
    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        health_check = this.gameObject.GetComponent<CharacterControl>();
        character_animator = this.gameObject.GetComponent<Animator>();
        AudioSource audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(health_check.is_alive() == true )
        {
            
            
          
            if (Input.GetMouseButton(0))
            {
                 
                if(ammo > 0)
                {
                    character_animator.SetBool("firing", true);
                    
                }
                if (ammo <= 0)
                {
                    character_animator.SetBool("firing", false);
                }
               /* if (ammo <= 0 && extra_ammo > 0)
                {
                    character_animator.SetBool("reloading", true);

                    //character_animator.SetBool("firing", false);
                }*/
            }
            else //if(Input.GetMouseButtonUp(0))
            {
                character_animator.SetBool("firing", false);

            }
            if(Input.GetKeyDown(KeyCode.R))
            {
                character_animator.SetBool("reloading", true);
                
            }
        }

        
    }
    public void reloading()
    {
        extra_ammo -= magazine_capacity - ammo;
        ammo = magazine_capacity;
        character_animator.SetBool("reloading", false);
    }


    public void firing()
    {
        if (ammo > 0)
        { 
            if (audioSource != null)
            {
                audioSource.Play();
            }
            ammo--;
            muzzle_flash.Play();
            Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, zombie_layer))
            {
                hit.collider.gameObject.GetComponent<Zombie>().take_damage();
            }
        }
    }

    public void add_extra_ammo(int add_ammo)
    {
        extra_ammo += add_ammo;
    }

    public float get_ammo_info()
    {
        return ammo;
    }
    public float get_extra_ammo_info()
    {
        return extra_ammo;
    }
}
