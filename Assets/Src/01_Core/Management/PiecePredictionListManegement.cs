using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiecePredictionListManegement : MonoBehaviour
{

    /*<責務>予測リストに設置するパズルピースの監視と更新、生成情報の管理を行う。
     */
    //参照用の位置情報
    [SerializeField] Transform[] piecesPosition; //予測リストに設置するパズルピースの位置情報
    //仮処置
    [SerializeField] ObjectFactory objectFactory; //オブジェクト生成クラス

    private GameObject[] piecePredictionListPieceObject; //予測リストに設置しているオブジェクトを格納（３つまで）
    private int[] listPieceIndex; //Nextオブジェクトのオブジェクト番号（enumから
    private int randomRangeMaxValues; //ランダム値の最大値
    private const int pieceObjectGenerateMaxValue = 3; //オブジェクト生成数

    private void Awake()
    {
        Initialize();
    }
    private void OnEnable()
    {
        //イベント登録
        EventManager.Instance.fallPiece += PredictionListUpdate;
    }
    private void OnDisable()
    {
        EventManager.Instance.fallPiece -= PredictionListUpdate;
    }

    void Start()
    {

        if (randomRangeMaxValues == 0) randomRangeMaxValues = 5;
        piecePredictionListPieceObject = new GameObject[pieceObjectGenerateMaxValue];
        listPieceIndex = new int[pieceObjectGenerateMaxValue];


        //最初に生成
        for (int i = 0; i < piecesPosition.Length; i++)
        {
            var randomValues = Random.Range(0, randomRangeMaxValues);
            piecePredictionListPieceObject[i] = objectFactory.GeneratePredictionListPiece(piecesPosition[i].transform, randomValues);
            listPieceIndex[i] = randomValues;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void Initialize()
    {
        //別オブジェクトのクラス参照の保障
        if (objectFactory == null)
        {
            objectFactory = GameObject.Find(AcquisitionObjectName.ObjectFactory.ToString()).GetComponent<ObjectFactory>();
        }
    }
    //予測リストの更新
    private void PredictionListUpdate()
    {
        //1フレーム待ち処理（PieceManagementのイベントメソッドを優先する
        StartCoroutine(PredictionListUpdateCoroutine());
    }
    //予測リストの繰り上げ
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
            listPieceIndex[i] = listPieceIndex[i + 1];
        }

        return pieceObjects;
    }
    //予測リストの末尾に設置するオブジェクトを生成
    private GameObject PredictionListPieceObjectNewGnerate(Transform generatePositon)
    {
        //生成する種類番号をランダムで決める
        var randomValues = Random.Range(0, randomRangeMaxValues);
        //予測リストに追加するパズルピースの生成
        var lastPieceObject = objectFactory.GeneratePredictionListPiece(generatePositon, randomValues);
        //生成したパズルピースの番号を要素数管理変数に格納
        listPieceIndex[listPieceIndex.Length - 1] = randomValues;

        return lastPieceObject;
    }
    private IEnumerator PredictionListUpdateCoroutine()
    {
        yield return null;

        //イベント通知越しに生成するパズルピースの種類を渡す、その後削除
        EventManager.Instance.PieceObjectGenerateDecidedEvent(listPieceIndex[0]);
        Destroy(piecePredictionListPieceObject[0]);

        //予測リストの参照と配置場所の更新
        piecePredictionListPieceObject = PredictionListPieceCarryUp(piecePredictionListPieceObject);
        //新規パズルピースの生成
        piecePredictionListPieceObject[piecePredictionListPieceObject.Length - 1] = PredictionListPieceObjectNewGnerate(piecesPosition[piecesPosition.Length - 1]);
    }
}
