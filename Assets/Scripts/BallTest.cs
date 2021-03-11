using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTest : MonoBehaviour
{
    [SerializeField] private float m_speed = 5f;

    private Rigidbody2D m_rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_rigidbody.velocity = Random.insideUnitCircle * m_speed;
    }

    private void FixedUpdate()
    {
        // Keep the speed constant
        var velocity = m_rigidbody.velocity.normalized * m_speed;
        // Make sure that the ball always moves vertically
        if (Mathf.Approximately(0f, velocity.y)) velocity.y = -0.1f;

        m_rigidbody.velocity = velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.rigidbody) return;

        // Combine the speed of the other object
        var otherSpeed = collision.rigidbody.velocity;
        m_rigidbody.velocity += otherSpeed;
    }
}
