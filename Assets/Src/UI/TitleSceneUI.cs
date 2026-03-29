using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleSceneUI : MonoBehaviour
{
    [Header("ランキング１～３のスコア表示UI")]
    [SerializeField] TextMeshProUGUI[] textsMeshPro; //マイスコアを反映させるテキスト
    [SerializeField] GameObject scoreBoard; //スコアボード
    [SerializeField] GameObject playGuide; //遊び方ボード
    [SerializeField] GameObject[] playGuidePage; //遊び方説明ボード
    [SerializeField] GameObject[] playGuideChangePageButton; //ページ変更のボタン ０．次へ　１．前へ　２．閉じる

    private int pageCount;
    // Start is called before the first frame update
    void Start()
    {
        PlayGuideBoardInitialize();
        //スコアボードを非表示に
        UIManager.Instance.UIActivityAndHidden(scoreBoard, false);
        //遊び方説明ボードを非表示に
        UIManager.Instance.UIActivityAndHidden(playGuide, false);


        pageCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(pageCount);
    }
    public void TrantitionTitleToGame()
    {
        GameManager.Instance.SceneTrantition(SceneType.GameScene);
    }
    //マイスコア表示
    public void MyScoreDisplay()
    {
        for(int i = 0; i < textsMeshPro.Length; ++i)
        {
            Debug.Log(textsMeshPro[i].text);
            Debug.Log(GameManager.Instance.scores[i].ToString());
            textsMeshPro[i].text = GameManager.Instance.scores[i].ToString();
        }
        //スコアボードを表示
        UIManager.Instance.UIActivityAndHidden(scoreBoard, true);
    }
    //マイスコア非表示
    public void MyScoreHidden()
    {
        UIManager.Instance.UIActivityAndHidden(scoreBoard, false);
    }
    //遊び方説明表示
    public void PlayGuideDisplay()
    {
        UIManager.Instance.UIActivityAndHidden(playGuide, true);
    }
    //閉じるボタンを押したときの処理
    //遊び方説明表示非表示
    public void PlayGuideHidden()
    {
        pageCount = 0;
        PlayGuideBoardInitialize();
        UIManager.Instance.UIActivityAndHidden(playGuide, false);
    }
    public void PlayGuideNextPage()
    {
        pageCount++;
        //対象ページを表示し、前ページを非表示に
        UIManager.Instance.UIActivityAndHidden(playGuidePage[pageCount], true);
        UIManager.Instance.UIActivityAndHidden(playGuidePage[pageCount - 1], false);
        if (pageCount == playGuidePage.Length - 1)
        {
            Debug.Log("その１");
            //次へボタンを非表示に
            UIManager.Instance.UIActivityAndHidden(playGuideChangePageButton[0], false);
            //閉じるボタンを表示
            UIManager.Instance.UIActivityAndHidden(playGuideChangePageButton[2], true); 
        }
        else if(pageCount != 0)
        {
            Debug.Log("その２");
            //前へボタンを表示
            UIManager.Instance.UIActivityAndHidden(playGuideChangePageButton[1], true);
        }
    }
    public void PlayGuideBackPage()
    {
        pageCount--;
        //対象ページを表示し、前ページを非表示に
        UIManager.Instance.UIActivityAndHidden(playGuidePage[pageCount], true);
        UIManager.Instance.UIActivityAndHidden(playGuidePage[pageCount + 1], false);
        if (pageCount != playGuidePage.Length - 1)
        {
            Debug.Log("その３");
            //閉じるボタンを非表示に
            UIManager.Instance.UIActivityAndHidden(playGuideChangePageButton[2], false);
            //次へボタンを表示
            UIManager.Instance.UIActivityAndHidden(playGuideChangePageButton[0], true);

            if (pageCount == 0)
            {
                //前へボタンを非表示に
                UIManager.Instance.UIActivityAndHidden(playGuideChangePageButton[1], false);
            }
        }
    }
    //遊び方説明ボードを初期状態にする
    private void PlayGuideBoardInitialize()
    {
        //１ページ目以外を非表示に
        UIManager.Instance.UIActivityAndHidden(playGuideChangePageButton[0], true);
        UIManager.Instance.UIActivityAndHidden(playGuideChangePageButton[1], false);
        UIManager.Instance.UIActivityAndHidden(playGuideChangePageButton[2], false);
        //次へ以外を非表示に
        UIManager.Instance.UIActivityAndHidden(playGuidePage[0], true);
        UIManager.Instance.UIActivityAndHidden(playGuidePage[1], false);
        UIManager.Instance.UIActivityAndHidden(playGuidePage[2], false);
    }
}
