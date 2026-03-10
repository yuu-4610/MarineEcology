using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleSceneUI : MonoBehaviour
{
    [Header("ランキング１～３のスコア表示UI")]
    [SerializeField] TextMeshProUGUI[] textsMeshPro;
    [SerializeField] GameObject scoreBoard;
    // Start is called before the first frame update
    void Start()
    {
        //スコアボードを非表示に
        MyScoreHidden();
    }

    // Update is called once per frame
    void Update()
    {
        
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
    public void MyScoreHidden()
    {
        UIManager.Instance.UIActivityAndHidden(scoreBoard, false);
    }
    //遊び方表示
    public void PlayGuidDisplay()
    {
        //playGuid.SetActive(true);
    }
}
