using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;
using Color = UnityEngine.Color;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public bool isSettingOpen { get; private set; } //取得用
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

    public void IsSettingOen()
    {
        isSettingOpen = !isSettingOpen;
    }
}
