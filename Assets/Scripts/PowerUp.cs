using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Caballol.Arkanoid
{
    [CreateAssetMenu(fileName = "PowerUp", menuName = "Arkanoid/PowerUp", order = 0)]
    public class PowerUp : ScriptableObject
    {
        public GameObject Visual => m_visual;
        [SerializeField] private GameObject m_visual;
    }
}