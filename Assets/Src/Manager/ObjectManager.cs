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
        DontDestroyOnLoad(this.gameObject);
    }
    public GameObject GetOrCreate(string key, GameObject prefab)
    {
        //objects を見て既に登録されているオブジェジェクトであればそのまま返す
        //登録されていなければ第2引数の参照を元に生成 + 返す
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
    //使用理由
    //１．ヒエラルキー依存の減少
    // →Find で探すことによる起きる問題が発生しない
    //２．アタッチミスによる起きるエラーを解決
    // →ScriptableObject に登録したオブジェクトを指定することでアタッチ作業で起こりえる問題を削減する
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
