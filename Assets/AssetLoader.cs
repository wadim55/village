using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class AssetLoader : MonoBehaviour
{
    
    
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
}
