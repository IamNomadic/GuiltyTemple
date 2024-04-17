using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public AudioSource DeathSound;
    public PlayerMovement playerMovement;
    public int maxHealth;
    public int currentHealth;

    [SerializeField] private bool delayingRegen;

    [SerializeField] private TMP_Text healthText;

    public bool dead;
    private Animator animator;
    private float healthToBe;

    private void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        healthText.text = currentHealth.ToString();
        delayingRegen = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (currentHealth >= maxHealth) currentHealth = maxHealth;
        if (playerMovement.WTransformed && currentHealth < maxHealth && !delayingRegen)
        {
            currentHealth += 1;
            StartCoroutine(RegenDelay());
            delayingRegen = true;
            Debug.Log("called");
            OnPlayerDamaged?.Invoke();
        }

        IEnumerator RegenDelay()
        {
            yield return new WaitForSeconds(1);
            delayingRegen = false;
        }
    }

    public static event Action OnPlayerDamaged;

    private IEnumerator LevelReset()
    {
        yield return new WaitForSeconds(2);
        Debug.Log("called");
        dead = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        playerMovement.canMove = true;
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
            dead = true;
            playerMovement.canMove = false;
        }
    }
}