using Avalonia.Controls;
using Player.ViewModels;

namespace Player.Views
{
    public partial class PlayerView : UserControl
    {
        public PlayerView()
        {
            InitializeComponent();
            DataContext = new PlayerViewModel();
        }
    }
}
