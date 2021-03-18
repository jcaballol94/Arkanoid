using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Caballol.Arkanoid
{
    public class Brick : MonoBehaviour
    {
        [SerializeField] private int m_life = 1;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (--m_life == 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}