using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    public Vector2 enemySize;
    public float generateRadius = 10f;
    public float generateTime = 5f;

    private Transform player;
    private float timer;
    private float generateTimer;
    private bool start;

    [Button]
    private void StartTest()
    {
        Initiate();
    }


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        start = false;
    }

    private void Initiate()
    {
        timer = 0;
        generateTimer = 0;
        start = true;
        Debug.Log("Game Start!");
    }

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            timer += Time.deltaTime;
            generateTimer += Time.deltaTime;
            if (generateTimer >= generateTime)
            {
                generateTimer = 0;
                float weight = Mathf.Sqrt(timer);
                while (weight >= enemySize.y)
                {
                    float size = Random.Range(enemySize.x, enemySize.y);
                    weight -= size;
                    Vector3 position = Random.insideUnitSphere;
                    position.z = 0;

                    CreateEnemy(player.position + position.normalized * generateRadius, size);
                }
            }
        }
    }

    private void CreateEnemy(Vector3 position, float size)
    {
        GameObject enemy = ObjectPoolManager.instance.Get(enemyPrefab);
        enemy.transform.position = position;
        enemy.transform.localScale = new Vector3(size, size, size);
        enemy.SetActive(true);
        enemy.GetComponent<EnemyController>().SetTarget(player);
    }
}
