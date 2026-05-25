using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;
using Color = UnityEngine.Color;

public class UIManager : MonoBehaviour
{
    /*<責務>UIの表示・非表示制御およびゲームシーンで取得したスコア情報の保持を行う。
     */
    public static UIManager Instance;

    private int totalPoint = 0; //総合得点
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
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        
    }
    private void OnDisable()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    //得点を加算
    public void SetPoint(int point)
    {
        totalPoint += point;
        //ポイント加算イベント
        EventManager.Instance.AddPointEvent();
    }
    //得点を取得
    public int GetPoint()
    {
        return totalPoint;
    }

    //設定項目やスコア表示に使用
    public void UIActivityAndHidden(GameObject canvasGroup, bool judge)
    {
        //アクティブかつ非表示
        var scoreBoardCanvas = canvasGroup.GetComponent<CanvasGroup>();
        var alphaValue = (judge) ? 1.0f : 0.0f;
        scoreBoardCanvas.alpha = alphaValue;
        scoreBoardCanvas.interactable = judge;
        scoreBoardCanvas.blocksRaycasts = judge;
    }
}
