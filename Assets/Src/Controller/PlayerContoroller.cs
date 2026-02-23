using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContoroller : MonoBehaviour
{
    [SerializeField] PlayableObject playableObject;
    [SerializeField] GameButton gameSceneButton;

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
        InputFuctin();
    }

    private void InputFuctin()
    {
        //Pieceオブジェクトを落とす処理 → Spaceキーを押したら
        if (input.ObjectDropInput() == 1)
        {
            playableObject.ObjectDrop();
        }

        //PlayableObjectを動かす処理 → マウスの操作で x 軸の変更
        moveInput = input.MousePositionValue();
        playableObject.ObjectMove(moveInput);

        //設定ボタンの表示切り替え → Left,RightShiftを押したら
        if(input.OptionButton() == 1)
        {
            gameSceneButton.OptionButtonOnClick();
        }
    }
}
