using System;
using System.Collections;
using System.Threading.Tasks;
using UniGLTF;
using UnityEngine;
using UnityEngine.Networking;
using VRM;
using UnityGLTF;
using UnityGLTF.Loader;
using System.Runtime.ExceptionServices;
using System.IO;


public class StreamLoader : IDataLoader
{
    private Stream _stream;

    public StreamLoader(Stream stream)
    {
        _stream = stream;
    }
    public async Task<Stream> LoadStreamAsync(string relativeFilePath)
    {
        return _stream;
    }
}

public class GlbLoader : MonoBehaviour
{

    public void Load(string url, bool importAnimation, Action<GameObject> callback)
    {
        Debug.Log($"{nameof(Load)} GLB URL: {url}");

        if (string.IsNullOrEmpty(url))
        {
            Debug.LogError($"{nameof(GlbLoader)}.{nameof(Load)} url is empty");
            callback?.Invoke(null);
            return;
        }

        GetVrmData(url, (byte[] avatarData) =>
        {
            Debug.Log($"{nameof(GlbLoader)} {url} loaded from url and created new");

            if (avatarData == null)
            {
                Debug.LogError("Invalid URL: Failed to download VRM file");
                return;
            }

            var glbData = new GlbBinaryParser(avatarData, "_character")
                .Parse();

            if (glbData == null)
            {
                Debug.LogError("Failed to parse VRM file, mesh not a parsable GLB");
                return;
            }

            try
            {
                var vrmData = new VRMData(glbData);

                var context = new VRMImporterContext(vrmData);

                var runtimeGltfInstance = context.Load();
                runtimeGltfInstance.EnableUpdateWhenOffscreen();
                runtimeGltfInstance.ShowMeshes();
                var avatarObject = runtimeGltfInstance.Root;

                glbData.Dispose();
                context.Dispose();

                Debug.Log($"{nameof(GlbLoader)} {nameof(Load)} load from url {url}");
                callback?.Invoke(avatarObject);

            }
            catch (Exception e)
            {
                Debug.Log($"{nameof(GlbLoader)} {nameof(Load)} error loading VRM: {e.Message}");
                System.IO.MemoryStream stream = new System.IO.MemoryStream(avatarData);
                GLTF.Schema.GLTFRoot gLTFRoot;
                GLTF.GLTFParser.ParseJson(stream, out gLTFRoot);

                var options = new ImportOptions()
                {
                    DataLoader = new StreamLoader(stream),
                    AnimationMethod = AnimationMethod.None
                };

                if (importAnimation)
                {
                    options.AnimationMethod = AnimationMethod.MecanimHumanoid;
                    options.AnimationLoopPose = true;
                }

                try
                {
                    UnityGLTF.GLTFSceneImporter sceneImporter = new UnityGLTF.GLTFSceneImporter(gLTFRoot, stream, options);

                    Action<GameObject, ExceptionDispatchInfo> OnImportComplete = (obj, info) =>
                    {
                        Debug.Log($"{nameof(GlbLoader)} {nameof(sceneImporter.LoadScene)} {obj == null}");

                        if (obj == null)
                            throw (info.SourceException);

                        Debug.Log($"{nameof(GlbLoader)} {nameof(Load)} load from url (not vrm) {url}");
                        callback?.Invoke(obj);
                    };

                    sceneImporter.LoadScene(-1, true, OnImportComplete);
                }
                catch (Exception e2)
                {
                    Debug.Log($"{nameof(GlbLoader)} {nameof(Load)} error loading glb {e2.Message}");
                    callback?.Invoke(null);
                }
            }

        });
    }

    public void GetVrmData(string url, Action<byte[]> callback)
    {
        try
        {
            StartCoroutine(DoGetVrmData(url, callback));
        }
        catch (Exception e)
        {
            Debug.LogError($"{nameof(GetVrmData)} Exception Occured {e.Message}");
            callback(null);
        }
    }

    public IEnumerator DoGetVrmData(string url, Action<byte[]> callback)
    {
        var request = UnityWebRequest.Get(url);
        byte[] data;

        yield return request.SendWebRequest();

        switch (request.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.ProtocolError:
            case UnityWebRequest.Result.DataProcessingError:
                Debug.LogError($"{nameof(GlbLoader)}.{nameof(GetVrmData)} - Request error: {request.error} {request.result} {url}");
                data = null;
                break;
            case UnityWebRequest.Result.Success:
                data = request.downloadHandler.data;
                break;
            default:
                Debug.LogError($"{nameof(GlbLoader)}.{nameof(GetVrmData)} - Request error: {request.error}");
                data = null;
                break;
        }

        request.Dispose();
        callback?.Invoke(data);
    }

}