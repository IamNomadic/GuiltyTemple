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
        Vector2 knockbackDirection = (Player.GetComponent<Rigidbody2D>().transform.position - transform.position).normalized;
        Player.GetComponent<PlayerHealth>().TakeDamage(1, knockbackDirection);
        Rigidbody2D rb = Player.GetComponent<Rigidbody2D>();
    }
}
