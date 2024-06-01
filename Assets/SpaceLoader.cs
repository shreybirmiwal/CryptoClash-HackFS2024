using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SpaceLoader : MonoBehaviour
{
    private const string SCENE_SPACE = "Space";

    private List<Scene> _loadedScenes = new List<Scene>();

    public List<Scene> LoadedScenes => _loadedScenes;

    public async void LoadSpace(string url, string scene, bool unloadLoadedSpaces = true, Vector3 position = default, Vector3 rotation = default)
    {
        Debug.Log($"{nameof(LoadSpace)}.{nameof(LoadSpace)} - Loading Space...");

        if (unloadLoadedSpaces)
            await UnloadLoadedScenesAsync();

        await LoadScene(url, scene);

        // Copy objects to main scene
        await CopyObjectsToMainSceneAsync(scene);
    }

    private async Task LoadScene(string assetBundleUrl, string sceneName)
    {
        var bundle = await DownloadBundleAsync(assetBundleUrl, sceneName);
        if (bundle != null) await LoadAssetBundleAsync(bundle, sceneName);
    }

    public async Task<UnityWebRequest> SendWebRequestAsync(UnityWebRequest unityWebRequest)
    {
        var isComplete = false;

        StartCoroutine(SendWebRequestCoroutine(unityWebRequest, () => isComplete = true));

        while (!isComplete && !unityWebRequest.isDone)
            await Task.Yield();

        return unityWebRequest;
    }

    public IEnumerator SendWebRequestCoroutine(UnityWebRequest unityWebRequest, Action callback)
    {
        yield return unityWebRequest.SendWebRequest();
        callback?.Invoke();
    }

    private async Task<AssetBundle> DownloadBundleAsync(string assetBundleUrl, string sceneName)
    {
        Debug.Log($"{nameof(DownloadBundleAsync)} - Downloading Bundle ({sceneName}): {assetBundleUrl}");

#if !UNITY_WEBGL
        while (!Caching.ready) await Task.Yield();
#endif
        using (var request = UnityWebRequestAssetBundle.GetAssetBundle(assetBundleUrl))
        {
            await SendWebRequestAsync(request);

            while (!request.isDone)
            {
                await Task.Yield();
                Debug.Log($"downloading... {(int)(request.downloadProgress * 100)}");
            }

            if (request.result == UnityWebRequest.Result.Success)
            {
                return DownloadHandlerAssetBundle.GetContent(request);
            }
            else
            {
                var ex = $"{nameof(DownloadBundleAsync)} - Failed to download {sceneName} from {assetBundleUrl}, Error:{request.error}, Response Code:{request.responseCode}";
                Debug.LogError(ex);
            }
        }
        return null;
    }

    private async Task LoadAssetBundleAsync(AssetBundle bundle, string sceneName)
    {
        Debug.Log($"{nameof(LoadAssetBundleAsync)} - Loading Scene {sceneName} from Bundle");

        SceneManager.sceneLoaded -= HandleSceneLoaded;
        SceneManager.sceneLoaded += HandleSceneLoaded;

        var task = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        while (!task.isDone) await Task.Yield();

        Debug.Log($"{nameof(LoadAssetBundleAsync)} - Scene {sceneName} loaded");

        bundle.Unload(false);
    }

    private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _loadedScenes.Add(scene);
    }

    private async Task UnloadLoadedScenesAsync()
    {
        for (var i = _loadedScenes.Count - 1; i >= 0; i--)
        {
            var scene = _loadedScenes[i];
            Debug.Log($"{nameof(UnloadLoadedScenesAsync)} - Unloading Scene {scene.name}");

            var task = SceneManager.UnloadSceneAsync(scene);

            while (!task.isDone) await Task.Yield();

            _loadedScenes.RemoveAt(i);

            Debug.Log($"{nameof(UnloadLoadedScenesAsync)} - Scene {scene.name} unloaded");
        }
    }

    private async Task CopyObjectsToMainSceneAsync(string sceneName)
    {
        // Wait for the scene to be fully loaded
        while (!_loadedScenes.Exists(scene => scene.name == sceneName))
        {
            await Task.Yield();
        }

        Scene sourceScene = _loadedScenes.Find(scene => scene.name == sceneName);
        Scene mainScene = SceneManager.GetActiveScene();

        Debug.Log($"{nameof(CopyObjectsToMainSceneAsync)} - Copying objects from Scene {sceneName} to Main Scene");

        // Copy all root objects from the source scene to the main scene
        foreach (GameObject rootObject in sourceScene.GetRootGameObjects())
        {
            SceneManager.MoveGameObjectToScene(rootObject, mainScene);
            Debug.Log($"{nameof(CopyObjectsToMainSceneAsync)} - Moved object {rootObject.name} to Main Scene");
        }

        // Optionally unload the source scene after copying objects
        var task = SceneManager.UnloadSceneAsync(sourceScene);

        while (!task.isDone) await Task.Yield();

        _loadedScenes.Remove(sourceScene);
        Debug.Log($"{nameof(CopyObjectsToMainSceneAsync)} - Scene {sceneName} unloaded after copying objects");
    }
}
