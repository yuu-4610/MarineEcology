using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static int sceneNumber; //シーン番号、遷移時に仕様

    private MyScore myScore;
    SceneType sceneType = 0;

    private static string path => Application.persistentDataPath + "";
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
        myScore = new MyScore();
        //SceneTrantition(SceneType.TitleScene);
    }

    // Update is called once per frame
    void Update()
    {
        switch (sceneNumber)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
        }
    }
    public void SceneTrantition(SceneType sceneType)
    {
        SceneManager.LoadScene(sceneType.ToString());
        sceneNumber = (int)sceneType;
    }
    public void GameFinish()
    {

    }
    //public static void Save()
}
