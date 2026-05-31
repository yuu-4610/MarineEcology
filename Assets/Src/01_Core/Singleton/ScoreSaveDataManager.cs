using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreSaveDataManager : MonoBehaviour
{
    /*<責務>ハイスコア情報の管理を行い、スコアデータの保持と書き出し処理を担当する。
     */
    public static ScoreSaveDataManager Instance;
    public int[] scores { get; private set; } //セーブ下リストのスコア + ゲーム終了時のスコア
    private const int listLength = 3; //Jsonファイルの保存数（処理回数に使用）

    private GameData gameData; //スコア書き込み用変数
    private GameData loadData; //スコア読み取り用変数


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

        Initialize();
    }
    private void OnEnable()
    {
        StartCoroutine(EventRegistration());
    }
    private void OnDisable()
    {
        EventManager.Instance.scoreLoad -= GameScoreInitialize;
        EventManager.Instance.scoreSave -= GameScoreSave;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Initialize()
    {
        gameData = new GameData();
        scores = new int[listLength + 1];
    }

    //スコアランキングの取得（タイトル画面で実行）
    public void GameScoreInitialize()
    {

        //データをロード
        loadData = SaveSystem.Load();

        //セーブデータが存在しなければ保存するための器を作成する
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

    //スコアの取得とランキング更新
    public void GameScoreSave()
    {
        //最終点数の取得
        var totalPoint = UIManager.Instance.GetPoint();
        //リストの中身が存在する場合は削除し、なければ新しく作る
        if (gameData.myScores == null) gameData.myScores = new List<MyScore>();
        else gameData.myScores.Clear();

        scores[listLength] = totalPoint;
        //降順に並び替え
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

    private IEnumerator EventRegistration()
    {
        while (EventManager.Instance == null)
        {
            yield return null;
        }
        EventManager.Instance.scoreLoad += GameScoreInitialize;
        EventManager.Instance.scoreSave += GameScoreSave;
    }
}
