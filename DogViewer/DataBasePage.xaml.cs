
using DogDatabase;
using Microsoft.Data.SqlClient;


namespace DogViewer;

public partial class DataBasePage : ContentPage, IQueryAttributable
{
	private List<Dog>? _breedList;
    private List<Controls.RatingButton> _ratingButtons;

    public DataBasePage()
	{
        InitializeComponent();
		Load();
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("selected"))
        {
            string message = query["selected"].ToString();
            string[] breed = message.Split("-");
            if (breed.Count() == 1)
                DisplayData(new Dog(breed[0], ""));
            else 
                DisplayData(new Dog(breed[0], breed[1]));
        }    
    }


    public void Load()
	{
        try
        {
            _breedList = App.Client.DogBreedList;
        }
        catch (Exception ex) 
        { 
            _breedList = new List<Dog>() { App.DefaultDog }; 
        }
		 
        lstViewDatabase.ItemsSource = _breedList;
        _ratingButtons = new List<Controls.RatingButton>()
        {
            imgBtnOneStar,
            imgBtnTwoStar,
            imgBtnThreeStar,
            imgBtnFourStar,
            imgBtnFiveStar,
        };
        SetRatingVisibility((Dog)lstViewDatabase.SelectedItem);
    }

	private void SetRatingVisibility(Dog selectedDog)
	{
       
		if (selectedDog == null)
		{
			lblRateDog.IsVisible = false;
            _ratingButtons.ForEach(x => x.IsVisible = false);
		}
		else
		{
            lblRateDog.IsVisible = true;
            _ratingButtons.ForEach(x => x.IsVisible = true);
            _ratingButtons.ForEach(x => x.IsEnabled = true);

            var app = Application.Current as App; 
            if (app.RatedDogs != null)
			{
				if (app.RatedDogs.Find(x => x.Id == selectedDog.Id) != null)
				{
                    _ratingButtons.ForEach(x => x.IsEnabled = false);
                }
			}
		}
	}

    private void DisplayDataOnSelected(object sender, SelectedItemChangedEventArgs e)
    {
		Dog selection = (Dog)e.SelectedItem;
        DisplayData(selection);
    }

    public void DisplayData(Dog selection)
    {
        if (selection != null)
        {
            var selectedDog = selection;
            try
            {
                selectedDog = App.DogContext.Dogs.Where(d => d.BreedName == selection.BreedName && d.SubBreed == selection.SubBreed).First();
            }
            catch (SqlException ex)
            {
                selectedDog = App.DefaultDog;
                App.AlertService.Alert("Connection error", "Could not connect to the database.", new bool[] { true, false, false });
            }

            SetInformationFromSelected(selectedDog);
            SetRatingVisibility(selectedDog);
        }
    }

    private async void SetInformationFromSelected(Dog selectedDog)
    {
        string imgSource = await App.Client.AsyncFetchBreedImage(selectedDog.BreedName, selectedDog.SubBreed);
        if (imgSource == "default")
            DatabaseDogPhotoImg.Source = "default_dogs.png";
        else
            DatabaseDogPhotoImg.Source = imgSource;

        if (selectedDog.SubBreed == "")
            lblDataSubBreed.Text = "N/A";
        else
            lblDataSubBreed.Text = selectedDog.SubBreed;

        lblDataBreed.Text = selectedDog.BreedName;
        lblDataAge.Text = selectedDog.AverageAge.ToString();
        lblDataCoatLength.Text = selectedDog.CoatLength;
        lblDataExcersize.Text = selectedDog.ExcersizeLevel.ToString();
        lblDataSize.Text = selectedDog.Size;
        lblDataTemper.Text = selectedDog.Temper;

        if (selectedDog.Ratings == 0)
            lblDataScore.Text = "No rating";
        else
            lblDataScore.Text = String.Format("{0:N1} stars", selectedDog.GetRating());
    }

    private void SearchDatabase(object sender, TextChangedEventArgs e)
    {
        lstViewDatabase.ItemsSource = _breedList.FindAll(x => x.BreedName.StartsWith(e.NewTextValue) || x.SubBreed.StartsWith(e.NewTextValue));
        SetRatingVisibility((Dog)lstViewDatabase.SelectedItem);
    }

    private void AddRating(object sender, EventArgs e)
    {
        var button = (Controls.RatingButton)sender;
        UpdateRating(button.RatingValue, (Dog)lstViewDatabase.SelectedItem);
    }


	private void UpdateRating(int value, Dog selectedDog)
	{
        selectedDog.AddRating(value);

        //App keeps a list of already rated dogs
        var app = Application.Current as App;

        Dog dog = new Dog();
		try
		{         
            dog = App.DogContext.Dogs.Where(d => d.BreedName == selectedDog.BreedName && d.SubBreed == selectedDog.SubBreed).First();
            if (dog != null)
            {
                dog.Score = selectedDog.Score;
                dog.Ratings = selectedDog.Ratings;
                dog.Stars = selectedDog.GetRating();
                App.DogContext.SaveChanges();
                app.RatedDogs.Add(dog);
            } 
        }
        catch (SqlException ex) 
        { 
            App.DefaultDog.AddRating(value);
            app.RatedDogs.Add(App.DefaultDog); 
        }

        lblDataScore.Text = String.Format("{0:N1} stars", dog.GetRating());
        SetRatingVisibility(dog);
    }
}