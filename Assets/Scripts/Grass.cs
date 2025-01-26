using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour
{
    public float distance = 20f;
    public Sprite[] sprites;
    [SerializeField] SpriteRenderer sprite;
    private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;


        sprite.sprite = sprites[Random.Range(0, sprites.Length)];
        Vector3 pos = Random.insideUnitSphere;
        pos.z = 0;
        transform.position = player.transform.position + pos * distance;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            if (Vector3.Magnitude(player.transform.position - transform.position) > distance)
            {
                RandomSprite();
            }
        }
    }

    private void RandomSprite()
    {
        sprite.sprite = sprites[Random.Range(0, sprites.Length)];
        Vector3 pos = Random.insideUnitSphere;
        pos.z = 0;
        transform.position = player.transform.position + pos.normalized * distance;
    }
}
