using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Caballol.Arkanoid
{
    public class PowerUpManager : MonoBehaviour
    {
        public event System.Action<PowerUp> onPicked;

        [SerializeField] [Range(0f, 1f)] private float m_spawnProbability = 0.5f;

        public void SpawnIfNeeded (Vector3 position)
        {
            if (Random.value < m_spawnProbability)
            {

            }
        }

        private void OnPicked(PowerUp powerUp)
        {
            onPicked?.Invoke(powerUp);
        }
    }
}