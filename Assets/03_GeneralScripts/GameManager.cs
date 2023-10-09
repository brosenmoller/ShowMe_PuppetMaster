using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private ServiceLocator serviceLocator;

    private void Awake()
    {
        serviceLocator = new ServiceLocator();
        ServiceSetup();
    }

    private void ServiceSetup()
    {
        serviceLocator.Add(new InputService());
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene loadedScene, LoadSceneMode loadSceneMode)
    {
        serviceLocator.OnSceneLoaded();
    }

    private void FixedUpdate()
    {
        serviceLocator.FixedUpdate();
    }
}
