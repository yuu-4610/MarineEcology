using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    public event Action PieceObjectDrop; //オブジェクトが落ちた時に発火
    public event Action PieceSyntghesis; //オブジェクト同士がくっついた時に発火
    public event Action PieceObjectGenerate; //オブジェクトを作成したときに発火
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
    public void PieceObjectDropEvent()
    {
        PieceObjectDrop?.Invoke();
        Debug.Log("処理完了");
    }

    public void PiecesynthesisEvent()
    {
        PieceSyntghesis?.Invoke();
    }
    public void PieceObjectGenerateEvent()
    {
        PieceObjectGenerate?.Invoke();
    }
    public void TrantitionGameToResultEvent()
    {
        TrantitionGameToResult?.Invoke();
        int count = TrantitionGameToResult?.GetInvocationList().Length ?? 0;
        Debug.Log($"登録数: {count}");
    }
    public int GetObjectGenerateListenerCount()
    {
        return PieceObjectGenerate?.GetInvocationList().Length ?? 0;
    }
    public void Debuger(Action eventFunction)
    {
        Debug.Log("=== ObjectGenerateEvent 呼び出し元 ===");
        Debug.Log(System.Environment.StackTrace);

        Debug.Log("Invoke フレーム：" + Time.frameCount);
        Debug.Log("登録数: " + eventFunction?.GetInvocationList().Length);
    }
}
