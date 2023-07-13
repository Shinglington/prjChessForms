using System;

namespace prjChessForms.MyChessLibrary
{
    public class PromotionSelectedEventArgs : EventArgs
    {
        public PromotionSelectedEventArgs(PromotionOption option)
        {
            SelectedOption = option;
        }

        public PromotionOption SelectedOption { get; set; }
    }
}
