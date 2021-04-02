using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Caballol.Arkanoid.Gameplay
{
    public class Brick : MonoBehaviour
    {
        public event System.Action<Vector3> onDie;

        [SerializeField] [HideInInspector] private ScaleInOut m_scaleInOut;
        [SerializeField] [HideInInspector] private Destroyable m_destroyable;

        public bool IsDestroyable => m_destroyable;

        private void OnValidate()
        {
            m_scaleInOut = GetComponentInChildren<ScaleInOut>();
            m_destroyable = GetComponentInChildren<Destroyable>();
        }

        private void Awake()
        {
            if (m_destroyable)
            {
                m_destroyable.onDestroyed += () =>
                {
                    onDie(transform.position);
                    gameObject.SetActive(false);
                };
            }
        }

        public void Spawn()
        {
            gameObject.SetActive(true);
            m_scaleInOut.ScaleIn();
            if (m_destroyable)
            {
                m_destroyable.Spawn();
            }
        }
    }
}