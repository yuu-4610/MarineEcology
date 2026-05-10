using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneProcessController : MonoBehaviour
{
    /*<責務>シーン遷移後に処理を行う
     *
     */
    public int sceneType { get; private set; }
    public static SceneProcessController Instance;

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
        StartCoroutine(EventRegistration());
    }
    private void OnDestroy()
    {
        EventManager.Instance.sceneTransition -= SceneProcess;
    }

    private void SceneProcess(int sceneType)
    {
        switch (sceneType)
        {
            case (int)SceneName.TitleScene: //０：タイトルシーン
                //マイスコアランキングの読み取り
                EventManager.Instance.ScoreLoadEvent();

                Debug.Log($"AudioManager.Instance.bgmSource.clip.name{AudioManager.Instance.bgmSource.clip.name}");
                //BGMの再生
                AudioManager.Instance.PlayBGM(AudioHelper.ToName(AudioFileName.kaityusekai));

                break;
            case (int)SceneName.GameScene: //１：ゲームシーン
                EventManager.Instance.ScoreLoadEvent(); //マイスコアランキングの読み取り

                //指定のBGMでなければ変更 ー＞ リトライしたときにBGMが途切れないよう

                Debug.Log($"AudioManager.Instance.bgmSource.clip.name{AudioManager.Instance.bgmSource.clip.name}");
                if (AudioManager.Instance.bgmSource.clip.name != AudioHelper.ToName(AudioFileName.tokonatunoumi))
                {
                    AudioManager.Instance.PlayBGM(AudioHelper.ToName(AudioFileName.tokonatunoumi));
                }

                //通ってはいる
                //プレイヤーの操作を受け付ける

                break;

            default: Debug.Log("存在しないシーン");
                break;
        }
    }
    private IEnumerator EventRegistration()
    {
        while (EventManager.Instance == null)
        {
            yield return null;
        }
        EventManager.Instance.sceneTransition += SceneProcess;
    }
}
