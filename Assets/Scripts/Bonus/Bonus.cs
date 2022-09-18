using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{

    [SerializeField] private Vector3 axeRotation = new Vector3(30, 30, -45);
    [SerializeField] private float speed = 5f;

    public BonusType type;

    // Start is called before the first frame update
    void Start()
    {
        float rnd = Random.Range(0f, 1f);
        type = rnd < 0.5 ? BonusType.SUPERBALL : BonusType.EXTEND;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * 5, Space.World);
        transform.Rotate(axeRotation * speed * Time.deltaTime);
    }
}

public enum BonusType
{
    SUPERBALL,
    EXTEND
}
