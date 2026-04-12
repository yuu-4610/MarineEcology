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

//列挙型で用意し以下の問題を未然に防ぐ
//・文字列指定した際のスペルミスによるエラーの防止
//・インデックス指定した際の可読性低下や混乱を防ぐ
//定義した値のみ使用できるようにして安全に管理するため
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
    Piece, //ピースオブジェクトに付与
}

public enum ReferenceObjectName
{
    Canvas_GameScene, //ゲームシーンのキャンバス
    PlayableObject, //プレイアブルオブジェクト
}
public enum ResourcePath
{
    BGM, //BGMが置いてあるパス
    SE, //SEが置いてあるパス
}
public enum AudioMixerGroupName
{
    BGM,
    SE,
}
public enum AudioFileName
{
    kaityusekai, //BGM　海中世界_backGround
    tokonatunoumi, //BGM　常夏の海_backGround
    onoma, //SE　Onoma
    whistle, //SE　警官のホイッスル
    moveCursor,
    buttonClick, //ボタンを押したときの音
    turnThePage, //遊び方ボードをめくるときの音
}