using System.Collections;
using UnityEngine;

namespace KnightPlatformer.Creatures
{
    public abstract class Patrol : MonoBehaviour
    {
        public abstract IEnumerator DoPatrol();
    }
}
