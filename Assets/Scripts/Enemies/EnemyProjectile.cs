using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    Rigidbody2D rb;
    bool exploding = false;
    [SerializeField]
    float deathTime; //how long does the explosion animation take
    Vector2 direction;
    float speed;
    int hitDamage;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = direction * speed;
    }

    public void BeThrown(Vector2 throwDirection, float throwSpeed, int damage) //yeet
    {
        direction = throwDirection;
        speed = throwSpeed;
        hitDamage = damage;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("projectile colliding");
        if (collision.gameObject.tag == "Player")
        {
            //send damage to the player            
        }
        Destroy(gameObject);
        if (!exploding)
        {
            StartCoroutine(DeathTimer());
            exploding = true;
            rb.velocity = new Vector2(0, 0);
        }
        //play explosion animation or somesuch
        IEnumerator DeathTimer()
        {
            yield return new WaitForSeconds(deathTime);

        }
    }
}
