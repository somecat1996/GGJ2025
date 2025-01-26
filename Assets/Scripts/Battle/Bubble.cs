using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public float life = 3f;
    public float force = 2f;
    public float burstForce = 200f;

    private float lifeTimer;
    private bool moving;
    [SerializeField] private Rigidbody2D rigidbody;
    private List<EnemyController> enemyControllers;

    // Start is called before the first frame update
    void Start()
    {
        enemyControllers = new List<EnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.IsRunning())
        {
            foreach (EnemyController controller in enemyControllers)
            {
                controller.transform.position = transform.position;
            }
            if (moving)
            {
                lifeTimer -= Time.deltaTime;
                if (lifeTimer <= 0)
                {
                    ReturnToPool();
                }
            }
        }
    }

    public void Shoot(Vector3 direction)
    {
        moving = true;
        lifeTimer = life;
        rigidbody.AddForce(direction * force / (5 * transform.localScale.x), ForceMode2D.Impulse);
    }

    private void ReturnToPool()
    {
        foreach(EnemyController controller in enemyControllers)
        {
            controller.ReturnToPool();
        }
        enemyControllers.Clear();
        moving = false;
        ObjectPoolManager.instance.Release(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Vector3 direction = transform.position - collision.transform.position;
            Vector3 velocity = collision.GetComponent<Rigidbody2D>().velocity;
            float f = Vector3.Dot(direction, velocity) / direction.magnitude;
            rigidbody.AddForce(direction.normalized * force * f / (5 * transform.localScale.x), ForceMode2D.Impulse);

        }
        else if (collision.tag == "Enemy")
        {
            if (transform.localScale.x < collision.transform.localScale.x)
            {
                Vector3 direction = collision.transform.position - transform.position;
                collision.GetComponent<Rigidbody2D>().AddForce(direction * force, ForceMode2D.Impulse);
                collision.GetComponent<EnemyController>().Damage(transform.localScale.x);
                ReturnToPool();
            }
            else
            {
                collision.GetComponent<EnemyController>().SetOff();
                enemyControllers.Add(collision.GetComponent<EnemyController>());
            }
        }
    }
}
