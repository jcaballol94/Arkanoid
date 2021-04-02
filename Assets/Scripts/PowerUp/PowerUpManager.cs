using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Caballol.Arkanoid.Gameplay
{
    public class PowerUpManager : MonoBehaviour
    {
        public event System.Action<PowerUp> onPicked;

        [SerializeField] private GameObject m_powerUpDropPrefab;
        [SerializeField] [Range(0f, 1f)] private float m_spawnProbability = 0.5f;
        [SerializeField] private PowerUp[] m_possiblePowerUps;

        private List<PowerUpDrop> m_dropPool = new List<PowerUpDrop>();

        private void Awake()
        {
            foreach (var powerUp in m_possiblePowerUps)
            {
                var dropGO = Instantiate(m_powerUpDropPrefab, transform);
                var drop = dropGO.GetComponent<PowerUpDrop>();

                drop.PowerUp = powerUp;
                drop.onPicked += OnPicked;

                Instantiate(powerUp.Visual, drop.transform);

                dropGO.SetActive(false);
                m_dropPool.Add(drop);
            }
        }

        public void SpawnIfNeeded (Vector3 a_position)
        {
            if (Random.value < m_spawnProbability)
            {
                var powerUp = m_dropPool[Random.Range(0, m_possiblePowerUps.Length)];
                if (powerUp && !powerUp.gameObject.activeSelf)
                {
                    powerUp.Spawn(a_position);
                }
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