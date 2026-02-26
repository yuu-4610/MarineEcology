using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Unity.Android.Gradle.Manifest;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static int sceneNumber; //シーン番号、遷移時に仕様

    private GameData gameData; //スコア書き込み用変数
    private GameData loadData; //スコア読み取り用変数
    private int[] scores; //セーブ下リストのスコア + ゲーム終了時のスコア
    private const int listLength = 3; //Jsonファイルの保存数（処理回数に使用）
    private bool isTitleSceneProcess = false; //タイトルシーン遷移時の処理制限

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
    }
    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        ObjectEventManager.Instance.TrantitionGameToResult -= GameFinish;
    }
    void Start()
    {
        ObjectEventManager.Instance.TrantitionGameToResult += GameFinish;
        //SceneTrantition(SceneType.TitleScene);
        GameScoreInitialize();
    }

    // Update is called once per frame
    void Update()
    {
        switch (sceneNumber)
        {
            case 0: //タイトルシーン
                GameScoreInitialize();

                isTitleSceneProcess = false;
                break;
            case 1: //ゲームシーン
                break;
            case 2:
                break;
        }
    }
    public void SceneTrantition(SceneType sceneType)
    {
        SceneManager.LoadScene(sceneType.ToString());
        sceneNumber = (int)sceneType;
        if(sceneNumber == (int)SceneType.TitleScene)
        {
            isTitleSceneProcess = true;
        }
    }
    //スコアランキングの取得（タイトル画面で実行）
    public void GameScoreInitialize()
    {
        if (!isTitleSceneProcess) return;

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
    public void GameFinish()
    {
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
}
