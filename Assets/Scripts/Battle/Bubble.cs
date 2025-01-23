using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public float life = 3f;
    public float speed = 2f;

    private float lifeTimer;
    private bool moving;
    private Vector3 direction;
    [SerializeField] private Rigidbody2D rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            rigidbody.MovePosition(transform.position + direction * speed * Time.deltaTime);
            lifeTimer -= Time.deltaTime;
            if (lifeTimer <= 0)
            {
                ReturnToPool();
            }
        }
    }

    public void Shoot(Vector3 d)
    {
        moving = true;
        direction = d;
        lifeTimer = life;
    }

    private void ReturnToPool()
    {
        moving = false;
        ObjectPoolManager.instance.Release(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
