using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneUI : MonoBehaviour
{
    [SerializeField] GameObject scoreBoard; //スコアランキングボード
    [SerializeField] GameObject settingBoard; //設定ボード
    [SerializeField] TextMeshProUGUI pointText; //得点反映テキスト
    [SerializeField] TextMeshProUGUI[] scoreRunkingsText; //スコアランキング反映用テキスト
    [SerializeField] GameObject GameFinishedMask; //ゲーム終了時の画面マスク
    [SerializeField] GameObject CountText; //ゲームオーバー前のカウント用テキスト

    private TextMeshProUGUI gameOrverCountText;
    private int zero = 0;
    private float judgementTimeRimit = 5;

    // Start is called before the first frame update
    void Start()
    {
        ObjectManager.Instance.Register("Canvas_GameScene", this.gameObject);
        gameOrverCountText = CountText.GetComponent<TextMeshProUGUI>();

        //UIの非表示かつアクティブ化
        UIManager.Instance.UIActivityAndHidden(settingBoard, false);
        UIManager.Instance.UIActivityAndHidden(scoreBoard, false);
        UIManager.Instance.UIActivityAndHidden(GameFinishedMask, false);
        UIManager.Instance.UIActivityAndHidden(CountText, false);

        //CountText の色を指定
        gameOrverCountText.color = new Color(1f, 1f, 1f, 0.6f);

        ObjectEventManager.Instance.TrantitionGameToResult += GameFinished;
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
        for (int i = 0; i < scoreRunkingsText.Length; ++i)
        {
            scoreRunkingsText[i].text = GameManager.Instance.scores[i].ToString();
        }

        UIManager.Instance.UIActivityAndHidden(settingBoard, true);
        GameManager.Instance.isPlayerControll = false;
    }
    public void OptionButtonHidden()
    {
        UIManager.Instance.UIActivityAndHidden(settingBoard, false);
        GameManager.Instance.isPlayerControll = true;
    }

    //スコアランキング表示用-----------------------------------------------------
    public void MyScoreDisplay()
    {
        UIManager.Instance.UIActivityAndHidden(scoreBoard, true);
    }
    public void MyScoreHidden()
    {
        UIManager.Instance.UIActivityAndHidden(scoreBoard, false);
    }
    public void TrantitionGameToTitle()
    {
        GameManager.Instance.SceneTrantition(SceneType.TitleScene);
    }

    private void GameFinished()
    {
        //ゲーム終了時の処理、「Finish」と黒い幕を降ろす
        UIManager.Instance.UIActivityAndHidden(GameFinishedMask, true);
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
            UIManager.Instance.UIActivityAndHidden(CountText, true);
            //テキストへの反映
            gameOrverCountText.text = $"{Mathf.CeilToInt(judgementTimeRimit - judgementTime)}";
        }
        //カウントリセット時の処理
        else
        {
            //テキストオブジェクトを非表示に
            UIManager.Instance.UIActivityAndHidden(CountText, false);
            //テキストへの反映
            gameOrverCountText.text = zero.ToString();
        }
    }
}
