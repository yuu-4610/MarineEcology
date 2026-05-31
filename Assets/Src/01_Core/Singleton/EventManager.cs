using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    /*<責務>ゲーム全体で使用する処理イベントの提供
     */
    public static EventManager Instance { get; private set; }

    public event Action dropPiece; //オブジェクトを落とした時に発火
    public event Action optionBoardDisplay; //Shiftキー押下次に発火
    public event Action fallPiece;
    public event Action transitionGameToResult; //ゲーム終了判定時に発火

    public event Action<Transform, Transform, int> synthesisPieceObjectGenerate; //パズルピースが合体した時に発火

    public event Action<int> pieceObjectGenerateDecided; //パズルピースが生成可能になったときに発火（予測リストからの番号提供時）

    public event Action scoreLoad; //スコア値をJSONファイルから書き出す際に発火
    public event Action scoreSave; //スコア値をJSONファイルに書き込む際に発火
    public event Action<int> sceneTransition; //シーン遷移命令時に発火

    public event Action judgeTimeCountStart; //判定ラインが判定を開始したときに発火
    public event Action judgeTimeCountReset; //判定ラインが判定を終了したときに発火

    public event Action addPoint; //ポイント加算時に発火

    public event Action playerMoveLimit; //ゲームシーン以外に遷移、オプションボード表示時、ゲーム終了時に発火
    public event Action playerMoveLimitCancellation; //オプションボード非表示時に発火


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

    //プレイヤーのSpaceキー押下によるパズルピースの落下イベント
    public void DropPieceEvent()
    {
        dropPiece?.Invoke();
    }

    //ゲームシーンからタイトルシーンへの遷移イベント
    public void TransitionGameToResultEvent()
    {
        transitionGameToResult?.Invoke();
    }

    //パズルピース同士の衝突イベント
    public void SynthesisPieceObjectGenerateEvent(Transform pieceTransform, Transform otherPieceTransform, int currentFishPieceTypeNumber)
    {
        synthesisPieceObjectGenerate?.Invoke(pieceTransform, otherPieceTransform, currentFishPieceTypeNumber);
    }

    //プレイヤーを追従するオブジェクトの生成イベント
    public void PieceObjectGenerateDecidedEvent(int generateNumber)
    {
        pieceObjectGenerateDecided?.Invoke(generateNumber);
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

    //ゲームオーバー前のタイムカウントを開始する処理
    public void JudgeTimeCountStartEvent()
    {
        judgeTimeCountStart?.Invoke();
    }

    //ゲームオーバー前のタイムカウントを終了する処理
    public void JudgeTimeCountResetEvent()
    {
        judgeTimeCountReset?.Invoke();
    }

    //オプションボードを開く
    public void OptionBoardDisplayEvent()
    {
        optionBoardDisplay?.Invoke();
    }

    public void FallPieceEvent()
    {
        fallPiece?.Invoke();
    }

    //ポイント加算イベント
    public void AddPointEvent()
    {
        addPoint?.Invoke();
    }

    public void PlayerMoveLimitEvent()
    {
        playerMoveLimit?.Invoke();
    }
    public void PlayerMoveLimitCancellationEvent()
    {
        playerMoveLimitCancellation?.Invoke();
    }
    public void Debuger(Action eventFunction)
    {
        Debug.Log("=== ObjectGenerateEvent 呼び出し元 ===");
        Debug.Log(System.Environment.StackTrace);

        Debug.Log("Invoke フレーム：" + Time.frameCount);
        Debug.Log("登録数: " + eventFunction?.GetInvocationList().Length);
    }
}
