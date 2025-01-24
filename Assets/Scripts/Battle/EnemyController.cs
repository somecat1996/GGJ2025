using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float force = 3f;

    private Transform target;
    private bool moving;
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
            rigidbody.AddForce(force * (target.position - transform.position).normalized);
        }
    }

    public void SetTarget(Transform t)
    {
        moving = true;
        target = t;
    }

    public void SetOff()
    {
        moving = false;
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
            ReturnToPool();
        }
    }
}
