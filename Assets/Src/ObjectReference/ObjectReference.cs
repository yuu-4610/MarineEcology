using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Data/ObjectReference")]
public class ObjectReference : ScriptableObject
{
    //参照するオブジェクトを登録する
    public GameObject[] pieceObjects;
    public GameObject[] nextPieces;
    public GameObject player; //プレイヤーオブジェクトの参照
    public GameObject gameSceneUI; //GameScemeのキャンバス
}
