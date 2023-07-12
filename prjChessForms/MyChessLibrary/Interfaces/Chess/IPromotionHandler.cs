namespace prjChessForms.MyChessLibrary
{
    public interface IPromotionHandler
    {
        bool PromotionNeeded(IChessMove move);
        void SendPromotion(PromotionOption option);
    }
}