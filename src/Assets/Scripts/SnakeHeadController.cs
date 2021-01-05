using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SnakeHeadController : MonoBehaviour
{
    private bool gameOver = false;
    private float minX, maxX, minY, maxY;
    private Vector2 direction = Vector2.right;
    private Vector2 lastDirection = Vector2.right;
    private float speed = 0.5f;

    [SerializeField] FoodSpawner foodSpawner;

    [SerializeField] GameObject bodyPartPrefab;

    [SerializeField] List<GameObject> bodyParts = new List<GameObject>();

    private Rigidbody2D rigidbody;
    private GameStatus gameStatus;

    void Start()
    {
        gameStatus = GameObject.FindObjectOfType<GameStatus>();
        rigidbody = GetComponent<Rigidbody2D>();

        var camera = Camera.main;
        minX = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.nearClipPlane)).x;
        maxX = camera.ViewportToWorldPoint(new Vector3(1, 0, camera.nearClipPlane)).x;
        minY = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.nearClipPlane)).y;
        maxY = camera.ViewportToWorldPoint(new Vector3(0, 1, camera.nearClipPlane)).y;

        StartCoroutine(nameof(MoveHead));
    }

    void Update()
    {
        MoveDirection();
        if (!gameOver)
        {
            CheckWallColision();
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "food")
        {
            var foodPosition = collider.gameObject.transform.position;
            Destroy(collider.gameObject);

            var newBodyPart = Instantiate(bodyPartPrefab, this.transform.position, Quaternion.identity);
            bodyParts.Insert(0, newBodyPart);
            this.transform.position = foodPosition;
            var gameStatus = GameObject.FindObjectOfType<GameStatus>();
            gameStatus.NewPoint();
        }
        else
        {
            if (collider.tag == "bodyPart" && collider.gameObject != bodyParts.FirstOrDefault() && collider.gameObject != bodyParts.Skip(1).FirstOrDefault())
            {
                GameOver();
            }
        }
    }

    private void CheckWallColision()
    {
        var headSize = this.GetComponent<Renderer>().bounds.size;
        var outOfBounds = ((this.transform.position.x - headSize.x / 2) < minX)
            || ((this.transform.position.x + headSize.x / 2) > maxX)
            || ((this.transform.position.y - headSize.y / 2) < minY)
            || ((this.transform.position.y + headSize.y / 2) > maxY);

        if (outOfBounds)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        StopCoroutine(nameof(MoveHead));
        gameStatus.GameOver();
    }

    private void MoveDirection()
    {
        if (Input.GetKey(KeyCode.A) && lastDirection != Vector2.right)
        {
            direction = Vector2.left;
        }
        if (Input.GetKey(KeyCode.D) && lastDirection != Vector2.left)
        {
            direction = Vector2.right;
        }
        if (Input.GetKey(KeyCode.W) && lastDirection != Vector2.down)
        {
            direction = Vector2.up;
        }
        if (Input.GetKey(KeyCode.S) && lastDirection != Vector2.up)
        {
            direction = Vector2.down;
        }
    }

    private IEnumerator MoveHead()
    {
        while (true)
        {
            lastDirection = direction;
            var lastPosition = this.transform.position;
            rigidbody.MovePosition(new Vector2(this.transform.position.x, this.transform.position.y) + (direction * speed));

            foreach (var part in bodyParts)
            {
                var pivot = part.transform.position;
                part.transform.position = lastPosition;
                lastPosition = pivot;
            }
            yield return new WaitForSeconds(0.1f);

        }
    }
}
