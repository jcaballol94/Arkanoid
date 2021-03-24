using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Caballol.Arkanoid
{
    public class PowerUpManager : MonoBehaviour
    {
        public event System.Action<PowerUp> onPicked;

        [SerializeField] private GameObject m_powerUpDropPrefab;
        [SerializeField] [Range(0f, 1f)] private float m_spawnProbability = 0.5f;
        [SerializeField] private float m_dropSpeed = 1f;
        [SerializeField] private PowerUp[] m_possiblePowerUps;

        public void SpawnIfNeeded (Vector3 position)
        {
            if (Random.value < m_spawnProbability)
            {
                var dropGO = Instantiate(m_powerUpDropPrefab, transform);
                dropGO.transform.position = position;

                var drop = dropGO.GetComponent<PowerUpDrop>();
                drop.Speed = m_dropSpeed;
                drop.PowerUp = m_possiblePowerUps[Random.Range(0, m_possiblePowerUps.Length)];
                drop.onPicked += OnPicked;
            }
        }

        public void Clear()
        {
            // Remove all the powerups
            for (int i = transform.childCount - 1; i >= 0; --i)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }

        private void OnPicked(PowerUp powerUp)
        {
            onPicked?.Invoke(powerUp);
        }
    }
}