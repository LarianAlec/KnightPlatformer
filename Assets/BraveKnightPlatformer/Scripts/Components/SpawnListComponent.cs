using System.Linq;
using UnityEngine;

namespace KnightPlatformer.Components
{
    public class SpawnListComponent : MonoBehaviour
    {
        [SerializeField] private SpawnData[] _spawners;

        public void Spawn(string id)
        {
            var spawner = _spawners.FirstOrDefault(element => element.Id == id);
            spawner?.Component.Spawn();
            /* It is equivalent of above code
            foreach (var data in _spawners)
            {
                if (data.Id == id)
                {
                    data.Component.Spawn();
                    break;
                }
            }*/
        }



        [SerializeField] 
        public class SpawnData : MonoBehaviour
        {
            public string Id;
            public SpawnComponent Component;
        }
    }
}
