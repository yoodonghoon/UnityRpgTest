using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : SingletonCommon<MySceneManager>
{
    public void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void BeforeLoadScene()
    {
        EventManager.Instance.OnLevelWasLoaded();
    }

    public void LoadScene(string name)
    {
        BeforeLoadScene();

        SceneManager.LoadSceneAsync(name);
    }
}
