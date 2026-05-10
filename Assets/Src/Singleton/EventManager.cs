using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    public event Action pieceObjectFall; //オブジェクトが落ちた時に発火
    public event Action transitionGameToResult; //ゲーム終了時に発火

    public event Action<Transform, Transform, int> synthesisPieceObjectGenerate; //パズルピースが合体した時に発火
    public event Action predictionListUpdate;

    public event Action onPlayerLeftRange;
    public event Action<int> pieceObjectGenerateDecided;

    public event Action scoreLoad;
    public event Action<int> sceneTransition;
    public event Action scoreSave;

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
        pieceObjectFall?.Invoke();
        Debug.Log("処理完了");
    }
    public void TransitionGameToResultEvent()
    {
        transitionGameToResult?.Invoke();
        int count = transitionGameToResult?.GetInvocationList().Length ?? 0;
        Debug.Log($"登録数: {count}");
    }
    public void SynthesisPieceObjectGenerateEvent(Transform pieceTransform, Transform otherPieceTransform, int currentFishPieceTypeNumber)
    {
        synthesisPieceObjectGenerate?.Invoke(pieceTransform, otherPieceTransform, currentFishPieceTypeNumber);
    }
    public void PredictionListUpdateEvent()
    {
        predictionListUpdate?.Invoke();
    }
    public void OnPlayerLeftRangeEvent()
    {
        onPlayerLeftRange?.Invoke();
    }
    public void PieceObjectGenerateDecidedEvent(int generateNumber)
    {
        pieceObjectGenerateDecided?.Invoke(generateNumber);
    }
    public int GetObjectGenerateListenerCount()
    {
        return synthesisPieceObjectGenerate?.GetInvocationList().Length ?? 0;
    }
    //スコアの更新イベント
    public void ScoreLoadEvent()
    {
        scoreLoad?.Invoke();
    }
    //各シーン遷移後に行う処理
    public void SceneTransitionEvent(int sceneType)
    {
        sceneTransition?.Invoke(sceneType);
    }
    public void ScoreSaveEvent()
    {
        scoreSave?.Invoke();
    }
    public void Debuger(Action eventFunction)
    {
        Debug.Log("=== ObjectGenerateEvent 呼び出し元 ===");
        Debug.Log(System.Environment.StackTrace);

        Debug.Log("Invoke フレーム：" + Time.frameCount);
        Debug.Log("登録数: " + eventFunction?.GetInvocationList().Length);
    }
}
