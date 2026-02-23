using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager Instance;
    private Dictionary<string, GameObject> objects = new();

    private void Awake()
    {
        //シングルトン
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }
    public GameObject GetOrCreate(string key, GameObject prefab)
    {
        if (!objects.TryGetValue(key, out var obj) || obj == null)
        {
            obj = Instantiate(prefab);
            obj.name = key;
            objects[key] = obj;
        }
        return obj;
    }
    // Start is called before the first frame update
    //オブジェクトのコンポーネント参照をする
    public T Get<T>(string key, GameObject prefab) where T : Component
    {
        var objct = GetOrCreate(key, prefab);
        return objct.GetComponent<T>();
    }
    public void Register(string key, GameObject obj)
    {
        objects[key] = obj;
    }
}
