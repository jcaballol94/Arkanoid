using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Caballol.Arkanoid.Gameplay
{
    public class PowerUpDrop : MonoBehaviour
    {
        public event System.Action<PowerUp> onPicked;

        public PowerUp PowerUp { get; set; }

        [SerializeField] [HideInInspector] private ScaleInOut m_scaleInOut;
        [SerializeField] private Destroyable m_kill;
        [SerializeField] private Destroyable m_pick;
        
        private void OnValidate()
        {
            m_scaleInOut = GetComponentInChildren<ScaleInOut>();
        }

        private void Awake()
        {
            m_kill.onDestroyed += () =>
            {
                gameObject.SetActive(false);
            };

            m_pick.onDestroyed += () =>
            {
                gameObject.SetActive(false);
                onPicked(PowerUp);
            };
        }

        public void Spawn (Vector3 a_position)
        {
            m_kill.Spawn();
            m_pick.Spawn();

            transform.position = a_position;
            gameObject.SetActive(true);
            m_scaleInOut.ScaleIn();
        }
    }
}