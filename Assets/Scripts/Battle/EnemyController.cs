using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float force = 3f;

    private Transform target;
    private bool moving;
    [SerializeField] private Rigidbody2D rigidbody;
    [SerializeField] private Collider2D collider;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (GameManager.Instance.IsRunning())
        {
            if (moving)
            {
                rigidbody.AddForce(force * (target.position - transform.position).normalized);
            }
        }
    }

    public void SetTarget(Transform t)
    {
        moving = true;
        target = t;

        collider.enabled = true;
    }

    public void SetOff()
    {
        moving = false;

        collider.enabled = false;
    }

    public void ReturnToPool()
    {
        moving =false;
        ObjectPoolManager.instance.Release(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && moving)
        {
            GameManager.Instance.Damage(1);
            ReturnToPool();
        }
    }
}
