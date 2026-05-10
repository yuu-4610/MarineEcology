using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFactory : MonoBehaviour
{
    //public static ObjectFactory Instance { get; private set; }
    //[HideInInspector] public Transform objectTransform; //生成予定オブジェクトの位置情報
    [SerializeField] ObjectReference objectReference;

    Dictionary<string, GameObject> parents = new Dictionary<string, GameObject>();
    //[HideInInspector] int objectLength; //オブジェクト配列の

    private void Awake()
    {
        //シングルトン
        //if (Instance != null && Instance != this)
        //{
        //    Destroy(this.gameObject);
        //    return;
        //}
        //Instance = this;
    }
    private void OnEnable()
    {
        EventManager.Instance.synthesisPieceObjectGenerate += SynthesisProcessGeneratePiece;
    }
    private void OnDisable()
    {
        EventManager.Instance.synthesisPieceObjectGenerate -= SynthesisProcessGeneratePiece;
    }
    private void Start()
    {

    }

    // Start is called before the first frame update
    //GameObject GenerateObject, Transform objectTransform, int childrenCount
    public GameObject GeneratePieceObject(int generatePieceNumber)
    {
        //親オブジェクトの生成
        var parentObject = GetPanetObject(GenerateParentObjectName.Pieces.ToString());
        //オブジェクト生成
        var pieceObject = Instantiate(objectReference.pieceObjects[generatePieceNumber], Vector3.zero, Quaternion.identity, parentObject.transform);

        return pieceObject;
    }

    //合成時パズルピース生成処理
    public void SynthesisProcessGeneratePiece(Transform piece, Transform otherPiece, int fishPieceTypeNumber)
    {
        //次のオブジェクトがあれば生成可能
        if (objectReference.listPieces.Length > fishPieceTypeNumber + 1) return;

        //親オブジェクトの生成
        var parentObject = GetPanetObject(GenerateParentObjectName.Pieces.ToString());

        //生成位置と回転情報の計算
        Vector3 center = (piece.position + otherPiece.position) / 2;
        Quaternion rotation = Quaternion.Lerp(piece.rotation, otherPiece.rotation, 0.5f);

        var prefabPiece = Instantiate(objectReference.pieceObjects[fishPieceTypeNumber + 1], center, rotation, parentObject.transform);

        //このメソッドで生成したパズルピースのステートを「Fall」にする
        prefabPiece.GetComponent<PieceStateController>().ChangeExecutionPieceState(PieceState.Fall);
    }

    public GameObject GeneratePredictionListPiece(Transform listPieceTransform, int generatePieceNumber)
    {
        var parentObject = GetPanetObject(GenerateParentObjectName.listPieces.ToString());

        var listPiece = Instantiate(objectReference.listPieces[generatePieceNumber], listPieceTransform.position, Quaternion.identity, parentObject.transform);

        return listPiece;
    }
    private GameObject GetPanetObject(string name)
    {
        //オブジェクトが存在しなければ新しく作成する
        if (!parents.TryGetValue(name, out var parent) || parent == null)
        {
            parent = new GameObject(name);
            parents[name] = parent;
        }

        return parent;
    }
}
