using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxBehavior : MonoBehaviour
{
    [SerializeField]
    // Start is called before the first frame update
    void Start()
    {

    }
    /*
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("player struck with attack hitbox");
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(GetComponentInParent<Enemy>().damage);
            GetComponentInParent<Enemy>().HitboxInactive();
        }
    }
    */
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("player struck with attack hitbox");
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(GetComponentInParent<Enemy>().damage);
            GetComponentInParent<Enemy>().HitboxInactive();
        }
    }

}
