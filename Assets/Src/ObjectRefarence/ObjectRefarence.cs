using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Data/ObjectRefarence")]
public class ObjectRefarence : ScriptableObject
{
    public GameObject[] pieceObjects;
    public GameObject[] nextPieces;
    public GameObject player; //プレイヤーオブジェクトの参照
}
