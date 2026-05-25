using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgeCountTimer : MonoBehaviour
{
    /*<責務>判定開始時に時間を計測する
     *複数オブジェクトから利用するため MonoBehaviour を継承している。
     *時間計測を行う側へコンポーネントを付与することで、責務と依存関係を明確化している。
     */
    public float judgeTimeCount { get; private set; } //判定時の時間測定
    public int countRimit { get; private set; } = 6;
    public bool isTimeCount { get; private set; } = false;

    private bool isFinished = false;
    // Start is called before the first frame update
    void Start()
    {
        judgeTimeCount = 0;

        EventManager.Instance.judgeTimeCountStart += IsJudgeTimeCount;
        EventManager.Instance.judgeTimeCountReset += IsCountReset;
    }

    private void OnDisable()
    {
        EventManager.Instance.judgeTimeCountStart -= IsJudgeTimeCount;
        EventManager.Instance.judgeTimeCountReset -= IsCountReset;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTimeCount)
        {
            judgeTimeCount += Time.deltaTime;

            if (judgeTimeCount > countRimit)
            {
                if (!isFinished)
                {
                    //ゲーム終了時のイベントを発火
                    EventManager.Instance.TransitionGameToResultEvent();
                    isFinished = true;
                }
            }
        }
        else
        {
            judgeTimeCount = 0;
        }
    }

    private void IsJudgeTimeCount()
    {
        isTimeCount = true;
    }

    private void IsCountReset()
    {
        isTimeCount = false;
    }
}
