using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFactory : MonoBehaviour
{
    /*<責務>パズルピースの生成処理を行う
     *
     */
    [SerializeField] ObjectReference objectReference;

    Dictionary<string, GameObject> parents = new Dictionary<string, GameObject>(); //親オブジェクトの登録
    //[HideInInspector] int objectLength; //オブジェクト配列の

    private void Awake()
    {
        
    }
    private void OnEnable()
    {
        //パズルピース衝突時の合成イベント
        EventManager.Instance.synthesisPieceObjectGenerate += SynthesisProcessGeneratePiece;
    }
    private void OnDisable()
    {
        EventManager.Instance.synthesisPieceObjectGenerate -= SynthesisProcessGeneratePiece;
    }
    private void Start()
    {

    }

    //プレイヤーを追従するパズルピースの生成
    public GameObject GeneratePieceObject(int generatePieceNumber, Vector3 generatePosition)
    {
        //親オブジェクトの生成
        var parentObject = GetOrCreatePanetObject(GenerateParentObjectName.Pieces.ToString());
        //オブジェクト生成
        var pieceObject = Instantiate(objectReference.pieceObjects[generatePieceNumber], generatePosition, Quaternion.identity, parentObject.transform);

        InitializeGenerateObject(pieceObject);

        return pieceObject;
    }

    //合成時パズルピース生成処理
    public void SynthesisProcessGeneratePiece(Transform piece, Transform otherPiece, int fishPieceTypeNumber)
    {
        var generateObjectFishPieceTypeNumber = fishPieceTypeNumber + 1;
        //次の番号があれば生成可能
        if (objectReference.pieceObjects.Length < generateObjectFishPieceTypeNumber) return;

        //親オブジェクトの生成
        var parentObject = GetOrCreatePanetObject(GenerateParentObjectName.Pieces.ToString());

        //生成位置と回転情報の計算
        Vector3 center = (piece.position + otherPiece.position) / 2;
        Quaternion rotation = Quaternion.Lerp(piece.rotation, otherPiece.rotation, 0.5f);

        //オブジェクト生成
        var prefabPiece = Instantiate(objectReference.pieceObjects[generateObjectFishPieceTypeNumber], center, rotation, parentObject.transform);
        //生成したオブジェクトの設定
        InitializeSynthesisGenereteObject(prefabPiece);
    }

    //パズルピースの予測リストに設置するパズルピースの生成処理
    public GameObject GeneratePredictionListPiece(Transform listPieceTransform, int generatePieceNumber)
    {
        //親オブジェクトの生成
        var parentObject = GetOrCreatePanetObject(GenerateParentObjectName.listPieces.ToString());

        //オブジェクト生成
        var listPiece = Instantiate(objectReference.listPieces[generatePieceNumber], listPieceTransform.position, Quaternion.identity, parentObject.transform);

        return listPiece;
    }

    //親オブジェクトの生成処理
    private GameObject GetOrCreatePanetObject(string name)
    {
        //オブジェクトが存在しなければ新しく作成する
        if (!parents.TryGetValue(name, out var parent) || parent == null)
        {
            parent = new GameObject(name);
            parents[name] = parent;
        }

        return parent;
    }

    //合成時に生成したパズルピースオブジェクトの初期化設定
    private void InitializeSynthesisGenereteObject(GameObject generateObject)
    {
        //生成したパズルピースのステートの処理を「Fall」にする
        generateObject.GetComponent<PieceController>().pieceStateHandler.Execute(PieceState.Fall);
        generateObject.GetComponent<PieceViewController>().FallView();
        generateObject.GetComponent<PieceFallAction>().HasVelocity();
    }

    //プレイヤーを追従するオブジェクトの初期化設定
    private void InitializeGenerateObject(GameObject pieceObject)
    {
        pieceObject.GetComponent<CircleCollider2D>().enabled = false;
    }
}
