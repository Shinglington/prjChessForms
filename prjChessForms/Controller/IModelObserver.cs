using prjChessForms.MyChessLibrary;
using System;


namespace prjChessForms.Controller
{
    interface IModelObserver
    {
        void OnModelChanged(object sender, ModelChangedEventArgs e);
    }
}
