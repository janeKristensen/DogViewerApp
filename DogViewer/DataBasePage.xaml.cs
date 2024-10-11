using DogDatabase;


namespace DogViewer;

public partial class DataBasePage : ContentPage
{
	private Dog? _selectedDog;
	private List<Dog>? _breedList;
	public DataBasePage()
	{
        InitializeComponent();
		Load();
    }

	public void Load()
	{
		 _breedList = App.Client.DogBreedList;
        lstViewDatabase.ItemsSource = _breedList;
		this.SetRatingVisibility();
    }

	private void SetRatingVisibility()
	{
		if (_selectedDog == null)
		{
			lblRateDog.IsVisible = false;
			imgBtnOneStar.IsVisible = false;
			imgBtnTwoStar.IsVisible = false;
			imgBtnThreeStar.IsVisible = false;
			imgBtnFourStar.IsVisible = false;
			imgBtnFiveStar.IsVisible = false;
		}
		else
		{
            lblRateDog.IsVisible = true;
            imgBtnOneStar.IsVisible = true;
			imgBtnTwoStar.IsVisible = true;
			imgBtnThreeStar.IsVisible = true;
			imgBtnFourStar.IsVisible = true;
			imgBtnFiveStar.IsVisible = true;
		}
	}

    private async void DisplayDataOnSelected(object sender, SelectedItemChangedEventArgs e)
    {
		Dog selected = (Dog)e.SelectedItem;

		if (selected != null)
		{
			using (var db = new DbContextDog())
			{
				_selectedDog = db.Dogs.Where(d => d.BreedName == selected.BreedName && d.SubBreed == selected.SubBreed).First();
			}

			DatabaseDogPhotoImg.Source = await App.Client.AsyncFetchBreedImage(_selectedDog.BreedName, _selectedDog.SubBreed);

			if (_selectedDog.SubBreed == "")
				lblDataSubBreed.Text = "N/A";
			else
				lblDataSubBreed.Text = _selectedDog.SubBreed;
			lblDataBreed.Text = _selectedDog.BreedName;
			lblDataAge.Text = _selectedDog.AverageAge.ToString();
			lblDataCoatLength.Text = _selectedDog.CoatLength;
			lblDataExcersize.Text = _selectedDog.ExcersizeLevel.ToString();
			lblDataSize.Text = _selectedDog.Size;
			lblDataTemper.Text = _selectedDog.Temper;

			this.SetRatingVisibility();
        }
    }

    private void SearchDatabase(object sender, TextChangedEventArgs e)
    {
		if(_breedList != null)
			lstViewDatabase.ItemsSource = _breedList.FindAll(x => x.BreedName.StartsWith(e.NewTextValue) || x.SubBreed.StartsWith(e.NewTextValue));
		else
            this.SetRatingVisibility();
    }

    private void AddRatingOne(object sender, EventArgs e)
    {

    }

    private void AddRatingTwo(object sender, EventArgs e)
    {

    }

    private void AddRatingThree(object sender, EventArgs e)
    {

    }

    private void AddRatingFour(object sender, EventArgs e)
    {

    }

    private void AddRatingFive(object sender, EventArgs e)
    {

    }
}