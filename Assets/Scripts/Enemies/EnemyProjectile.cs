using System.Collections;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float deathTime; //how long does the explosion animation take

    private Vector2 direction;
    private bool exploding;
    private int hitDamage;
    private Rigidbody2D rb;
    private float speed;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        rb.velocity = direction * speed;
    }

    public void BeThrown(Vector2 throwDirection, float throwSpeed, int damage) //yeet
    {
        direction = throwDirection;
        speed = throwSpeed;
        hitDamage = damage;
    }

    public void OnCollision(Collision collision)
    {
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