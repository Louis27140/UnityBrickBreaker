using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int score = 0;
    public bool startGame = false;

    private GameObject brickTemplate;
    private GameObject ballTemplate;

    [SerializeField] private Vector2 bricksLayer = new Vector2(40, 6);
    [SerializeField] private List<brickObject> brickObjects = new List<brickObject>();

    public static GameManager inst;

    private float spaceBetween = 0.5f;
    private GameObject[] spawns;

    private List<GameObject> bricks = new List<GameObject>();

    [SerializeField] private TMP_Text scoreText;

    [SerializeField] private GameObject winPanel;

    private bool isPaused = false;

    private GameObject paddle;

    // Start is called before the first frame update
    void Start()
    {
        if (inst == null)
        {
            inst = this;
        }

        paddle = GameObject.FindGameObjectWithTag("Player");
        brickTemplate = Resources.Load<GameObject>("Prefabs/Brick");
        ballTemplate = Resources.Load<GameObject>("Prefabs/Ball");

        spawnBricks();
    }

    // Update is called once per frame
    void Update()
    {
        if (bricks.Count == 0) WinGame();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = isPaused ? 1f : 0;
            isPaused = !isPaused;
        }
    }

    private void spawnBricks()
    {
        spawns = GameObject.FindGameObjectsWithTag("SpawnBrick");

        float width = spawns.Sum(spawn => Mathf.Abs(spawn.transform.localPosition.x));
        float height = spawns.Sum(spawn => Mathf.Abs(spawn.transform.localPosition.y));

        float offsetX = brickTemplate.transform.localScale.x + spaceBetween;
        float offsetY = brickTemplate.transform.localScale.y + spaceBetween;

        int nbrMaxBrickX = (int)(width / offsetX);
        int nbrMaxBrickY = (int)(height / offsetY);

        int brickX = bricksLayer.x > nbrMaxBrickX ? nbrMaxBrickX : (int)bricksLayer.x;
        int brickY = bricksLayer.y > nbrMaxBrickY ? nbrMaxBrickY : (int)bricksLayer.y;

        for (int i = 0; i < brickX; i++)
        {
            for (int j = 0; j < brickY; j++)
            {
                Vector3 v3 = spawns[0].transform.position + new Vector3(i * offsetX, -j * offsetY);
                brick tmp = brickTemplate.GetComponent<brick>();
                tmp.b = brickObjects[j];
                bricks.Add(Instantiate(brickTemplate, v3, Quaternion.identity));
            }
        }
    }

    public void setScore(int score)
    {
        this.score += score;
        scoreText.text = this.score.ToString();
    }

    public void removeBricksOfList(GameObject go)
    {
        bricks.Remove(go);
    }

    private void WinGame()
    {
        winPanel.SetActive(true);
        Destroy(GameObject.FindGameObjectWithTag("Ball"));
    }

    public void reload()
    {
        winPanel.SetActive(false);
        restartGame();
    }
    public IEnumerator LoseGame()
    {
        WaitForSeconds wait = new WaitForSeconds(0.01f);
        foreach (GameObject go in shuffle(bricks))
        {
            go.GetComponent<brick>().Delete();
            yield return wait;
        }
        bricks.Clear();
        Debug.Log("Game Over");
        restartGame();
    }

    private List<GameObject> shuffle(List<GameObject> gos)
    {
        for (int i = 0; i < gos.Count; i++)
        {
            GameObject tmp = gos[i];
            int rnd = Random.Range(i, gos.Count);
            gos[i] = gos[rnd];
            gos[rnd] = tmp;
        }

        return gos;
    }

    private void restartGame()
    {
        SceneManager.LoadScene("Game");
    }

}
