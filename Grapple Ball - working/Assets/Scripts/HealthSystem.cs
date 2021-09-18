using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour {


    [SerializeField]
    private int maxHealth = 10;
    public int currentHealth;
    private HealthBar health;

    private void Start()
    {
        health = GameObject.FindGameObjectWithTag("PlayerHpBar").GetComponent<HealthBar>();
        currentHealth = maxHealth;
    }

    public void ModifyHealth(int amount)
    {
        currentHealth += amount;
        float currentHealthPct = (float)currentHealth / (float)maxHealth;
        health.HandleHealthChanged(currentHealthPct);
    }

}
