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

    private IPlayerMove moveAction;
    private IPlayerMove fallAction;
    // Start is called before the first frame update

    private void Awake()
    {
        InitializePlayerMoveAction();
    }
    private void OnEnable()
    {
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveCommand();
        TestInputReception();
    }
    private void InitializePlayerMoveAction()
    {
        //PlayerMoveActionはこの場のみ
        var processMoveAction = new PlayerMoveAction(this.gameObject.transform, this.gameObject.transform.position.x, 4);
        moveAction = new PlayerMoveHandler(processMoveAction);

        //落としたと検知し、イベントを呼ぶクラス
        fallAction = new PlayerFallHandler();
    }

    private void MoveCommand()
    {
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
            if(SceneManager.GetActiveScene().name == SceneType.GameScene.ToString())
            {
                ObjectManager.Instance.Get<GameSceneUI>(ReferenceObjectName.Canvas_GameScene.ToString(), objectReference.gameSceneUI).OptionButtonDisplay();
            }
            //gameSceneButton.OptionButtonDisplay();
        }
    }
}
