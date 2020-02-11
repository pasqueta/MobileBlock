using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private static MenuManager instance;

    public static MenuManager Instance { get => instance; }

    #region Unity
    // Start is called before the first frame update
    void Start()
    {
        // if the singleton hasn't been initialized yet
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    public void Play(int sceneBuildIndex)
    {
        SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single);
    }
}
