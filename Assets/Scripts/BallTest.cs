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
        m_rigidbody.velocity = m_rigidbody.velocity.normalized * m_speed;
    }
}
