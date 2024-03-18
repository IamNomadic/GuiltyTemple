using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public AudioSource DeathSound;
    public Animator animator;
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

    // Update is called once per frame
    void Update()
    {

    }
   // IEnumerator Reset ()
   // {
    //    yield WaitForSeconds (2);
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
   // }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthText.text = currentHealth.ToString();
        if (currentHealth <= 0)
        {
            Debug.Log("you are dead");
            animator.Play("Hit");
            DeathSound.Play();
            //StartCoroutine("Reset");
        }
    }
}
