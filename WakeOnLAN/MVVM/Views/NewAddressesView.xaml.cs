using WakeOnLAN.MVVM.ViewModel;

namespace WakeOnLAN.MVVM.Views;

public partial class NewAddressesView : ContentPage
{
	public NewAddressesView()
	{
		InitializeComponent();
		BindingContext =  new NewAddressesViewModel();
	}
}