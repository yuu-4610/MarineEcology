using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleButton : MonoBehaviour
{
    //[SerializeField] GameObject[] TitleSceneButton; //タイトルシーンのボタン
    //[SerializeField] GameObject myScoreBoard; //マイスコア表示
    //[SerializeField] GameObject playGuid; //遊び方表示
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 3; ++i)
        {
            
        }
        //myScoreBoard.SetActive(false);
        //playGuid.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //GameScene へ移動
    public void TrantitionTitleToGame()
    {
        GameManager.Instance.SceneTrantition(SceneType.GameScene);
    }
    //マイスコア表示
    public void MyScoreDisplay()
    {
        //myScoreBoard.SetActive(true);
    }
    //遊び方表示
    public void PlayGuidDisplay()
    {
        //playGuid.SetActive(true);
    }
}
