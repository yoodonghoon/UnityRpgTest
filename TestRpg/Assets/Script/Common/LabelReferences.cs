using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public class LabelReferences : MonoBehaviour
{
    // 어드레서블의 Label을 얻어올 수 있는 필드.
    public AssetLabelReference assetLabel;
    // 경로 캐싱.
    private IList<IResourceLocation> _locations;
    // 생성된 게임오브젝트를 Destroy하기 위해 참조값을 캐싱한다.
    private List<GameObject> _gameObjects = new List<GameObject>();

    public void GetLocations()
    {
        // 해당 번들 파일이 이미 캐시 파일로 존재하면 '0'을 반환한다.
        Addressables.GetDownloadSizeAsync(assetLabel.labelString).Completed +=
            (handle) =>
            {
                Debug.Log("size : " + handle.Result);
            };
        // 빌드타겟의 경로를 가져온다.
        // 경로이기 때문에 메모리에 에셋이 로드되진 않는다.
        Addressables.LoadResourceLocationsAsync(assetLabel.labelString).Completed +=
            (handle) =>
            {
                _locations = handle.Result;
            };
    }

    public void Instantiate()
    {
        var location = _locations[Random.Range(0, _locations.Count)];

        // 경로를 인자로 GameObject를 생성한다.
        // 메모리에 GameObject가 로드된다.
        Addressables.InstantiateAsync(location, Vector3.one, Quaternion.identity).Completed +=
            (handle) =>
            {
                // 생성된 개체의 참조값 캐싱
                _gameObjects.Add(handle.Result);
            };
    }

    public void Destroy()
    {
        if (_gameObjects.Count == 0)
            return;

        var index = _gameObjects.Count - 1;
        // InstantiateAsync <-> ReleaseInstance
        // ref count가 0이면 메모리에 GameObject가 언로드된다.
        Addressables.ReleaseInstance(_gameObjects[index]);
        _gameObjects.RemoveAt(index);
    }
}