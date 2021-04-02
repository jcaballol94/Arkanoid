using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Caballol.Arkanoid.Gameplay
{
    public abstract class DestroyObjectHandler : MonoBehaviour
    {
        public abstract void OnObjectDestroyed(Destroyable ar_destroyedObject);
    }
}