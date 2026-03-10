using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextPiece : MonoBehaviour
{
    public ObjectReference objectReference;
    [SerializeField] GameObject[] piecesPosition; //Nextオブジェクトを設置する位置
    private GameObject[] nextPieceObject; //Nextオブジェクトを格納（３つまで）

    private int[] nextPieceIndex; //Nextオブジェクトのオブジェクト番号（enumから
    private int randomRangeMaxValues; //ランダム値
    private const int pieceObjectMaxValue = 3; //オブジェクト生成数

    // Start is called before the first frame update

    void Start()
    {
        if (randomRangeMaxValues == 0) randomRangeMaxValues = 4;
        nextPieceObject = new GameObject[pieceObjectMaxValue];
        nextPieceIndex = new int[pieceObjectMaxValue];

        //最初に生成
        for (int i = 0; i < piecesPosition.Length; i++)
        {
            var randomValues = Random.Range(0, randomRangeMaxValues);
            nextPieceObject[i] = ObjectProcess.Instance.GeneratePieceObject(objectReference.nextPieces[randomValues], piecesPosition[i].transform, GenerateParentObjectName.NextPieces.ToString());
            nextPieceIndex[i] = randomValues;
        }
        //イベント登録
        ObjectEventManager.Instance.ObjectGenerate += NextPieceGenerate;
    }
    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        ObjectEventManager.Instance.ObjectGenerate -= NextPieceGenerate;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void NextPieceGenerate()
    {
        //Playerクラスに要素：０の参照を渡す
        //Playerオブジェクトの登録がない場合は、ObjectManagerのplayerオブジェクトを使用
        ObjectManager.Instance.Get<PlayableObject>(ReferenceObjectName.PlayableObject.ToString(), objectReference.player).randomValues = nextPieceIndex[0];
        //要素：０のオブジェクトを破壊
        Destroy(nextPieceObject[0]);
        //Nextオブジェクトを更新
        nextPieceObject = SetNextPieces(nextPieceObject);
    }
    //ネクストピースを進める
    private GameObject[] SetNextPieces(GameObject[] pieceObjects)
    {
        //要素０、１に参照を繰り上げ
        for (int i = 0; i < pieceObjects.Length - 1; ++i)
        {
            pieceObjects[i] = pieceObjects[i+1];
            pieceObjects[i].transform.position = piecesPosition[i].transform.position;
            nextPieceIndex[i] = nextPieceIndex[i+1];
        }
        //末尾（要素：２）に参照するオブジェクトを生成
        var randomValues = Random.Range(0, randomRangeMaxValues);
        pieceObjects[pieceObjectMaxValue - 1] = ObjectProcess.Instance.GeneratePieceObject(objectReference.nextPieces[randomValues], piecesPosition[pieceObjectMaxValue - 1].transform, GenerateParentObjectName.NextPieces.ToString());
        pieceObjects[pieceObjectMaxValue - 1].transform.position = piecesPosition[pieceObjectMaxValue - 1].transform.position;
        nextPieceIndex[pieceObjectMaxValue - 1] = randomValues;

        return pieceObjects;
    }
}
