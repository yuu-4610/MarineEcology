using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public ObjectReference objectReference;
    public FishNode fishNodeType; //Nodeタイプ
    [SerializeField] PieceMoveAction pieceMoveAction;
    [SerializeField] PieceState pieceState;
    private Rigidbody2D rigidbody2D; //物理演算
    private CircleCollider2D circleCollider2D; //当たり判定

    public int point { get; private set; } //得点数
    public bool isDestroyed = false; //このオブジェクトが存在するか
    //合成後オブジェクト：生成後当たり判定を有効化
    //NextObject：落下後当たり判定を有効化
    public bool hasCollider = false; //パズルピース同士の合体が可能か
    private bool isDropObject = true; //落下処理を行うオブジェクトか（＝プレイヤーに追従するオブジェクトか）
    private IPieceMove moveAction;
    private bool processOrder = false; //処理順
    private static int fruitsSerial = 0; //生成番号
    private int mySerial; //生成番号格納用

    private void Awake()
    {
        //識別用
        ++fruitsSerial;
        mySerial = fruitsSerial;
        //ピースオブジェクトの各ポイント（点数）
        point = 5 * ((int)fishNodeType + 1);
    }

    // Use this for initialization
    void Start()
    {
        //コンポーネントの取得
        rigidbody2D = GetComponent<Rigidbody2D>();
        circleCollider2D = GetComponent<CircleCollider2D>();
        //オブジェクトのコンポーネントのアクティブ初期値
        //Nextオブジェクト → 非活性：合成後オブジェクト → 活性
        if(!hasCollider) circleCollider2D.enabled = false;
        //ObjectManager.Instance.Register(GenerateParentObjectName.Pieces.ToString(), this.gameObject);
        // tag 名を指定
        this.gameObject.tag = TagName.Piece.ToString();

        InitializePieceMoveAction();

        //駒合成時に下ベクトルに向ける
        EventManager.Instance.PieceSyntghesis += PieceSyntghesis;
    }
    private void OnEnable()
    {
        //イベント登録
        EventManager.Instance.PieceObjectDrop += ObjectDrop;
    }
    private void OnDisable()
    {
        EventManager.Instance.PieceObjectDrop -= ObjectDrop;
        EventManager.Instance.PieceSyntghesis -= PieceSyntghesis;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 v = rigidbody2D.velocity;
        v.y = -4f;
        rigidbody2D.velocity = v;

        //移動の命令クラスのメソッドを呼び出す
        MoveCommand();
    }
    private void ObjectDrop()
    {
        //イベント発火後、当たり判定を有効にする
        circleCollider2D.enabled = true;
    }
    private void InitializePieceMoveAction()
    {
        moveAction = new PieceMoveHandler(pieceMoveAction);
    }
    private void MoveCommand()
    {
        //落下させるオブジェクト（＝Playerを追従する）であれば移動（追従）処理を行う
        if(isDropObject) moveAction.Execute();
    }
    private void PieceSyntghesis()
    {
        //if(isSynthesis) rigidbody2D.gravityScale = 1f;
        //circleCollider2D.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collisionObject)
    {
        //指定オブジェクトの確認
        if (!collisionObject.gameObject.TryGetComponent(out Piece otherPiece)) return;
        //Enumの確認
        if (otherPiece.fishNodeType != fishNodeType) return;
        //相互のオブジェクトを徹底調査
        if (this == null || otherPiece == null || this.transform == null || otherPiece.transform == null) return;

        if (GameManager.Instance.testFlg) return;
        StartCoroutine(PieceSyntghesisCoroutine(otherPiece));
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
                if (objectReference.pieceObjects.Length > (int)fishNodeType + 1)
                {
                    //次番号のオブジェクトを生成
                    var nextObject = ObjectProcess.Instance.SynthesisPieceEvent(objectReference.pieceObjects[(int)fishNodeType + 1], this.gameObject, otherPiece, GenerateParentObjectName.Pieces.ToString());
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