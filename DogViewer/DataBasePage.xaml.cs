using DogDatabase;
using Microsoft.Data.SqlClient;


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

            imgBtnOneStar.IsEnabled = true;
            imgBtnTwoStar.IsEnabled = true;
            imgBtnThreeStar.IsEnabled = true;
            imgBtnFourStar.IsEnabled = true;
            imgBtnFiveStar.IsEnabled = true;

            if (App.RatedDogs != null)
			{
				if (App.RatedDogs.Find(x => x.Id == _selectedDog.Id) != null)
				{
					imgBtnOneStar.IsEnabled = false;
					imgBtnTwoStar.IsEnabled = false;
					imgBtnThreeStar.IsEnabled = false;
					imgBtnFourStar.IsEnabled = false;
					imgBtnFiveStar.IsEnabled = false;
				}
			}
		}
	}

    private async void DisplayDataOnSelected(object sender, SelectedItemChangedEventArgs e)
    {
		Dog selected = (Dog)e.SelectedItem;

		if (selected != null)
		{
			try
			{
                using (var db = new DbContextDog())
                {
                    _selectedDog = db.Dogs.Where(d => d.BreedName == selected.BreedName && d.SubBreed == selected.SubBreed).First();
                }
            }
            catch (SqlException ex) 
            { 
                _selectedDog = App.DefaultDog; 
                await App.AlertSvc.DisplayAlert("Connection error", "Could not connect to the database.");
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
			if (_selectedDog.Score == 0)
				lblDataScore.Text = "No rating";
			else
                lblDataScore.Text = String.Format("{0:N1} stars", _selectedDog.GetRating());

            this.SetRatingVisibility();
        }
    }

    private void SearchDatabase(object sender, TextChangedEventArgs e)
    {
		if(_breedList != null)
            lstViewDatabase.ItemsSource = _breedList.FindAll(x => x.BreedName.StartsWith(e.NewTextValue) || x.SubBreed.StartsWith(e.NewTextValue));
 
        this.SetRatingVisibility();
    }

    private void AddRatingOne(object sender, EventArgs e)
    {
        UpdateRating(1);
    }

    private void AddRatingTwo(object sender, EventArgs e)
    {
        UpdateRating(2);
    }

    private void AddRatingThree(object sender, EventArgs e)
    {
        UpdateRating(3);
    }

    private void AddRatingFour(object sender, EventArgs e)
    {
        UpdateRating(4);
    }

    private void AddRatingFive(object sender, EventArgs e)
    {
		UpdateRating(5);
    }

	private void UpdateRating(int value)
	{
        _selectedDog.AddRating(value);
		App.RatedDogs.Add(_selectedDog);

		try
		{
            using (var db = new DbContextDog())
            {
                var dog = db.Dogs.Find(_selectedDog.Id);
                if (dog != null)
                {
                    dog.Score = _selectedDog.Score;
                    dog.Ratings = _selectedDog.Ratings;
                    dog.Stars = _selectedDog.GetRating();
                    db.SaveChanges();
                }
            }
        }
        catch (SqlException ex) 
        { 
            App.DefaultDog.AddRating(value);
            App.RatedDogs.Add(App.DefaultDog); 
        }

        lblDataScore.Text = String.Format("{0:N1} stars", _selectedDog.GetRating());
        this.SetRatingVisibility();
    }
}