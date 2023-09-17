using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitLevelComponent : MonoBehaviour 
{
    [SerializeField] private string _scene;

    public void Exit()
    {
        SceneManager.LoadScene(_scene);
    }
}
