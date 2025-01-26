using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float force = 3f;

    private Transform target;
    private bool moving;
    private float size;
    [SerializeField] private Rigidbody2D rigidbody;
    [SerializeField] private Collider2D collider;
    [SerializeField] private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
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
        size = transform.localScale.x;

        collider.enabled = true;
        animator.SetBool("Run", true);

        rigidbody.mass = transform.lossyScale.x;
        rigidbody.drag = transform.lossyScale.x * 2;
    }

    public void SetOff()
    {
        moving = false;

        collider.enabled = false;
    }

    public void ReturnToPool(bool kill = true)
    {
        moving =false;
        if (kill)
        {
            GameManager.Instance.AddExp(size);
        }
        ObjectPoolManager.instance.Release(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && moving)
        {
            GameManager.Instance.Damage(transform.lossyScale.x);
            ReturnToPool(false);
        }
    }

    public void Damage(float d)
    {
        float currentSize = transform.localScale.x;
        currentSize -= d;
        if (currentSize < 0)
        {
            ReturnToPool();
        }
        else
        {
            transform.localScale = new Vector3(currentSize, currentSize, currentSize);
        }
    }
}
