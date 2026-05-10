using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static int sceneNumber; //シーン番号、遷移時に仕様
    public int[] scores { get; private set; } //セーブ下リストのスコア + ゲーム終了時のスコア
    //public bool isPlayerControll = false; //プレイヤーの入力制限

    //public bool testFlg; //テスト用 → Pieceオブジェクトの衝突時の処理を OFF にする

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
        EventManager.Instance.transitionGameToResult -= GameFinish;
    }
    void Start()
    {
        //万が一TitleSceneから始まらなかった場合の処理
        var startScene = SceneManager.GetActiveScene().name;
        if(startScene != SceneName.TitleScene.ToString())
        {
            SceneTransition(SceneName.TitleScene);
            sceneNumber = (int)SceneName.TitleScene;
        }
        //正常に進行したときの処理
        else
        {
            sceneNumber = (int)SceneName.TitleScene;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SceneTransition(SceneName sceneType)
    {
        sceneNumber = (int)sceneType;
        //シーン遷移・各シーン開始時の処理を有効化
        SceneManager.LoadScene(sceneType.ToString());
        //シーン遷移時に処理
        EventManager.Instance.SceneTransitionEvent((int)sceneType);

        //ゲームシーン以外に遷移する場合
        if (sceneNumber != (int)SceneName.GameScene)
        {
            
        }
    }
    
    //GameScene 終了時の処理
    private void GameFinish()
    {
        //PlayableObject の操作を不可能にする
        
        //マイスコアを更新
        EventManager.Instance.ScoreSaveEvent();
    }

    //EventManagerが生成されるまで待つ
    private IEnumerator EventRegistration()
    {
        while(EventManager.Instance == null)
        {
            yield return null;
        }
        EventManager.Instance.transitionGameToResult += GameFinish;
    }
}
