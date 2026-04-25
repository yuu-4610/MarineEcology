using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSceneUI : MonoBehaviour
{
    [SerializeField] GameObject scoreBoard; //スコアランキングボード
    [SerializeField] GameObject optiongBoard; //設定ボード
    [SerializeField] TextMeshProUGUI pointText; //得点反映テキスト
    [SerializeField] TextMeshProUGUI[] scoreRunkingsText; //スコアランキング反映用テキスト
    [SerializeField] GameObject gameFinishedMask; //ゲーム終了時の画面マスク
    [SerializeField] GameObject finishedText;
    [SerializeField] GameObject countText; //ゲームオーバー前のカウント用テキスト
    [SerializeField] GameObject resultScoreOrderText; //ゲーム終了時のスコア

    private TextMeshProUGUI gameOrverCountText;
    private TextMeshProUGUI resultScoreText;
    private int zero = 0;
    private float judgementTimeRimit = 5;
    private float textAlphaValueCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        //参照可能オブジェクトとして登録
        ObjectManager.Instance.Register(ReferenceObjectName.Canvas_GameScene.ToString(), this.gameObject);

        //コンポーネントの取得
        gameOrverCountText = countText.GetComponent<TextMeshProUGUI>();
        resultScoreText = resultScoreOrderText.GetComponent<TextMeshProUGUI>();

        //UIの非表示かつアクティブ化
        UIManager.Instance.UIActivityAndHidden(optiongBoard, false);
        UIManager.Instance.UIActivityAndHidden(scoreBoard, false);
        UIManager.Instance.UIActivityAndHidden(gameFinishedMask, false);
        UIManager.Instance.UIActivityAndHidden(countText, false);
        UIManager.Instance.UIActivityAndHidden(resultScoreOrderText, false);

        //テキストの色を指定
        gameOrverCountText.color = new Color(1f, 1f, 1f, 0.6f);
        resultScoreText.color = new Color(1f, 1f, 1f, 1f);
        //テキストを初期化
        resultScoreText.text = zero.ToString();
        pointText.text = zero.ToString();
        textAlphaValueCount = 0;

        StartCoroutine(EventRegistration());
    }
    private void OnDisable()
    {
        ObjectEventManager.Instance.TrantitionGameToResult -= GameFinished;
    }

    // Update is called once per frame
    void Update()
    {
        AddPoint();
    }
    public void AddPoint()
    {
        //得点をテキストに
        pointText.text = UIManager.Instance.GetPoint().ToString();
    }

    //オプションボタン表示用----------------------------------------------------
    public void OptionButtonDisplay()
    {
        Debug.Log("１");
        for (int i = 0; i < scoreRunkingsText.Length; ++i)
        {
            scoreRunkingsText[i].text = GameManager.Instance.scores[i].ToString();
        }
        Debug.Log("２");

        UIManager.Instance.UIActivityAndHidden(optiongBoard, true);
        GameManager.Instance.isPlayerControll = false;
    }
    public void OptionButtonHidden()
    {
        UIManager.Instance.UIActivityAndHidden(optiongBoard, false);
        GameManager.Instance.isPlayerControll = true;
    }

    //スコアランキング表示用-----------------------------------------------------
    public void MyScoreDisplay()
    {
        UIManager.Instance.UIActivityAndHidden(scoreBoard, true);
        UIManager.Instance.UIActivityAndHidden(optiongBoard, false);
    }
    public void MyScoreHidden()
    {
        UIManager.Instance.UIActivityAndHidden(scoreBoard, false);
        UIManager.Instance.UIActivityAndHidden(optiongBoard, true);
    }
    public void TrantitionGameToTitle()
    {
        GameManager.Instance.SceneTrantition(SceneType.TitleScene);
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
            if (currentScene == SceneType.GameScene.ToString())
            {
                GameManager.Instance.SceneTrantition(SceneType.GameScene);
            }
        }
    }

    private void GameFinished()
    {
        //ゲーム終了時の処理、「Finish」と黒い幕を降ろす
        UIManager.Instance.UIActivityAndHidden(gameFinishedMask, true);
        AudioManager.Instance.PlaySE(AudioHelper.ToName(AudioFileName.whistle));

        StartCoroutine(GameFinishedText());
    }
    //ゲームオーバーカウント用のテキストに反映
    public void IsNearGameOver(float isTime)
    {
        //判定時間は以下からとする「経った時間 - 1秒」
        var judgementTime = isTime - 1.0f;

        //カウント中の処理
        if(judgementTime > 0 && judgementTime < judgementTimeRimit)
        {
            //テキストオブジェクトを表示する
            UIManager.Instance.UIActivityAndHidden(countText, true);
            //テキストへの反映
            //Mathf.CeilToInt ー＞ 切り上げ
            gameOrverCountText.text = $"{Mathf.CeilToInt(judgementTimeRimit - judgementTime)}";
        }
        //カウントリセット時の処理
        else
        {
            //テキストへの反映
            gameOrverCountText.text = zero.ToString();
            //テキストオブジェクトを非表示に
            UIManager.Instance.UIActivityAndHidden(countText, false);
        }
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
            textAlphaValueCount = Time.deltaTime * 2;
            color.a -= textAlphaValueCount;
            if(color.a < 0)
            {
                color.a = 0;
            }

            //Debug.Log($"finishedTextObjectColor{finishedTextObjectColor.a}");
            targetText.color = color;

            yield return null;
        }

        //最終的な取得スコアを表示
        UIManager.Instance.UIActivityAndHidden(resultScoreOrderText, true);
        resultScoreText.text = UIManager.Instance.GetPoint().ToString();
    }
    //ObjectEventManagerが生成されるまで待つ
    private IEnumerator EventRegistration()
    {
        while (ObjectEventManager.Instance == null)
        {
            yield return null;
        }
        ObjectEventManager.Instance.TrantitionGameToResult += GameFinished;
    }
}
