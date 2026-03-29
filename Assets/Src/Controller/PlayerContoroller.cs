using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerContoroller : MonoBehaviour
{
    [SerializeField] PlayableObject playableObject;
    //[SerializeField] GameSceneUI gameSceneButton;
    public ObjectReference objectReference;

    private PlayerInput input; //入力処理クラス
    private float moveInput;
    // Start is called before the first frame update
    void Start()
    {
        input = new PlayerInput();
    }

    // Update is called once per frame
    void Update()
    {
        InputPlayerControll();
        InputGame();
        //絶対消す
        if(SceneManager.GetActiveScene().name == SceneType.GameScene.ToString())
        {
            if (Input.GetKey(KeyCode.M))
            {
                ObjectEventManager.Instance.TrantitionGameToResultEvent();
            }
        }
    }

    private void InputPlayerControll()
    {
        if (!GameManager.Instance.isPlayerControll) return;
        //Pieceオブジェクトを落とす処理 → Spaceキーを押したら
        if (input.ObjectDropInput() == 1)
        {
            playableObject.PieceObjectDrop();
        }

        //PlayableObjectを動かす処理 → マウスの操作で x 軸の変更
        moveInput = input.MousePositionValue();
        playableObject.ObjectMove(moveInput);
    }
    private void InputGame()
    {
        //設定ボタンの表示切り替え → Left,RightShiftを押したら
        if (input.OptionButton() == 1)
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
