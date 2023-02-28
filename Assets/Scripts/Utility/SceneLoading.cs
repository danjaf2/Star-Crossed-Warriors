using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoading {

    // We can put basic 'resets' here that will get executed at each scene load.
    public static event Action OnChangeScene = delegate {
        Time.timeScale = 1f;
    };

    public static event Action OnBeforeQuit = delegate { };


    public static void LoadScene(int sceneIndex) {
        SceneManager.LoadScene(sceneIndex);
        OnChangeScene.Invoke();
    }

    public static AsyncOperation LoadSceneAsync(int sceneIndex) {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneIndex);
        // Ensures the 'OnChangeScene' event will be raised once the scene loads.
        loadOperation.completed += (x) => OnChangeScene.Invoke();
        return loadOperation;
    }

    public static void Quit() {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif

        Application.Quit();
    }
}
