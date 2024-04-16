using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D Player)
    {
        Player.GetComponent<PlayerHealth>().TakeDamage(1);
        Rigidbody2D rb = Player.GetComponent<Rigidbody2D>();
        Vector2 knockbackDirection = (transform.position - rb.transform.position).normalized;
        rb.velocity = knockbackDirection * -3000;
    }
}
