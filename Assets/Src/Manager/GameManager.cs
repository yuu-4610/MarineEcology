using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using TMPro;
using Unity.Android.Gradle.Manifest;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static int sceneNumber; //シーン番号、遷移時に仕様
    public int[] scores { get; private set; } //セーブ下リストのスコア + ゲーム終了時のスコア
    public bool isPlayerControll = false; //プレイヤーの入力制限

    private GameData gameData; //スコア書き込み用変数
    private GameData loadData; //スコア読み取り用変数
    private const int listLength = 3; //Jsonファイルの保存数（処理回数に使用）
    private bool isTrantitionSceneProcess = false; //シーン遷移時に一度だけ行う処理のフラグ

    public bool testFlg; //テスト用 → Pieceオブジェクトの衝突時の処理を OFF にする

    // Start is called before the first frame update
    private void Awake()
    {
        //シングルトン
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        //Titleシーン（一番最初のシーン）で配置したオブジェクトを残す
        DontDestroyOnLoad(this.gameObject);
    }
    private void OnEnable()
    {
        //イベントの登録
        //シングルトンオブジェクトの生成が順不同であるため、生成を待ちイベント登録する
        StartCoroutine(EventRegistration());
    }
    private void OnDisable()
    {
        ObjectEventManager.Instance.TrantitionGameToResult -= GameFinish;
        Debug.Log("ここです");
    }
    void Start()
    {
        //万が一TitleSceneから始まらなかった場合の処理
        var startScene = SceneManager.GetActiveScene().name;
        if(startScene != SceneType.TitleScene.ToString())
        {
            SceneTrantition(SceneType.TitleScene);
            sceneNumber = (int)SceneType.TitleScene;
        }
        //正常に進行したときの処理
        else
        {
            //シーンごとにある処理を有効にする
            isTrantitionSceneProcess = true;
            sceneNumber = (int)SceneType.TitleScene;
        }

    }

    // Update is called once per frame
    void Update()
    {
        switch (sceneNumber)
        {
            case (int)SceneType.TitleScene: //０：タイトルシーン
                if (isTrantitionSceneProcess)
                {
                    //マイスコアランキングの読み取り
                    GameScoreInitialize();

                    //処理の無効化（処理はシーン遷移時に１度のみ）
                    isTrantitionSceneProcess = false;
                }

                break;
            case (int)SceneType.GameScene: //１：ゲームシーン
                if (isTrantitionSceneProcess)
                {
                    //マイスコアランキングの読み取り
                    GameScoreInitialize();
                    //プレイヤーの操作を受け付ける
                    isPlayerControll = true;

                    //処理の無効化（処理はシーン遷移時に１度のみ）
                    isTrantitionSceneProcess = false;
                }
                break;
        }
    }
    public void SceneTrantition(SceneType sceneType)
    {
        //シーン遷移・各シーン開始時の処理を有効化
        SceneManager.LoadScene(sceneType.ToString());
        isTrantitionSceneProcess = true;
        sceneNumber = (int)sceneType;

        //ゲームシーン以外に遷移する場合
        if (sceneNumber != (int)SceneType.GameScene) isPlayerControll = false;
    }
    //スコアランキングの取得（タイトル画面で実行）
    public void GameScoreInitialize()
    {
        gameData = new GameData();
        scores = new int[listLength + 1];

        //データをロード
        loadData = SaveSystem.Load();
        
        //セーブデータが存在しない場合は新しく作る
        if (loadData.myScores == null)
        {
            loadData.myScores = new List<MyScore>();
        }
        if (loadData.myScores.Count == 0)
        {
            for (int i = 0; i < 3; ++i)
            {
                //初期化子の時点で id と myScore が作成される（id：０　myScores：０）
                loadData.myScores.Add(new MyScore());
            }
        }

        for (int i = 0; i < listLength; ++i)
        {
            scores[i] = loadData.myScores[i].myScore;
        }
        
    }
    //GameScene 終了時の処理
    private void GameFinish()
    {
        //PlayableObject の操作を不可能にする
        isPlayerControll = false;
        //マイスコアを更新
        GameScoreSave();
    }
    //スコアの取得とランキング更新
    public void GameScoreSave()
    {
        //最終点数の取得
        var totalPoint = UIManager.Instance.GetPoint();
        if (gameData.myScores == null) gameData.myScores = new List<MyScore>();
        gameData.myScores.Clear();

        scores[listLength] = totalPoint;
        scores = scores.OrderByDescending(x => x).ToArray();
        for (int i = 0; i < listLength; ++i)
        {
            MyScore myScore = new MyScore();
            myScore.id = i + 1;
            myScore.myScore = scores[i];
            gameData.myScores.Add(myScore);
        }
        SaveSystem.Save(gameData);
    }
    //ObjectEventManagerが生成されるまで待つ
    private IEnumerator EventRegistration()
    {
        while(ObjectEventManager.Instance == null)
        {
            yield return null;
        }
        ObjectEventManager.Instance.TrantitionGameToResult += GameFinish;
    }
}
