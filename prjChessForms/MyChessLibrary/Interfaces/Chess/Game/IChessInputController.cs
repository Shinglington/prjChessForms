namespace prjChessForms.MyChessLibrary
{
    public interface IChessInputController
    {
        void ConnectHandlers(IMoveHandler moveHandler, IPromotionHandler promotionHandler);
        void OnBoardClickReceived(object sender, SquareClickedEventArgs e);
        void OnPromotionReceived(object sender, PromotionSelectedEventArgs e);
    }
}
