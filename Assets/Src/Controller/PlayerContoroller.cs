using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerContoroller : MonoBehaviour
{
    //[SerializeField] PlayableObject playableObject;
    //[SerializeField] GameSceneUI gameSceneButton;
    [SerializeField] ObjectReference objectReference;

    private IPlayerMove moveAction; //移動処理命令クラス用インターフェース
    private IPlayerMove fallAction; //落下処理命令クラス用インターフェース


    private void Awake()
    {
        
    }
    private void OnEnable()
    {
        
    }
    void Start()
    {
        InitializePlayerMoveAction();
    }

    // Update is called once per frame
    void Update()
    {
        //移動と落下の命令クラスのメソッドを呼び出す
        MoveCommand();
        TestInputReception();
    }
    private void InitializePlayerMoveAction()
    {
        /*１．命令クラスをInterface越しで初期化 → 依存度の低下＝クラスの変更が容易になる
         *２．命令クラスの責務を限定 → 状態を持たない＝処理クラスを呼ぶだけに専念できる
         *３．処理クラスをメソッド内でのみ使用 → 依存度の低下＝クラスの変更が容易になる
         *４．処理クラスと依存関係にする → InitializePlayerMoveActionの項目２が実現できる
        */

        //PlayerMoveActionはこの場のみ
        var processMoveAction = new PlayerMoveAction(this.gameObject.transform, this.gameObject.transform.position.x, 4);
        moveAction = new PlayerMoveHandler(processMoveAction);

        //落とした(=Spaceキー押下)と検知し、イベントを呼ぶクラス
        fallAction = new PlayerFallHandler();

    }

    private void MoveCommand()
    {
        /*１．
         */

        //Playerを動かす処理 → マウスの操作で x 軸の変更
        var inputMoveValue = PlayerInput.MousePositionValue();
        moveAction.Execute(inputMoveValue);

        //パズルピースを落とす処理 → Spaceキーを押したらイベントを呼ぶ
        var inputFallValue = PlayerInput.PieceObjectDropInput();
        fallAction.Execute(inputFallValue);
    }
    private void TestInputReception()
    {
        //設定ボタンの表示切り替え → Left,RightShiftを押したら
        if (PlayerInput.OptionButton() == 1)
        {
            Debug.Log(objectReference.gameSceneUI);
            if(SceneManager.GetActiveScene().name == SceneName.GameScene.ToString())
            {
                ObjectManager.Instance.Get<GameSceneUI>(ReferenceObjectName.Canvas_GameScene.ToString(), objectReference.gameSceneUI).OptionButtonDisplay();
            }
            //gameSceneButton.OptionButtonDisplay();
        }
    }
}
