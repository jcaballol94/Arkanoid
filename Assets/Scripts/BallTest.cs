using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTest : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;

    // Start is called before the first frame update
    void Start()
    {
        var rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = Random.insideUnitCircle * _speed;
    }
}
