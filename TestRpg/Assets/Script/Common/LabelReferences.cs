using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public class LabelReferences : MonoBehaviour
{
    // ��巹������ Label�� ���� �� �ִ� �ʵ�.
    public AssetLabelReference assetLabel;
    // ��� ĳ��.
    private IList<IResourceLocation> _locations;
    // ������ ���ӿ�����Ʈ�� Destroy�ϱ� ���� �������� ĳ���Ѵ�.
    private List<GameObject> _gameObjects = new List<GameObject>();

    public void GetLocations()
    {
        // �ش� ���� ������ �̹� ĳ�� ���Ϸ� �����ϸ� '0'�� ��ȯ�Ѵ�.
        Addressables.GetDownloadSizeAsync(assetLabel.labelString).Completed +=
            (handle) =>
            {
                Debug.Log("size : " + handle.Result);
            };
        // ����Ÿ���� ��θ� �����´�.
        // ����̱� ������ �޸𸮿� ������ �ε���� �ʴ´�.
        Addressables.LoadResourceLocationsAsync(assetLabel.labelString).Completed +=
            (handle) =>
            {
                _locations = handle.Result;
            };
    }

    public void Instantiate()
    {
        var location = _locations[Random.Range(0, _locations.Count)];

        // ��θ� ���ڷ� GameObject�� �����Ѵ�.
        // �޸𸮿� GameObject�� �ε�ȴ�.
        Addressables.InstantiateAsync(location, Vector3.one, Quaternion.identity).Completed +=
            (handle) =>
            {
                // ������ ��ü�� ������ ĳ��
                _gameObjects.Add(handle.Result);
            };
    }

    public void Destroy()
    {
        if (_gameObjects.Count == 0)
            return;

        var index = _gameObjects.Count - 1;
        // InstantiateAsync <-> ReleaseInstance
        // ref count�� 0�̸� �޸𸮿� GameObject�� ��ε�ȴ�.
        Addressables.ReleaseInstance(_gameObjects[index]);
        _gameObjects.RemoveAt(index);
    }
}