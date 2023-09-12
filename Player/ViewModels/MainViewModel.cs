using CommunityToolkit.Mvvm.ComponentModel;

namespace Player.ViewModels;

public partial class MainViewModel : ObservableObject
{
    public string Greeting => "Welcome to Avalonia!";
}
