/// <summary>
/// Stand-in for UnityEvents to be able call methods from the 'SceneLoading' static class.
/// If using code, it's best to refer to 'SceneLoading' directly.
/// </summary>
class SceneLoader : Singleton<SceneLoader>
{
    protected override void Awake() {
        Instance = this;
    }

    public void LoadScene(int sceneIndex) => SceneLoading.LoadScene(sceneIndex);
    public void Quit() => SceneLoading.Quit();
}
