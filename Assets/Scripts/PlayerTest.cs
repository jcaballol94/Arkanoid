using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    [SerializeField] private float m_speed = 10f;

    private Rigidbody2D m_rigidbody;
    private float m_target;

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
            }
        }
    }

    private void FixedUpdate()
    {
        var dist = m_target - transform.position.x;
        var velocity = Mathf.Clamp(dist / Time.deltaTime , - m_speed, m_speed);

        m_rigidbody.velocity = Vector2.right * velocity;
    }
}
