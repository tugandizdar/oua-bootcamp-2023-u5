using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private float health;
    private float lerpTimer;
    public float maxHealth = 100f;
    public float chipSpeed = 2f;
    public Image healthBar;


    void Start()
    {

        health = maxHealth;
    }


    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();
    }

    public void UpdateHealthUI()
    {
        Debug.Log(health);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        lerpTimer = 0f;

            }
}
