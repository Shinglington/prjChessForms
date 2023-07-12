namespace prjChessForms.MyChessLibrary
{
    class PromotionHandler : IPromotionHandler
    {
        private bool _awaitingPromotionInput;
        private PromotionOption _selectedPromotion;

        public PromotionHandler()
        {
            _awaitingPromotionInput = false;
        }
        public bool PromotionNeeded(IChessMove move)
        {
            return false;
        }

        public void ReceivePromotion(PromotionOption option)
        {
            if (_awaitingPromotionInput)
            {
                Debug.WriteLine("Promotion received to {0}", option.ToString());
                _selectedPromotion = option;
                _semaphoreReceiveClick.Release();
            }
        }
    }
}
