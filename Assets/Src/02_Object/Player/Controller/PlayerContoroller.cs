using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerContoroller : MonoBehaviour
{
    /*<責務>プレイヤー入力を受け取り、入力状態に応じて適切な挙動処理を実行する。
     *・
     */
    private PlayerMoveHandler moveAction; //移動処理命令クラス用インターフェース
    private PlayerDropHandler dropAction; //落下処理命令クラス用インターフェース
    private float moveInputValue; //移動の入力値、持続的に所持するため
    private float fallInputValue; //落下処理の入力値、持続的に所持するため
    private bool isauthorityToAct;

    private float moveLimit = 3f;

    private void Awake()
    {
        Initialize();
    }
    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        EventManager.Instance.playerMoveLimit -= AuthorityDeprivation;
        EventManager.Instance.playerMoveLimitCancellation -= AuthorityToActGrant;
    }
    void Start()
    {
        EventManager.Instance.playerMoveLimit += AuthorityDeprivation;
        EventManager.Instance.playerMoveLimitCancellation += AuthorityToActGrant;
        isauthorityToAct = true;
    }

    // Update is called once per frame
    void Update()
    {
        //移動命令の処理
        MoveInput();
        //落下命令の処理
        FallInput();
        InputReception();
    }
    //初期化子
    private void Initialize()
    {
        /*１．命令クラスをInterface越しで初期化 → 依存度の低下＝クラスの変更が容易になる
         *２．命令クラスの責務を限定 → 状態を持たない＝処理クラスを呼ぶだけに専念できる
         *３．処理クラスをメソッド内でのみ使用 → 依存度の低下＝クラスの変更が容易になる
         *４．処理クラスと依存関係にする → InitializePlayerMoveActionの項目２が実現できる
        */

        //移動処理クラスの参照を渡す
        var processMoveAction = new PlayerMoveAction(this.gameObject.transform, this.gameObject.transform.position.x, moveLimit);
        moveAction = new PlayerMoveHandler(processMoveAction);

        //落とした(=Spaceキー押下)と検知し、イベントを呼ぶクラス
        var processDropAction = new PlayerDropAction();
        dropAction = new PlayerDropHandler(processDropAction);

    }
    //移動処理命令クラスを呼び出す
    private void MoveInput()
    {
        if (isauthorityToAct)
        {
            //Playerを動かす処理 → マウスの操作で x 軸の変更
            moveInputValue = PlayerInput.MousePositionValue();
            moveAction.Execute(moveInputValue);
        }
    }
    //落下処理命令クラスを呼び出す
    private void FallInput()
    {
        //パズルピースを落とす処理 → Spaceキーを押したらイベントを呼ぶ
        fallInputValue = PlayerInput.PieceObjectDropInput();
        if(fallInputValue > 0) dropAction.Execute();
    }
    private void InputReception()
    {
        //設定ボタンの表示切り替え → Left,RightShiftを押したら
        if (PlayerInput.OptionButton() == 1)
        {
            
            if(SceneManager.GetActiveScene().name == SceneName.GameScene.ToString())
            {
                EventManager.Instance.OptionBoardDisplayEvent();
            }
        }
    }

    private void AuthorityToActGrant()
    {
        isauthorityToAct = true;
    }
    private void AuthorityDeprivation()
    {
        isauthorityToAct = false;;
    }
}
