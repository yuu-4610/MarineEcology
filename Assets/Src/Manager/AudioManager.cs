using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource bgmSource;
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] AudioMixerGroup seMixerGroup;
    [SerializeField] Slider bgmChangeSlider;
    [SerializeField] Slider seChangeSlider;

    private Dictionary<string, AudioClip> bgmDic, seDic; //Resourcesフォルダから読み込む用
    private List<AudioSource> seSourceArray;
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

        //SE用AudioSourceを複数個作成
        for (int i = 0; i < seSourceNumber; i++)
        {
            this.gameObject.AddComponent<AudioSource>();
        }
        AudioSource[] audioSourceArray = GetComponents<AudioSource>();
        seSourceArray = new List<AudioSource>();

        for(int i = 0; i < audioSourceArray.Length; i++)
        {
            audioSourceArray[i].playOnAwake = false;

            seSourceArray.Add(audioSourceArray[i]);
            seSourceArray[i].outputAudioMixerGroup = seMixerGroup;
        }

        //スライダーに音量値を反映
        audioMixer.GetFloat("BGM", out float bgmValue);
        bgmChangeSlider.value = bgmValue;

        audioMixer.GetFloat("SE", out float seValue);
        seChangeSlider.value = seValue;
    }
    //スライダーにアタッチ
    public void SetBGM(float volume)
    {
        audioMixer.SetFloat("BGM", volume);
    }
    public void SetSE(float volume)
    {
        audioMixer.SetFloat("SE", volume);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void PlaySE(AudioClip audioClip)
    {
        var sourceNumber = 0;
        for (int i = 0; i < seSourceNumber;i++)
        {
            if (!seSourceArray[i].isPlaying)
            {
                sourceNumber = i;
                sePlayCount++;
                break;
            }
            //SE再生枠が上限に達した状態でSEを鳴らしたときの処理
            //１．待機させて処理（非同期
            //２．何もしない
            //else if(sePlayCount == seSourceNumber)
            //{
            //    async Task WaitCount()
            //    {
            //        while(sePlayCount >= 10)
            //        {
            //            await Task.Yield();
            //        }
            //        sourceNumber = i;
            //        sePlayCount++;
            //    }
            //}
        }
        seSourceArray[sourceNumber].PlayOneShot(audioClip);
        sePlayCount--;
    }
    public void PlayBGM(AudioClip audioClip)
    {

    }
}
