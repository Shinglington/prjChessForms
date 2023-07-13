using System.Diagnostics;
using System.Threading;

namespace prjChessForms.MyChessLibrary
{
    class PromotionHandler : IPromotionHandler
    {
        private bool _awaitingPromotionInput;
        private SemaphoreSlim _semaphoreReceiveClick;
        private PromotionOption _selectedPromotion;
        public PromotionHandler()
        {
            _awaitingPromotionInput = false;
            _semaphoreReceiveClick = new SemaphoreSlim(0, 1);
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
