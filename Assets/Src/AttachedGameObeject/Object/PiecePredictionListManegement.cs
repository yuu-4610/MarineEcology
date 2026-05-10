using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiecePredictionListManegement : MonoBehaviour
{

    /*<責務>パズルピースの予測リストを管理するクラス
     *値を保持し、生成やリストの更新を適切に行えるよう取り仕切るクラス
     *・値から状況を判断して、生成処理や更新処理を指示する
     */
    [SerializeField] ObjectReference objectReference;
    //参照用の位置情報
    [SerializeField] Transform[] piecesPosition; //予測リストに設置するパズルピースの位置情報
    //仮処置
    [SerializeField] ObjectFactory objectFactory; //オブジェクト生成クラス

    private GameObject[] piecePredictionListPieceObject; //Nextオブジェクトを格納（３つまで）
    private int[] nextPieceIndex; //Nextオブジェクトのオブジェクト番号（enumから
    private int randomRangeMaxValues; //ランダム値の最大値
    private const int pieceObjectMaxValue = 3; //オブジェクト生成数

    // Start is called before the first frame update

    void Start()
    {
        Initialize();

        if (randomRangeMaxValues == 0) randomRangeMaxValues = 4;
        piecePredictionListPieceObject = new GameObject[pieceObjectMaxValue];
        nextPieceIndex = new int[pieceObjectMaxValue];


        //最初に生成
        for (int i = 0; i < piecesPosition.Length; i++)
        {
            var randomValues = Random.Range(0, randomRangeMaxValues);
            piecePredictionListPieceObject[i] = objectFactory.GeneratePredictionListPiece(piecesPosition[i].transform, randomValues);
            nextPieceIndex[i] = randomValues;
        }
    }
    private void OnEnable()
    {
        //イベント登録
        EventManager.Instance.predictionListUpdate += PredictionListUpdate;
        //EventManager.Instance.onPlayerLeftRange += ;
    }
    private void OnDisable()
    {
        EventManager.Instance.predictionListUpdate -= PredictionListUpdate;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void Initialize()
    {
        if (objectFactory == null)
        {
            objectFactory = GameObject.Find("ObjectFactory").GetComponent<ObjectFactory>();
        }
    }
    private void PredictionListUpdate()
    {
        /*イベント発火
         *イベント通知越しに生成するパズルピースの種類を通知
         */
        EventManager.Instance.PieceObjectGenerateDecidedEvent(nextPieceIndex[0]);
        Destroy(piecePredictionListPieceObject[0]);
        
        //予測リストの参照と配置場所の更新
        piecePredictionListPieceObject = PredictionListPieceCarryUp(piecePredictionListPieceObject);
        //新規パズルピースの生成
        piecePredictionListPieceObject[piecePredictionListPieceObject.Length - 1] = PredictionListPieceObjectNewGnerate(piecesPosition[piecesPosition.Length - 1]);

    }
    //予測リストを更新する
    private GameObject[] PredictionListPieceCarryUp(GameObject[] pieceObjects)
    {
        //要素番号の参照を繰り上げ（１ → ０、２ → １）
        for (int i = 0; i < pieceObjects.Length - 1; ++i)
        {
            //配列要素番号の繰り上げ
            pieceObjects[i] = pieceObjects[i + 1];
            //画面上の予測リストの設置場所の繰り上げ
            pieceObjects[i].transform.position = piecesPosition[i].transform.position;
            //パズルピースの種類番号の繰り上げ
            nextPieceIndex[i] = nextPieceIndex[i + 1];
        }

        //配列末尾のオブジェクトを削除する
        Destroy(pieceObjects[pieceObjects.Length - 1]);

        return pieceObjects;
    }

    private GameObject PredictionListPieceObjectNewGnerate(Transform generatePositon)
    {
        //生成する種類番号をランダムで決める
        var randomValues = Random.Range(0, randomRangeMaxValues);
        //予測リストに追加するパズルピースの生成
        var lastPieceObject = objectFactory.GeneratePredictionListPiece(generatePositon, randomValues);

        return lastPieceObject;
    }
}
