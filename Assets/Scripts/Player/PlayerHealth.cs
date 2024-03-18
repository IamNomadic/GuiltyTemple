using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class PlayerHealth : MonoBehaviour
{
    public static event Action OnPlayerDamaged;
    public AudioSource DeathSound;
    public Animator animator;
    [SerializeField]
    public int maxHealth = 8;
    public int currentHealth;
    [SerializeField]
    private TMP_Text healthText;
    void Start()
    {
        currentHealth = maxHealth;
        healthText.text = currentHealth.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator LevelReset ()
    {
        yield return new WaitForSeconds(2);
        Debug.Log("called");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthText.text = currentHealth.ToString();
        OnPlayerDamaged?.Invoke();
        if (currentHealth <= 0)
        {
            StartCoroutine("LevelReset");
            Debug.Log("you are dead");
            DeathSound.Play();
            animator.Play("Hit");
           
            

        } 
    }
}
