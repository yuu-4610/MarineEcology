using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPieceSynthesis
{
    void PieceSynthesisProcess(Transform piece, Transform otherPiece, int fishPieceTypeNumber);
}
