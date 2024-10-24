using DogDatabase;
using Microsoft.Data.SqlClient;


namespace DogViewer;

public partial class DataBasePage : ContentPage
{
	private Dog? _selectedDog;
	private List<Dog>? _breedList;
    private List<ImageButton> _ratingButtons;
    private App _app = (App)Application.Current;

    public DataBasePage()
	{
        InitializeComponent();
		Load();
    }

	public void Load()
	{
		 _breedList = App.Client.DogBreedList;
        lstViewDatabase.ItemsSource = _breedList;
        _ratingButtons = new List<ImageButton>()
        {
            imgBtnOneStar,
            imgBtnTwoStar,
            imgBtnThreeStar,
            imgBtnFourStar,
            imgBtnFiveStar,
        };
        SetRatingVisibility();
    }

	private void SetRatingVisibility()
	{
       
		if (_selectedDog == null)
		{
			lblRateDog.IsVisible = false;
            _ratingButtons.ForEach(x => x.IsVisible = false);
		}
		else
		{
            lblRateDog.IsVisible = true;
            _ratingButtons.ForEach(x => x.IsVisible = true);
            _ratingButtons.ForEach(x => x.IsEnabled = false);

            if (_app.RatedDogs != null)
			{
				if (_app.RatedDogs.Find(x => x.Id == _selectedDog.Id) != null)
				{
                    _ratingButtons.ForEach(x => x.IsEnabled = false);
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
                App.AlertService.Alert("Connection error", "Could not connect to the database.");
                
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

			if (_selectedDog.Ratings == 0)
				lblDataScore.Text = "No rating";
			else
                lblDataScore.Text = String.Format("{0:N1} stars", _selectedDog.GetRating());

            SetRatingVisibility();
        }
    }

    private void SearchDatabase(object sender, TextChangedEventArgs e)
    {
		if(_breedList != null)
            lstViewDatabase.ItemsSource = _breedList.FindAll(x => x.BreedName.StartsWith(e.NewTextValue) || x.SubBreed.StartsWith(e.NewTextValue));
 
        SetRatingVisibility();
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

        //Dogs can only be rated once per instance of app
		_app.RatedDogs.Add(_selectedDog);

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
            _app.RatedDogs.Add(App.DefaultDog); 
        }

        lblDataScore.Text = String.Format("{0:N1} stars", _selectedDog.GetRating());
        SetRatingVisibility();
    }
}