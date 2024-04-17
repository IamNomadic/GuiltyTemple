using System.Collections;
using UnityEngine;

public class PlatformPassthrough : MonoBehaviour
{
    /*
     * Straight from a tutorial so we can get things moving:
     * to use this, attach this script to the player and fill in playerCollider /w the existing player object.
     * then, set up the passthrough platform with a "Platform Effector 2D", check "Use One Way", and on the Box Collider 2D, check "Used By Effector".
     */
    [SerializeField] private BoxCollider2D playerCollider;

    private GameObject currentPlatform;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode
                .S)) //I know this isn't ideal but I haven't touched the input parser for this whole project, I forget how it works >:V
            if (currentPlatform != null)
                StartCoroutine(PassThrough());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PassthroughPlatform")) currentPlatform = collision.gameObject;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PassthroughPlatform")) currentPlatform = null;
    }

    private IEnumerator PassThrough()
    {
        var platformCollider = currentPlatform.GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        yield return new WaitForSeconds(0.25f);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
    }
}