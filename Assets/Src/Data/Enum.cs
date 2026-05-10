using System.Collections;
using UnityEngine;

public enum FishType
{
    type0 = 0, //カクレクマノミ
    type1 = 1, //メバル
    type2 = 2, //アジ
    type3 = 3, //ヤマメ
    type4 = 4, //マゴチ
    type5 = 5, //キハダ
    type6 = 6, //マダイ
    type7 = 7, //カジキ
    type8 = 8, //ジンベエ
}

//列挙型で用意し以下の問題を未然に防ぐ
//・文字列指定した際のスペルミスによるエラーの防止
//・インデックス指定した際の可読性低下や混乱を防ぐ
//定義した値のみ使用できるようにして安全に管理するため
public enum SceneName
{
    TitleScene, //タイトルシーン
    GameScene, //ゲームシーン
}

public enum GenerateParentObjectName
{
    Pieces, //ピースオブジェクト
    listPieces, //Nextオブジェクト
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
public enum PieceState
{
    Idle, //デフォルト値
    Follow, //追従
    Fall, //落下
    Synthesis, //合成
}