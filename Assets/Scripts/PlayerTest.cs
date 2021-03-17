using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    [SerializeField] private float m_speed = 10f;
    [SerializeField] [Range(0f, 1f)] private float m_stickiness = 0.5f;

    private Rigidbody2D m_rigidbody;
    private float m_target;
    private bool m_move = false;

    private void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_target = transform.position.x;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            var camera = Camera.main;
            var ray = camera.ScreenPointToRay(Input.mousePosition);
            var gamePlane = new Plane(Vector3.back, Vector3.zero);

            if (gamePlane.Raycast(ray, out var dist))
            {
                var clickedPos = ray.GetPoint(dist);
                m_target = clickedPos.x;
                m_move = true;
                return;
            }
        }
        m_move = false;
    }

    private void FixedUpdate()
    {
        var velocity = 0f;

        if (m_move)
        {
            var dist = m_target - transform.position.x;
            velocity = Mathf.Clamp(dist / Time.deltaTime, -m_speed, m_speed);
        }

        m_rigidbody.velocity = Vector2.right * velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.rigidbody) return;

        // Combine the speed of the other object
        var mySpeed = m_rigidbody.velocity;
        collision.rigidbody.velocity += mySpeed * m_stickiness;
    }
}
