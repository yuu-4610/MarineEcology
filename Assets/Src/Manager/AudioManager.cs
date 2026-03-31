using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] AudioSource bgmSource; //基本１つのみ再生するので、柔軟性より管理しやすさをとるため、BGMを流す AudioSource はアタッチする
    [SerializeField] AudioMixer audioMixer; //オーディオミキサー
    [SerializeField] AudioMixerGroup bgmMixerGroup; //BGM音量管理グループ名
    [SerializeField] AudioMixerGroup seMixerGroup; //SE音量管理グループ名
    [SerializeField] Slider bgmChangeSlider; //BGMの音量調整用スライダー
    [SerializeField] Slider seChangeSlider; //SEの音量調整用スライダー

    private Dictionary<string, AudioClip> bgmDic, seDic; //
    private List<AudioSource> seSourceArray; //SE用 AudioSource のリスト
    private const int seSourceNumber = 10;
    private int sePlayCount = 0;
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
    // Start is called before the first frame update
    void Start()
    {
        sePlayCount = 0;
        //BGM用AudioSourceに初期BGMとループ処理
        bgmSource.outputAudioMixerGroup = bgmMixerGroup;
        bgmSource.loop = true;
        //PlayBGM();

        //SE用AudioSourceを 10 個このオブジェクトに追加する
        for (int i = 0; i < seSourceNumber; i++)
        {
            var addObject =  this.gameObject.AddComponent<AudioSource>();
            //新たに追加したコンポーネントに ミキサーのグループ"SE" を割り当てる
            addObject.outputAudioMixerGroup = seMixerGroup;
        }
        //ここでは全ての AudioSource を取得
        AudioSource[] audioSourceArray = GetComponents<AudioSource>();
        seSourceArray = new List<AudioSource>();

        for (int i = 0; i < audioSourceArray.Length; i++)
        {
            audioSourceArray[i].playOnAwake = false;

            if (audioSourceArray[i].outputAudioMixerGroup == seMixerGroup){
                seSourceArray.Add(audioSourceArray[i]);
            }
        }

        //スライダーに音量値を反映
        audioMixer.GetFloat(AudioMixerGroupName.BGM.ToString(), out float bgmValue);
        bgmChangeSlider.value = bgmValue;

        audioMixer.GetFloat(AudioMixerGroupName.SE.ToString(), out float seValue);
        seChangeSlider.value = seValue;

        //登録リストの作成
        bgmDic = new Dictionary<string, AudioClip>();
        seDic = new Dictionary<string, AudioClip>();

        //Resourcesフォルダに格納しているBGM, SEの音源素材をリストに追加
        object[] bgmList = Resources.LoadAll(PathHelper.ToName(ResourcePath.BGM));
        object[] seList = Resources.LoadAll(PathHelper.ToName(ResourcePath.SE));

        //上記で作成したリストを登録リスト(Dictionary)に登録
        foreach (AudioClip bgm in bgmList)
        {
            bgmDic[bgm.name] = bgm;
        }
        foreach (AudioClip se in seList)
        {
            seDic[se.name] = se;
        }
    }
    //スライダーにアタッチ
    public void SetBGMVolume(float volume)
    {
        audioMixer.SetFloat("BGM", volume);
    }
    public void SetSEVolume(float volume)
    {
        audioMixer.SetFloat("SE", volume);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void PlaySE(string audioClip)
    {
        //指定したクリップ名がない場合は再生せず終了する
        if (!seDic.ContainsKey(audioClip)) return;

        foreach (AudioSource seSource in seSourceArray)
        {
            //作成した AudioSource から再生していないものを探す
            //空いている AudioSource があれば再生
            if (!seSource.isPlaying)
            {
                seSource.PlayOneShot(seDic[audioClip] as AudioClip);
                return;
            }
        }
    }
    public void PlayBGM(AudioClip audioClip)
    {
        //指定したクリップ名がない場合は再生せず終了する
        if (!bgmDic.ContainsKey(audioClip.name)) return;
        bgmSource.clip = audioClip;
        bgmSource.Play();
    }
}
