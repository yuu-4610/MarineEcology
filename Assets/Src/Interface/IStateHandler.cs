using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateHandler
{
    //汎用な状態管理の変更クラス用インターフェース
    //引数を整数型にすることで、enum を使って管理している状態を入れることが可能となる
    void Execute(int stateNumbere);

    int IsDesignationStatus();
}
