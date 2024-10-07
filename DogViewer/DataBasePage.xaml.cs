
namespace DogViewer;

public partial class DataBasePage : ContentPage
{
	public DataBasePage()
	{
        InitializeComponent();
		Load();
    }

	public void Load()
	{
		lstViewDatabase.ItemsSource = App.Client.DogBreedList;
	}

}