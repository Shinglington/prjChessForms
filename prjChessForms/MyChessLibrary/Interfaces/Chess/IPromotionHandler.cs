namespace prjChessForms.MyChessLibrary
{
    public interface IPromotionHandler
    {
        bool PromotionNeeded(IChessMove move);
        void ReceivePromotion(PromotionOption option);
    }
}