using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class brick : MonoBehaviour
{
    public brickObject b;

    private TMP_Text nbrHitText;

    private int actualHit = 0;

    private MeshRenderer mesh;

    [SerializeField, Range(0, 1)] private float bonusChanceToSpawn = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        nbrHitText = GetComponentInChildren<TMP_Text>();
        mesh.material.color = b.color;
        nbrHitText.text = b.nbrHit.ToString();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            actualHit++;
            if (actualHit == b.nbrHit)
            {
                GameManager.inst.setScore(b.score);
                GameManager.inst.removeBricksOfList(gameObject);
                DeleteWithDrop();
            }
            nbrHitText.text = (b.nbrHit - actualHit).ToString();
        }
        else if (collision.gameObject.CompareTag("SuperBall"))
        {
            GameManager.inst.setScore(b.score);
            GameManager.inst.removeBricksOfList(gameObject);
            Delete();
        }
    }

    private void DeleteWithDrop()
    {
        GameObject bonusTemplate = Resources.Load<GameObject>("Prefabs/Bonus");
        float rnd = Random.Range(0, 1);
        if (rnd < bonusChanceToSpawn) Instantiate(bonusTemplate, transform.position, bonusTemplate.transform.rotation);
        Destroy(gameObject);
    }

    public void Delete()
    {
        Destroy(gameObject);
    }
}
