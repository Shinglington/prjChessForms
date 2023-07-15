namespace prjChessForms.MyChessLibrary
{
    public interface ICheckHandler
    {
        void AddRulebook(IRulebook rulebook);
        bool IsInCheck(PieceColour colour);
        bool IsInCheckmate(PieceColour colour);
    }
}
