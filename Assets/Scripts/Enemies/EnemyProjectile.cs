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
    private bool hit = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        rb.velocity = direction * speed;
        if (!GetComponent<Renderer>().isVisible || hit == true)
        {
            Destroy(gameObject);
        }
    }

    public void BeThrown(Vector2 throwDirection, float throwSpeed, int damage) //yeet
    {
        direction = throwDirection;
        speed = throwSpeed;
        hitDamage = damage;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            hit = true;
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(hitDamage);
        }

    }

}