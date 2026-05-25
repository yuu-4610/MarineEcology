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
    /*<責務>ゲームの進行度の
     */
    public static GameManager Instance;
    public static int sceneNumber; //シーン番号、遷移時に仕様

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

        //イベントの登録
        //シングルトンオブジェクトの生成が順不同であるため、生成を待ちイベント登録する
        StartCoroutine(EventRegistration());
    }
    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        EventManager.Instance.transitionGameToResult -= GameFinish;
    }
    void Start()
    {
        //万が一TitleSceneから始まらなかった場合の処理
        var startScene = SceneManager.GetActiveScene().name;
        SceneTransition(SceneName.TitleScene);
        sceneNumber = (int)SceneName.TitleScene;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    public void SceneTransition(SceneName sceneType)
    {
        sceneNumber = (int)sceneType;
        //シーン遷移・各シーン開始時の処理を有効化
        /*現在がタイトルシーンではないかつ、指定シーンがタイトルシーンでなければ処理
         *->タイトルシーンにいるのにタイトルシーンに遷移処理をしたくない
         */
        if(sceneType == SceneName.TitleScene)
        {
            if (SceneManager.GetActiveScene().name == SceneName.TitleScene.ToString())
            {
                StartCoroutine(SceneProcessMethodCall(sceneNumber));
                return;
            }
            else
            {
                SceneManager.LoadScene(sceneType.ToString());
                StartCoroutine(SceneProcessMethodCall(sceneNumber));
            }
        }
        else if(sceneType == SceneName.GameScene)
        {
            SceneManager.LoadScene(sceneType.ToString());
            StartCoroutine(SceneProcessMethodCall(sceneNumber));
        }
        
    }
    
    //GameScene 終了時の処理
    private void GameFinish()
    {
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

    private IEnumerator SceneProcessMethodCall(int sceneType)
    {
        while (SceneProcessController.Instance == null || !SceneProcessController.Instance.hasEvent)
        {
            yield return null;
        }
        //シーン遷移時に処理
        EventManager.Instance.SceneTransitionEvent(sceneType);
        Debug.Log("呼んだ");
    }
}
