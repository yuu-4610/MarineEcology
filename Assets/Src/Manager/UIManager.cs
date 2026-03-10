using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;
using Color = UnityEngine.Color;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    //public bool isSettingOpen { get; private set; } //取得用
    private GameObject optionObject;

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
    }
    //得点を取得
    public int GetPoint()
    {
        return totalPoint;
    }

    public bool IsSettingOen()
    {
        //isSettingOpen = !isSettingOpen;

        //return isSettingOpen;
        return true;
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
