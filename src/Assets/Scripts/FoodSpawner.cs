using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FoodSpawner : MonoBehaviour
{
    [SerializeField] GameObject foodPrefab;


    private float minX, maxX, minY, maxY;

    // Start is called before the first frame update
    void Start()
    {
        var camera = Camera.main;
        minX = camera.ViewportToWorldPoint(new Vector3(0.1f, 0, camera.nearClipPlane)).x;
        maxX = camera.ViewportToWorldPoint(new Vector3(0.9f, 0, camera.nearClipPlane)).x;
        minY = camera.ViewportToWorldPoint(new Vector3(0, 0.1f, camera.nearClipPlane)).y;
        maxY = camera.ViewportToWorldPoint(new Vector3(0, 0.9f, camera.nearClipPlane)).y;

        SpawnFood();
    }

    public void SpawnFood()
    {
        var allBodyParts = UnityEngine.Object.FindObjectsOfType<GameObject>().ToList().Where(x => x.tag == "bodyPart");
        var spawnPosition = GetSpawnPosition(allBodyParts);
        Instantiate(foodPrefab, spawnPosition, Quaternion.identity);
    }

    private Vector3 GetSpawnPosition(IEnumerable<GameObject> bodyParts)
    {
        var position = new Vector3(Mathf.Floor(Random.Range(minX, maxX)), Mathf.Floor(Random.Range(minY, maxY)), 0);

        foreach (var go in bodyParts)
        {
            float dist = Vector3.Distance(position, go.transform.position);
            if (dist < 0.49)
            {
                return GetSpawnPosition(bodyParts);
            }
        }

        return position;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
