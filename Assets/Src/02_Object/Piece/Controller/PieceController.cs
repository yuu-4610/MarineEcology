using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class PieceController : MonoBehaviour
{
    /*<責務>状態に応じて挙動を変更していく
     *このクラスは挙動の命令クラスを呼ぶだけなので、処理内容は把握してない
     *
     */
    [Header("パズルピースの番号・魚の種類")]
    public FishType fishPieceType; //パズルピースの番号
    [Header("パズルピースの移動処理をするクラス")]
    [SerializeField] PieceFollowAction pieceFollowAction; //パズルピースのプレイヤー追従処理をするクラス
    [SerializeField] PieceFallAction pieceFallAction; //パズルピースの落下時に処理をするクラス
    [SerializeField] PieceViewController pieceViewController; //パズルピースの見た目とコンポーネントの状況を変更するクラス
    private PieceSynthesisAction pieceSynthesisAction; //衝突処理クラス

    private IPieceMove moveAction; //ステートごと命令クラスの基盤インターフェース
    public PieceStateHandler pieceStateHandler { get; private set; } //落下状態専用状態指定・取得クラス
    private PieceViewHandler pieceViewHandler; //見た目とコンポーネントの状況変更処理の命令クラス
    private static int fruitsSerial = 0; //生成番号
    public int mySerial { get; private set; } //生成番号格納用
    private int point; //得点数


    private void Awake()
    {
        //識別用
        ++fruitsSerial;
        mySerial = fruitsSerial;
        //ピースオブジェクトの各ポイント（点数）
        point = 5 * ((int)fishPieceType + 1);

        Initialize();
    }

    private void OnEnable()
    {
        //イベント登録
        EventManager.Instance.dropPiece += MoveFall;
    }
    private void OnDisable()
    {
        //イベント解除
        //破壊されたオブジェクトのイベントが登録されたまま発火させるとエラーが起きるため
        EventManager.Instance.dropPiece -= MoveFall;
    }

    void Start()
    {
        // tag 名を指定
        this.gameObject.tag = TagName.Piece.ToString();

        // 最初の依存先の指定とステート別処理の命令
        if (pieceStateHandler.IsDesignationState() != (int)PieceState.Fall)
        {
            //Stateの変更
            ChangeMoveProcessDependence((int)PieceState.Idle);
            moveAction.Execute();
        }
        else
        {
            //合成語に生成したオブジェクトであればイベントの解除
            //登録状態だと、プレイヤーの入力によりイベントが走るため
            EventManager.Instance.dropPiece -= MoveFall;
        }
    }

    void Update()
    {
        //移動の命令クラスのメソッドを呼び出す
        MoveFollow();
    }

    private void Initialize()
    {
        //処理に必要なMonoBehaviour継承クラスの参照取得保険
        if (!gameObject.TryGetComponent(out PieceFollowAction pieceFollowAction))
        {
            this.pieceFollowAction = pieceFollowAction.AddComponent<PieceFollowAction>();
        }
        if (!gameObject.TryGetComponent(out PieceFallAction pieceFallAction))
        {
            this.pieceFallAction = pieceFallAction.AddComponent<PieceFallAction>();
        }
        if (!gameObject.TryGetComponent(out PieceViewController pieceViewController))
        {
            this.pieceViewController = pieceViewController.AddComponent<PieceViewController>();
        }

        //命令クラスの取得
        var pieceStateController = new PieceStateController();
        pieceStateHandler = new PieceStateHandler(pieceStateController);
        pieceViewHandler = new PieceViewHandler(this.pieceViewController);
    }

    /*interfaceを利用して依存先の変更をする
     *処理内容を知らなくてよい（処理メソッド含む
     *参照の切り替えが可能
     */
    private void ChangeMoveProcessDependence(int pieceState)
    {
        if (pieceState == (int)PieceState.Idle) moveAction = new PieceIdleHandler(pieceStateHandler);
        else if (pieceState == (int)PieceState.Follow) moveAction = new PieceFollowHandler(pieceFollowAction, pieceStateHandler, pieceViewHandler);
        else if (pieceState == (int)PieceState.Fall) moveAction = new PieceFallHandler(pieceFallAction, pieceStateHandler, pieceViewHandler);
        else if(pieceState == (int)PieceState.Synthesis) moveAction = new PieceSynthesisHandler(pieceSynthesisAction, pieceStateHandler);
    }
    //パズルピースのプレイヤー追従処理
    private void MoveFollow()
    {
        //落下させるオブジェクト（＝Playerを追従する）であれば移動（追従）処理を行う
        //->状態が落下になっていればはじくため、枠内にいるパズルピースに影響は出ない
        if (IsDesignationStateValue() <= (int)PieceState.Follow)
        {
            //一度のみ依存先の変更
            //Stateの変更
            if (IsDesignationStateValue() == (int)PieceState.Idle) ChangeMoveProcessDependence((int)PieceState.Follow);
            moveAction.Execute();
        }
    }
    //パズルピースの落下処理
    private void MoveFall()
    {
        //Stateの変更
        ChangeMoveProcessDependence((int)PieceState.Fall);
        //イベント発火、落下処理命令をする
        moveAction.Execute();
        //イベントの削除
        EventManager.Instance.dropPiece -= MoveFall;
    }


    private void OnCollisionEnter2D(Collision2D otherCollisionObject)
    {
        //tag名がPieceに設定しているオブジェクトが触れれば
        if (otherCollisionObject.gameObject.tag == TagName.Piece.ToString())
        {
            //衝突したオブジェクトの存在チェック
            otherCollisionObject.gameObject.TryGetComponent(out PieceController otherPiece);

            //このオブジェクトと衝突オブジェクトの状態を確認
            if (!IsOtherPieceCollisionJudgement(otherPiece)) return;
            //mySerial 値が小さい方で処理をする
            if (mySerial < otherPiece.mySerial)
            {
                //処理クラスに値の受け渡し
                pieceSynthesisAction = new PieceSynthesisAction(point, (int)fishPieceType, this.transform, otherPiece.transform);
                //Stateの変更
                ChangeMoveProcessDependence((int)PieceState.Synthesis);
                moveAction.Execute();
            }

            //このオブジェクトを削除
            Destroy(this.gameObject);
        }
    }

    //相互の状態確認処理メソッド
    private bool IsOtherPieceCollisionJudgement(PieceController otherPiece)
    {
        //Enumの確認
        if (otherPiece.fishPieceType != fishPieceType) return false;
        //相互のオブジェクトを徹底調査
        if (this == null || otherPiece == null || this.transform == null || otherPiece.transform == null) return false;
        //衝突オブジェクトのStateの確認
        if (otherPiece.IsDesignationStateValue() == (int)PieceState.Synthesis) return false;

        return true;
    }
    //現在のStateの確認
    public int IsDesignationStateValue()
    {
        return pieceStateHandler.IsDesignationState();
    }
}
