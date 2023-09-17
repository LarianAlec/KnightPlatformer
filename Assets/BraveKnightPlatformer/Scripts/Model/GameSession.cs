using UnityEngine;

namespace Model
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] PlayerData _data;
        public PlayerData Data => _data;

        private void Awake()
        {
            if(IsSessionExist())
            {
                DestroyImmediate(gameObject);
            }
            else
            {
                DontDestroyOnLoad(this);
            }
        }

        private bool IsSessionExist()
        {
            var sessionsObjects = FindObjectsOfType<GameSession>();
            foreach (var gameSession in sessionsObjects)
            {
                if (gameSession != this)
                {
                    return true;
                }
            }
            return false;
        }

    }
}