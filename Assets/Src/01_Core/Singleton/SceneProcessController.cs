using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneProcessController : MonoBehaviour
{
    /*<責務>シーン遷移時に処理を行う
     */
    public static SceneProcessController Instance;
    public int sceneType { get; private set; }
    public bool hasEvent { get; private set; } = false;

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

        StartCoroutine(EventRegistration());
    }

    private void OnEnable()
    {
        
    }
    private void OnDestroy()
    {
        EventManager.Instance.sceneTransition -= SceneProcess;
    }

    //シーン遷移時に行う処理
    private void SceneProcess(int sceneType)
    {
        switch (sceneType)
        {
            case (int)SceneName.TitleScene: //０：タイトルシーン
                //マイスコアランキングの読み取り
                EventManager.Instance.ScoreLoadEvent();

                //BGMの再生
                //StartCoroutine(WaitPlayBGM(AudioFileName.kaityusekai));
                WaitPlay(AudioFileName.kaityusekai);

                break;
            case (int)SceneName.GameScene: //１：ゲームシーン
                EventManager.Instance.ScoreLoadEvent(); //マイスコアランキングの読み取り

                //指定のBGMでなければ変更 -> リトライしたときにBGMが途切れないよう
                Debug.Log(AudioHelper.ToName(AudioFileName.tokonatunoumi));
                if (AudioManager.Instance.bgmSource.clip.name != AudioHelper.ToName(AudioFileName.tokonatunoumi))
                {
                    //StartCoroutine(WaitPlayBGM(AudioFileName.tokonatunoumi));
                    WaitPlay(AudioFileName.tokonatunoumi);
                }

                break;

            default: Debug.Log("存在しないシーン");
                break;
        }
    }

    private IEnumerator WaitPlayBGM(AudioFileName audioFileName)
    {
        yield return null;
        AudioManager.Instance.PlayBGM(AudioHelper.ToName(audioFileName));
    }
    private void WaitPlay(AudioFileName audioFileName)
    {
        AudioManager.Instance.PlayBGM(AudioHelper.ToName(audioFileName));
    }

    private IEnumerator EventRegistration()
    {
        while (EventManager.Instance == null)
        {
            yield return null;
        }
        EventManager.Instance.sceneTransition += SceneProcess;
        Debug.Log("準備できた");
        hasEvent = true;
    }
}
