using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public ObjectReference objectReference;
    //使用予定
    [Header("パズルピースの番号・魚の種類")]
    public FishType fishPieceType; //パズルピースの番号
    [Header("パズルピースの移動処理をするクラス")]
    [SerializeField] PieceMoveAction pieceMoveAction; //パズルピースのプレイヤー追従処理をするクラス
    [Header("パズルピースのコンポーネントや見た目を管理するクラス")]
    [SerializeField] PieceViewController pieceViewController;
    private IPieceMove moveAction;
    private IPieceMove fallAction;
    private IStateHandler stateHandler; //落下状態専用状態指定・取得クラス
    private IColliderJudgement colliderJudgement;
    private static int fruitsSerial = 0; //生成番号
    public int mySerial { get; private set; } //生成番号格納用


    [HideInInspector] public int point { get; private set; } //得点数
    [HideInInspector] public bool isDestroyed = false; //このオブジェクトが存在するか
    //合成後オブジェクト：生成後当たり判定を有効化
    //NextObject：落下後当たり判定を有効化
    [HideInInspector] public bool hasCollider = false; //パズルピース同士の合体が可能か
    
    private bool processOrder = false; //処理順
    

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
        InitializePieceMoveAction();
    }
    private void OnEnable()
    {
        //イベント登録
        EventManager.Instance.PieceObjectDrop += ObjectDrop;
        EventManager.Instance.PieceSyntghesis += PieceSyntghesis;
    }
    private void OnDisable()
    {
        EventManager.Instance.PieceSyntghesis -= PieceSyntghesis;
    }

    // Update is called once per frame
    void Update()
    {
        //Vector2 v = rigidbody2D.velocity;
        //v.y = -4f;
        //rigidbody2D.velocity = v;

        //移動の命令クラスのメソッドを呼び出す
        MoveCommand();
    }
    
    private void InitializePieceMoveAction()
    {
        //パズルピースの各状態値の保持と状態の変更、状態の取得を命令するクラス
        var pieceStateController = new PieceStateController();
        stateHandler = new PieceStateHandler(pieceStateController);
        //パズルピースの各状態の処理命令をするクラス
        var pieceViewHandler = new PieceViewHandler(pieceViewController);

        //移動処理命令クラス（MonoBehaviour継承クラス）を渡す
        moveAction = new PieceMoveHandler(pieceMoveAction, (PieceStateHandler)stateHandler, pieceViewHandler);
        //落下処理命令クラス（MonoBehaviour継承クラス）を渡す
        fallAction = new PieceFallHandler((PieceStateHandler)stateHandler, pieceViewHandler);
        //衝突処理時の条件クラス
        var pieceColliderJudgement = new PieceColliderJudgement();
        colliderJudgement = new PieceColliderJudgementHandler(pieceColliderJudgement, fishPieceType);
    }
    private void MoveCommand()
    {
        //落下させるオブジェクト（＝Playerを追従する）であれば移動（追従）処理を行う
        //ー＞状態が落下になっていればはじくため、枠内にいるパズルピースに影響は出ない
        if(stateHandler.IsDesignationStatus() < (int)PieceState.Drop) moveAction.Execute();
    }
    private void ObjectDrop()
    {
        //イベント発火、落下処理命令をする
        fallAction.Execute();
        //イベントの削除
        EventManager.Instance.PieceObjectDrop -= ObjectDrop;
    }
    private void PieceSyntghesis()
    {
        //if(isSynthesis) rigidbody2D.gravityScale = 1f;
        //circleCollider2D.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D otherCollisionObject)
    {
        if (!colliderJudgement.IsHitJudgement(otherCollisionObject)) return;

        //StartCoroutine(PieceSyntghesisCoroutine(otherPiece));
    }

    private IEnumerator PieceSyntghesisCoroutine(Piece otherPiece)
    {
        //当たったPieceオブジェクトが削除されていなければ（isDestroyed = false）処理
        if (!otherPiece.isDestroyed)
        {
            // my_serial の値が大きいオブジェクトに処理をさせる
            if (mySerial < otherPiece.mySerial)
            {
                //第３のオブジェクトと衝突したときに処理を走らせないようにする
                isDestroyed = true;
                otherPiece.isDestroyed = true;
                //効果音を出す 
                AudioManager.Instance.PlaySE(AudioHelper.ToName(AudioFileName.onoma));

                //次のオブジェクトがあれば実行
                if (objectReference.pieceObjects.Length > (int)fishPieceType + 1)
                {
                    //次番号のオブジェクトを生成
                    var nextObject = ObjectFactory.Instance.SynthesisPieceEvent(objectReference.pieceObjects[(int)fishPieceType + 1], this.gameObject, otherPiece, GenerateParentObjectName.Pieces.ToString());
                    if(nextObject != null)
                    {
                        nextObject.GetComponent<Piece>().hasCollider = true;
                    }
                    processOrder = true;
                }
                UIManager.Instance.SetPoint(otherPiece.point);
            }
            // my_serial の値の値が小さいほうの処理
            else
            {
                processOrder = true;
            }
            //削除可能（processOrder）になるまで繰り返す
            while (!processOrder) yield return null;
            Destroy(this.gameObject);
        }
        
    }
}