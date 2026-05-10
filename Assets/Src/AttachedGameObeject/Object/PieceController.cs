using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
    private PieceSynthesisAction pieceSynthesisAction;

    private IPieceMove moveAction; //ステートごと命令クラスの基盤IF
    private PieceStateHandler pieceStateHandler; //落下状態専用状態指定・取得クラス
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
    }

    // Use this for initialization
    void Start()
    {
        //オブジェクトのコンポーネントのアクティブ初期値
        //Nextオブジェクト → 非活性：合成後オブジェクト → 活性
        //ObjectManager.Instance.Register(GenerateParentObjectName.Pieces.ToString(), this.gameObject);

        // tag 名を指定
        this.gameObject.tag = TagName.Piece.ToString();
        Initialize();
        //最初の依存先の指定とステート別処理の命令
        if (pieceStateHandler.IsDesignationState() != (int)PieceState.Fall)
        {
            //Stateの変更
            ChangeMoveProcessDependence((int)PieceState.Idle);
            moveAction.Execute();
        }
    }
    private void OnEnable()
    {
        //イベント登録
        EventManager.Instance.pieceObjectFall += MoveFall;
    }
    private void OnDisable()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //移動の命令クラスのメソッドを呼び出す
        MoveFollow();
    }

    /*命令クラスを全てInterface越しに呼び出す、コンストラクタで処理クラスの参照を渡す
     *M：処理する際は命令クラスのメソッドだけ呼び出せばよい（Interface内で宣言しているメソッド
     *　 →処理内容を知らなくてよい（処理メソッド含む
     *　 依存が一方通行になる（Piece → 命令クラス → 処理クラス
     *　 →このクラスと処理クラスが命令クラスを持つことにより、一方通行の処理二も書関わらず依存関係が複雑になる
     *D：interface実現クラスに特定のクラスを渡している
     *　 →処理クラスを差し替えるときに、このクラスと命令クラスを変更する必要がある
     *　 このクラスが一部の処理クラスに依存している（参照のために
     *　 →依存関係が命令クラスを跨いだものではなくなる（Piece → 命令クラス → 処理クラス
     *　                                              |--------------------↑
     *　  生成処理クラスが依存を持ってこのクラスに渡す選択もあるが、その場合生成処理クラスが責務過多になる
     *　  これを解決する＝きれいな依存関係を構築する単には、DIコンテナが必要となってくる（本プロジェクトは未実装
     */
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

    //interfaceを利用して依存先の変更をする
    private void ChangeMoveProcessDependence(int pieceState)
    {
        if (pieceState == (int)PieceState.Idle) moveAction = new PieceIdleHandler(pieceStateHandler);
        else if (pieceState == (int)PieceState.Follow) moveAction = new PieceFollowHandler(pieceFollowAction, pieceStateHandler, pieceViewHandler);
        else if (pieceState == (int)PieceState.Fall) moveAction = new PieceFallHandler(pieceFallAction, pieceStateHandler, pieceViewHandler);
        else if(pieceState == (int)PieceState.Synthesis) moveAction = new PieceSynthesisHandler(pieceSynthesisAction, pieceStateHandler);
    }
    private void MoveFollow()
    {
        //落下させるオブジェクト（＝Playerを追従する）であれば移動（追従）処理を行う
        //ー＞状態が落下になっていればはじくため、枠内にいるパズルピースに影響は出ない
        if (IsDesignationStateValue() <= (int)PieceState.Follow)
        {
            //一度のみ初期化
            //Stateの変更
            if (IsDesignationStateValue() == (int)PieceState.Idle) ChangeMoveProcessDependence((int)PieceState.Follow);
            IsDesignationStateValue();
            moveAction.Execute();
        }
    }
    private void MoveFall()
    {
        //Stateの変更
        ChangeMoveProcessDependence((int)PieceState.Fall);
        //イベント発火、落下処理命令をする
        moveAction.Execute();
        //イベントの削除
        EventManager.Instance.pieceObjectFall -= MoveFall;
    }


    private void OnCollisionEnter2D(Collision2D otherCollisionObject)
    {
        if(otherCollisionObject.gameObject.tag == TagName.Piece.ToString())
        {
            //衝突したオブジェクトの存在チェック
            otherCollisionObject.gameObject.TryGetComponent(out PieceController otherPiece);

            //このオブジェクトと衝突オブジェクトの状態を確認
            if (!IsOtherPieceCollisionJudgement(otherCollisionObject, otherPiece)) return;
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
    private bool IsOtherPieceCollisionJudgement(Collision2D otherCollision2D, PieceController otherPiece)
    {
        //Enumの確認
        if (otherPiece.fishPieceType != fishPieceType) return false;
        //相互のオブジェクトを徹底調査
        if (this == null || otherPiece == null || this.transform == null || otherPiece.transform == null) return false;

        if (otherPiece.IsDesignationStateValue() == (int)PieceState.Synthesis) return false;

        return true;
    }
    public int IsDesignationStateValue()
    {
        Debug.Log($"現在のステート{pieceStateHandler.IsDesignationState()}");
        return pieceStateHandler.IsDesignationState();
    }
}
