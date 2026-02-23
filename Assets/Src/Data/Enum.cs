using System.Collections;
using UnityEngine;

public enum FishNode
{
    node0 = 0, //カクレクマノミ
    node1 = 1, //メバル
    node2 = 2, //アジ
    node3 = 3, //ヤマメ
    node4 = 4, //マゴチ
    node5 = 5, //キハダ
    node6 = 6, //マダイ
    node7 = 7, //カジキ
    node8 = 8, //ジンベエ
}
public enum SceneType
{
    TitleScene, //タイトルシーン
    GameScene, //ゲームシーン
}

public enum GenerateParentObjectName
{
    Pieces, //ピースオブジェクト
    NextPieces, //Nextオブジェクト
}

public enum TagName
{
    Piece, //ピース
}