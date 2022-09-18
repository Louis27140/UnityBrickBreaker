using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PaddleController : MonoBehaviour
{
    public float speed = 1.5f;

    #region variable input mobile
    private float distance;
    private bool dragging = false;
    private Transform toDrag;
    private Vector3 offset;
    #endregion
    [SerializeField] private GameObject paddle;
    [SerializeField] private GameObject ball;

    [SerializeField] private Transform LeftWall;
    [SerializeField] private Transform RightWall;

    private Vector3 move;

    private float paddleOffset = 2f;

    private float bonusTime = 0;

    private TMP_Text bonusTimeDisplay;

    private void Start()
    {
        bonusTimeDisplay = GetComponentInChildren<TMP_Text>();
    }

    private void Update()
    {
        if (bonusTime > 0)
        {
            bonusTime -= Time.deltaTime;
            bonusTimeDisplay.text = bonusTime.ToString("0.0");
        }
        else
        {
            bonusTimeDisplay.text = "";
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        move = new Vector3(Input.GetAxis("Horizontal"), 0, 0) * speed * Time.deltaTime;

        //InputMobileDrag(); A corriger

        if (move.x != 0 && !GameManager.inst.startGame)
        {
            if (ball == null) ball = GameObject.FindGameObjectWithTag("Ball");
            ball.GetComponent<BallController>().launchBall();
            GameManager.inst.startGame = true;
            return;
        }

        transform.Translate(move);

        if (LeftWall.position.x + paddleOffset > transform.position.x)
        {
            transform.position = new Vector3(LeftWall.position.x + paddleOffset, transform.position.y, transform.position.z);
        }
        else if (RightWall.position.x - paddleOffset < transform.position.x)
        {
            transform.position = new Vector3(RightWall.position.x - paddleOffset, transform.position.y, transform.position.z);
        }
    }

    private void InputMobileDrag()
    {
        if (Input.touchCount != 1)
        {
            dragging = false;
        }

        Touch touch = Input.touches[0];

        Vector3 touchPos = touch.position;
        if (touch.phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(touchPos);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Player"))
                {

                    toDrag = hit.transform;
                    distance = hit.transform.position.z - Camera.main.transform.position.z;

                    move = new Vector3(touchPos.x, 0, 0);
                    move = Camera.main.ScreenToWorldPoint(move);
                    offset = toDrag.position - move;
                    dragging = true;
                }
            }
        }

        if (dragging && touch.phase == TouchPhase.Moved)
        {
            move = new Vector3(Input.mousePosition.x, 0, 0);
            move = Camera.main.ScreenToWorldPoint(move);
            toDrag.position = move + offset;
        }

        if (dragging && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled))
        {
            dragging = false;
        }

        move.y = 0;
        move.z = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Bonus")) return;
        Bonus bonus = other.GetComponent<Bonus>();
        if (bonus != null && bonusTime == 0)
        {
            switch (bonus.type)
            {
                case BonusType.SUPERBALL:
                    StartCoroutine(ActiveSuperball());
                    break;
                case BonusType.EXTEND:
                    StartCoroutine(activeExtendPaddle());
                    break;
                default:
                    break;
            }
        }
        Destroy(bonus.gameObject);
    }

    private IEnumerator ActiveSuperball()
    {
        bonusTime = 10;
        if (ball != null)
        {
            ball.tag = "SuperBall";
            ball.GetComponent<BallController>().force = 20;
        }
        yield return new WaitUntil(() => bonusTime < 0);
        if (ball != null)
        {
            ball.tag = "Ball";
            ball.GetComponent<BallController>().force = 10;
        }
        
        bonusTime = 0;
    }

    private IEnumerator activeExtendPaddle()
    {
        bonusTime = 10;
        Vector3 paddleTmp = paddle.transform.localScale;
        paddle.transform.localScale += new Vector3(0, 1.5f, 0);
        yield return new WaitUntil(() => bonusTime < 0);
        paddle.transform.localScale = paddleTmp;
        bonusTime = 0;
    }
}
