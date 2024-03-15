using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{

    [SerializeField]
    private int maxHealth = 6;
    public int currentHealth;
    [SerializeField]
    private TMP_Text healthText;
    void Start()
    {
        currentHealth = maxHealth;
        healthText.text = currentHealth.ToString();
    }

    

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthText.text = currentHealth.ToString();
        if(currentHealth <= 0)
        {
            Debug.Log("you are dead");
        }
    }    
}
