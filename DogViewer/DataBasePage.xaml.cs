using DogDatabase;


namespace DogViewer;

public partial class DataBasePage : ContentPage
{
	private Dog? _selectedDog;
	public DataBasePage()
	{
        InitializeComponent();
		Load();
    }

	public void Load()
	{
		lstViewDatabase.ItemsSource = App.Client.DogBreedList;
	}

    private void DisplayDataOnSelected(object sender, SelectedItemChangedEventArgs e)
    {
		Dog selected = (Dog)e.SelectedItem;

        using (var db = new DbContextDog())
		{
			_selectedDog = db.Dogs.Where(d => d.BreedName == selected.BreedName).First();
		}

		lblDataBreed.Text = _selectedDog.BreedName;
        lblDataSubBreed.Text = _selectedDog.SubBreed;
    }
}