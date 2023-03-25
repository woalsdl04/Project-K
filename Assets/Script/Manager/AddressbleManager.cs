using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;


public class AddressbleManager : Singleton<AddressbleManager>
{
    private Dictionary<string, Object> _bundleObjs = new Dictionary<string, Object>();

    public void LoadAssetAsync<T>(string assetName) where T : Object //에셋 로드
    {
        _bundleObjs.TryGetValue(assetName, out Object obj);

        if (obj) // 로드가 완료된 에셋
        {
            
        }
        else 
        {
            Addressables.LoadAssetAsync<T>(assetName).Completed += handle =>
            {
                switch (handle.Status)
                {
                    case AsyncOperationStatus.Succeeded:
                        {
                            obj = handle.Result;
                            _bundleObjs.Add(assetName, obj);
                        }
                        break;
                    case AsyncOperationStatus.Failed:

                        break;
                }
            };
        }
    }

    public T InstantiateOrNull<T>(string key, Transform parentTrans = null) where T : Object // 로드된 에셋 생성
    {
        if (_bundleObjs.TryGetValue(key, out Object obj))
        {
            if(obj)
            {
                return Instantiate((T)obj, parentTrans);
            }
        }

        return null;
    }

    public void SetSprite(Sprite sprite, string spriteName)
    {
        Addressables.LoadAssetAsync<Sprite>(spriteName).Completed += handle =>
        {
            sprite = handle.Result;
        };
    }

    public void Start()
    {
        LoadAssetAsync<GameObject>("Box");

    }

    public void CreateBox()
    {
        InstantiateOrNull<GameObject>("Box");
    }
}
