using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
    private Rigidbody rb;

    public float force = 10f;

    private int nbrBounceWall = 0;
    private int nbrMaxBounceWall = 10;

    private AudioSource audioSource;
    private AudioClip hitClip;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        audioSource = GetComponent<AudioSource>();
        hitClip = Resources.Load<AudioClip>("Sounds/Hit");
    }

    private void FixedUpdate()
    {
        rb.velocity = force * (rb.velocity.normalized);
    }

    public void launchBall()
    {
        transform.parent = null;
        rb.velocity = Vector3.up * force;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            nbrBounceWall++;
            if (nbrBounceWall == nbrMaxBounceWall)
            {
                rb.velocity = Vector3.down * force;
                nbrBounceWall = 0;
            }
        }
        else if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Brick"))
        {
            nbrBounceWall = 0;
        }
        audioSource.PlayOneShot(hitClip);
    }
}
