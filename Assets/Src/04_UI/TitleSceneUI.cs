using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleSceneUI : MonoBehaviour
{
    [Header("ランキング１～３のスコア表示UI")]
    [SerializeField] TextMeshProUGUI[] myScoresText; //マイスコアを反映させるテキスト
    [SerializeField] GameObject scoreBoard; //スコアボード
    [SerializeField] GameObject playGuideBord; //遊び方ボード
    [SerializeField] GameObject audioSettingBoard;
    [SerializeField] GameObject[] playGuidePages; //遊び方説明ボード各ページ
    [SerializeField] GameObject[] playGuideChangePageButton; //ページ変更のボタン ０．次へ　１．前へ　２．閉じる

    private int pageCount;
    // Start is called before the first frame update
    void Start()
    {
        PlayGuideBoardInitialize();
        //スコアボードを非表示に
        UIManager.Instance.UIActivityAndHidden(scoreBoard, false);
        //遊び方説明ボードを非表示に
        UIManager.Instance.UIActivityAndHidden(playGuideBord, false);

        UIManager.Instance.UIActivityAndHidden(audioSettingBoard, false);


        pageCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TransitionTitleToGame()
    {
        GameManager.Instance.SceneTransition(SceneName.GameScene);
    }
    //マイスコア表示
    public void MyScoreDisplay()
    {
        for(int i = 0; i < myScoresText.Length; ++i)
        {
            myScoresText[i].text = ScoreSaveDataManager.Instance.scores[i].ToString();
        }
        //スコアボードを表示
        UIManager.Instance.UIActivityAndHidden(scoreBoard, true);
        //ボタンクリック時の効果音
        AudioManager.Instance.PlaySE(AudioHelper.ToName(AudioFileName.buttonClick));
    }
    //マイスコア非表示
    public void MyScoreHidden()
    {
        UIManager.Instance.UIActivityAndHidden(scoreBoard, false);
    }
    //遊び方説明表示
    public void PlayGuideDisplay()
    {
        UIManager.Instance.UIActivityAndHidden(playGuideBord, true);
        AudioManager.Instance.PlaySE(AudioHelper.ToName(AudioFileName.buttonClick));
    }
    //閉じるボタンを押したときの処理
    //遊び方説明表示非表示
    public void PlayGuideHidden()
    {
        pageCount = 0;
        PlayGuideBoardInitialize();
        UIManager.Instance.UIActivityAndHidden(playGuideBord, false);
    }
    public void PlayGuideNextPage()
    {
        pageCount++;
        //ページをめくる効果音
        AudioManager.Instance.PlaySE(AudioHelper.ToName(AudioFileName.turnThePage));
        //対象ページを表示し、前ページを非表示に
        UIManager.Instance.UIActivityAndHidden(playGuidePages[pageCount], true);
        UIManager.Instance.UIActivityAndHidden(playGuidePages[pageCount - 1], false);
        if (pageCount == playGuidePages.Length - 1)
        {
            //次へボタンを非表示に
            UIManager.Instance.UIActivityAndHidden(playGuideChangePageButton[0], false);
            //閉じるボタンを表示
            UIManager.Instance.UIActivityAndHidden(playGuideChangePageButton[2], true); 
        }
        else if(pageCount != 0)
        {
            //前へボタンを表示
            UIManager.Instance.UIActivityAndHidden(playGuideChangePageButton[1], true);
        }
    }
    public void PlayGuideBackPage()
    {
        pageCount--;
        //ページをめくる効果音
        AudioManager.Instance.PlaySE(AudioHelper.ToName(AudioFileName.turnThePage));
        //対象ページを表示し、前ページを非表示に
        UIManager.Instance.UIActivityAndHidden(playGuidePages[pageCount], true);
        UIManager.Instance.UIActivityAndHidden(playGuidePages[pageCount + 1], false);
        if (pageCount != playGuidePages.Length - 1)
        {
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
        UIManager.Instance.UIActivityAndHidden(playGuidePages[0], true);
        UIManager.Instance.UIActivityAndHidden(playGuidePages[1], false);
        UIManager.Instance.UIActivityAndHidden(playGuidePages[2], false);
    }

    public void AudioSettingBoardDisplay()
    {
        UIManager.Instance.UIActivityAndHidden(audioSettingBoard, true);

        AudioManager.Instance.PlaySE(AudioHelper.ToName(AudioFileName.buttonClick));
    }

    public void AUdioSettingBoardHidden()
    {
        UIManager.Instance.UIActivityAndHidden(audioSettingBoard, false);
    }
}
