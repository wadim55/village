using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;

public class AssetLoader : MonoBehaviour
{
    [SerializeField] private string url;
    [SerializeField] private Transform point;
    [SerializeField] private AssetBundle _bundle;

    private void Start()
    {
        StartCoroutine(LoadBundleFromServer(url, SaveBundle));
    }

    IEnumerator LoadBundleFromServer(string url, Action<AssetBundle> response)
    {
        var request = UnityWebRequestAssetBundle.GetAssetBundle(url);

        yield return request.SendWebRequest();

        if (!request.isHttpError && !request.isNetworkError)
        {
            response(DownloadHandlerAssetBundle.GetContent(request));
        }
        else
        {
            Debug.LogErrorFormat("error request [{0}, {1}]", url, request.error);

            response(null);
        }

        request.Dispose();
    }

    private void SaveBundle(AssetBundle bundle)
    {
        _bundle = bundle;
        InstantiateObject("Kazuko_Questgiver", point);
    }

    public void InstantiateObject(string name, Transform spawn)
    {
        var prefab = _bundle.LoadAsset(name);
        Instantiate(prefab, spawn.position, spawn.rotation);
    }
}
