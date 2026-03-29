using System;
using System.Collections;
using UnityEngine;

public class ObjectEventManager : MonoBehaviour
{
    public static ObjectEventManager Instance {  get; private set; }

    public event Action ObjectDrop; //オブジェクトが落ちた時に発火
    public event Action PieceSyntghesis; //オブジェクト同士がくっついた時に発火
    public event Action ObjectGenerate; //オブジェクトを作成したときに発火
    public event Action TrantitionGameToResult; //ゲーム終了時に発火

    // Use this for initialization
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ObjectDropEvent()
    {
        ObjectDrop?.Invoke();
    }

    public void PiecesynthesisEvent()
    {
        PieceSyntghesis?.Invoke();
    }
    public void ObjectGenerateEvent()
    {
        ObjectGenerate?.Invoke();
    }
    public void TrantitionGameToResultEvent()
    {
        TrantitionGameToResult?.Invoke();
    }
    public int GetObjectGenerateListenerCount()
    {
        return ObjectGenerate?.GetInvocationList().Length ?? 0;
    }
    public void Debuger(Action eventFunction)
    {
        Debug.Log("=== ObjectGenerateEvent 呼び出し元 ===");
        Debug.Log(System.Environment.StackTrace);

        Debug.Log("Invoke フレーム：" + Time.frameCount);
        Debug.Log("登録数: " + eventFunction?.GetInvocationList().Length);
    }
}