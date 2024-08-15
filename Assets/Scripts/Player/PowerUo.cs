using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUo : MonoBehaviour
{
    public PowerUpSO data;
    public SpriteRenderer color;
    public PlayerMovement pM;

    private void Start()
    {
        color.color = data.color;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("speed");
        pM.walkSpeed = 2;
        Destroy(gameObject);

    
}

}
