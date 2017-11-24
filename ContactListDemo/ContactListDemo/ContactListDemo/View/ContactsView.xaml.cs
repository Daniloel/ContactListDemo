namespace ContactListDemo.View
{
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactsView : ContentPage
    {
        public ContactsView()
        {
            try
            {
                InitializeComponent();
                BindingContext = new ViewModel.ContactViewModel();
            }
            catch (System.Exception ex)
            {

            }
        }
    }
}