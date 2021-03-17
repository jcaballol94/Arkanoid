using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField] private int m_life = 1;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (--m_life == 0)
        {
            Destroy(this.gameObject);
        }
    }
}
