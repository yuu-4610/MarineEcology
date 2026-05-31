using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSceneUI : MonoBehaviour
{
    /*<責務>ゲームシーン内で使用するUIの表示・更新・状態管理を行う。
     */
    [Header("スコアランキングボード(UI)")]
    [SerializeField] GameObject scoreBoard; //スコアランキングボード
    [Header("設定ボード(UI)")]
    [SerializeField] GameObject optionBoard; //設定ボード
    [Header("得点表示テキスト")]
    [SerializeField] TextMeshProUGUI scoreText; //得点反映テキスト
    [Header("スコアランキング用テキスト")]
    [SerializeField] TextMeshProUGUI[] scoreRunkingsText; //スコアランキング反映用テキスト
    [Header("ゲームオーバー後表示マスク")]
    [SerializeField] GameObject gameFinishedMask; //ゲーム終了時の画面マスク
    [Header("ゲーム終了テキスト")]
    [SerializeField] GameObject finishedText; //ゲーム終了を知らせるテキスト
    [Header("ゲームオーバーカウントテキスト")]
    [SerializeField] GameObject timeCountText; //ゲームオーバー前のカウント用テキスト

    [SerializeField] GameObject resultScoreOrderText; //ゲーム終了時のスコア

    [SerializeField] JudgeCountTimer judgeCountTimer;

    private TextMeshProUGUI nearGameOrverCountText; //ゲームオーバー前のカウントダウンテキスト
    private TextMeshProUGUI resultScoreText; //リザルト画面で出すスコアテキスト
    private bool isTimeCountTextDisplay; //タイムカウントテキストの表示有無
    private float finishedTextAlphaTimeCount = 0; //フェードアウトに使用する
    private int zero = 0; //値０


    private void Awake()
    {
        Initialized();
    }

    void Start()
    {
        //UIの非表示かつアクティブ化
        UIManager.Instance.UIActivityAndHidden(optionBoard, false);
        UIManager.Instance.UIActivityAndHidden(scoreBoard, false);
        UIManager.Instance.UIActivityAndHidden(gameFinishedMask, false);
        UIManager.Instance.UIActivityAndHidden(timeCountText, false);
        UIManager.Instance.UIActivityAndHidden(resultScoreOrderText, false);

        //テキストの色を指定
        nearGameOrverCountText.color = new Color(1f, 1f, 1f, 0.6f);
        resultScoreText.color = new Color(1f, 1f, 1f, 1f);
        //テキストを初期化
        resultScoreText.text = zero.ToString();
        scoreText.text = zero.ToString();
        finishedTextAlphaTimeCount = zero;

        StartCoroutine(EventRegistration());
    }
    private void OnDisable()
    {
        EventManager.Instance.transitionGameToResult -= GameFinish;
        EventManager.Instance.addPoint -= PointUpdate;
        EventManager.Instance.optionBoardDisplay -= OptionButtonDisplay;
    }

    // Update is called once per frame
    void Update()
    {
        if (judgeCountTimer.isTimeCount)
        {
            //パズルピースが触れるたびに毎回表示処理をしないように、バッファを設ける（１秒
            if(judgeCountTimer.judgeTimeCount > 1)
            {
                JudgeTimeCountTextDisplay();
            }

            //テキストへの反映
            //Mathf.CeilToInt ー＞ 切り上げ
            if (judgeCountTimer.judgeTimeCount > 0 && judgeCountTimer.judgeTimeCount < judgeCountTimer.countRimit)
            {
                nearGameOrverCountText.text = $"{Mathf.CeilToInt(judgeCountTimer.countRimit - judgeCountTimer.judgeTimeCount)}";
            }
            //タイムカウントが指定の秒数を過ぎたら（6秒
            else if(judgeCountTimer.judgeTimeCount > judgeCountTimer.countRimit)
            {
                JudgeTimeCountTextHidden();
            }
        }
        else
        {
            JudgeTimeCountTextHidden();
        }
    }

    //コンポーネントの取得
    private void Initialized()
    {
        nearGameOrverCountText = timeCountText.GetComponent<TextMeshProUGUI>();
        resultScoreText = resultScoreOrderText.GetComponent<TextMeshProUGUI>();
    }

    public void PointUpdate()
    {
        //得点をテキストに
        scoreText.text = UIManager.Instance.GetPoint().ToString();
    }

    //オプションボタン表示用----------------------------------------------------
    public void OptionButtonDisplay()
    {
        for (int i = 0; i < scoreRunkingsText.Length; ++i)
        {
            scoreRunkingsText[i].text = ScoreSaveDataManager.Instance.scores[i].ToString();
        }

        EventManager.Instance.PlayerMoveLimitEvent();
        UIManager.Instance.UIActivityAndHidden(optionBoard, true);
    }
    public void OptionButtonHidden()
    {
        EventManager.Instance.PlayerMoveLimitCancellationEvent(); //プレイヤーに行動権限を付与
        UIManager.Instance.UIActivityAndHidden(optionBoard, false);
    }

    //スコアランキング表示用-----------------------------------------------------
    public void MyScoreDisplay()
    {
        UIManager.Instance.UIActivityAndHidden(scoreBoard, true);
        UIManager.Instance.UIActivityAndHidden(optionBoard, false);
    }
    public void MyScoreHidden()
    {
        UIManager.Instance.UIActivityAndHidden(scoreBoard, false);
        UIManager.Instance.UIActivityAndHidden(optionBoard, true);
    }
    public void TransitionGameToTitle()
    {
        GameManager.Instance.SceneTransition(SceneName.TitleScene);
    }
    public void ReloadGameScene()
    {
        //再読み込み
        var currentScene = SceneManager.GetActiveScene().name;

        //リトライボタンを押したときに消えずにロードするバグが発生
        //これの対策として、アクティブ化かつ透明化の処理 + バリデーションをする 
        UIManager.Instance.UIActivityAndHidden(gameFinishedMask, false);
        if (gameFinishedMask.GetComponent<CanvasGroup>().alpha != 1.0f)
        {
            if (currentScene == SceneName.GameScene.ToString())
            {
                //EventManager.Instance.PlayerMoveLimitCancellationEvent(); //プレイヤーに行動権限付与
                GameManager.Instance.SceneTransition(SceneName.GameScene);
            }
        }
    }

    private void GameFinish()
    {
        EventManager.Instance.PlayerMoveLimitEvent();
        //ゲーム終了時の処理、「Finish」と黒い幕を降ろす
        UIManager.Instance.UIActivityAndHidden(gameFinishedMask, true);
        AudioManager.Instance.PlaySE(AudioHelper.ToName(AudioFileName.whistle));

        StartCoroutine(GameFinishedText());
    }

    //ゲームオーバーカウント用のテキストに反映
    public void JudgeTimeCountTextDisplay()
    {
        //既に処理済みであれば終了する
        if (isTimeCountTextDisplay) return;
        UIManager.Instance.UIActivityAndHidden(timeCountText, true);

        isTimeCountTextDisplay = true;
    }
    private void JudgeTimeCountTextHidden()
    {
        //既に処理済みであれば終了する
        if (!isTimeCountTextDisplay) return;
        UIManager.Instance.UIActivityAndHidden(timeCountText, false);

        isTimeCountTextDisplay = false;
    }
    //ゲーム終了後の「GameFinish」と得点を表示するテキストの処理
    private IEnumerator GameFinishedText()
    {
        yield return new WaitForSeconds(1f);
        var targetText = finishedText.GetComponent<TextMeshProUGUI>();
        var color = targetText.color;

        //テキストの alpha値を０に近づけていく ー＞ 消えたらスコアを表示
        while(color.a > 0)
        {
            finishedTextAlphaTimeCount = Time.deltaTime * 2;
            color.a -= finishedTextAlphaTimeCount;
            if(color.a < 0)
            {
                color.a = 0;
            }

            targetText.color = color;

            yield return null;
        }

        UIManager.Instance.UIActivityAndHidden(resultScoreOrderText, true);
        //取得した得点を参照する
        resultScoreText.text = UIManager.Instance.GetPoint().ToString();
    }
    //EventManagerが生成されるまで待つ
    private IEnumerator EventRegistration()
    {
        while (EventManager.Instance == null)
        {
            yield return null;
        }
        EventManager.Instance.transitionGameToResult += GameFinish;
        EventManager.Instance.addPoint += PointUpdate;
        EventManager.Instance.optionBoardDisplay += OptionButtonDisplay;
    }
    
}
