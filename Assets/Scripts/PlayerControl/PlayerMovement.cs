using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float force = 10.0f;

    [SerializeField] private Rigidbody2D rigidbody;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform body;
    private bool flip = false;
    private Vector3 moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.IsRunning())
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
            if (moveDirection.magnitude > 0 && !animator.GetBool("Run"))
            {
                animator.SetBool("Idle", false);
                animator.SetBool("Run", true);
                if (moveDirection.x < 0 && !flip)
                {
                    flip = true;
                    body.localScale = new Vector3(-body.localScale.x, body.localScale.y, body.localScale.z);
                }
                else if (moveDirection.x > 0 && flip)
                {
                    flip = false;
                    body.localScale = new Vector3(-body.localScale.x, body.localScale.y, body.localScale.z);
                }
            }
            else if (moveDirection.magnitude == 0 && animator.GetBool("Run"))
            {
                animator.SetBool("Idle", true);
                animator.SetBool("Run", false);
            }
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.IsRunning())
            rigidbody.AddForce(moveDirection * (force + GameManager.Instance.movingSpeed));
    }
}
