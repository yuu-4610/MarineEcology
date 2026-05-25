using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveSystem
{
    /*<責務>JSONファイルへの書き出し・書き込み処理の提供
     */
    private static string path => Application.persistentDataPath + "/myScore.json"; //保存先とファイル名

    public static void Save(GameData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
    }
    public static GameData Load()
    {
        // path がなければ（myScore.jsonがなければ）新しく作る
        if (!File.Exists(path))
        {
            return new GameData();
        }

        //対象 Json を読み取る
        string json = File.ReadAllText(path);
        GameData data = JsonUtility.FromJson<GameData>(json);

        return data;
    }
}
