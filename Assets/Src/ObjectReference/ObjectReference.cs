using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Data/ObjectReference")]
public class ObjectReference : ScriptableObject
{
    //参照するオブジェクトを登録する
    public GameObject[] pieceObjects;
    public GameObject[] listPieces;

    //クラス情報の参照用のため、ScriptableObjectへ移行
    public GameObject gameSceneUI; //GameScemeのキャンバス
}
