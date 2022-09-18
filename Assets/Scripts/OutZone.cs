using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutZone : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball") || other.gameObject.CompareTag("SuperBall"))
        {
            StartCoroutine(GameManager.inst.LoseGame());
            Destroy(other.gameObject);
        }
    }
}
