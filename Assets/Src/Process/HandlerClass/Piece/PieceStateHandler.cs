using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceStateHandler : IStateHandler
{
    private PieceStateController pieceStateController;
    private PieceState pieceState;
    public PieceStateHandler(PieceStateController pieceStateController)
    {
        this.pieceStateController = pieceStateController;
    }
    public void Execute(int stateNumber)
    {
        pieceStateController.ChangeExecutionPieceState(pieceState);
    }
    public int IsDesignationStatus()
    {
        return (int)pieceStateController.GetPieceState();
    }
}
