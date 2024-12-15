
using DogDatabase;
using DogViewer.Services;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;


namespace DogViewer;

public partial class DataBasePage : ContentPage, IQueryAttributable
{
    private List<DogViewer.Models.RatingButton> _ratingButtons;
    private Dog _selectedDog;

    public DataBasePage()
	{
        InitializeComponent();
        Load();
    }

    public async void Load()
	{
        _selectedDog = new Dog();
        _ratingButtons = new List<DogViewer.Models.RatingButton>()
        {
            imgBtnOneStar,
            imgBtnTwoStar,
            imgBtnThreeStar,
            imgBtnFourStar,
            imgBtnFiveStar,
        };

        lstViewDatabase.ItemsSource = await App.Data.GetBreedList();
    }

    private void DisplayDataOnSelected(object sender, SelectedItemChangedEventArgs e)
    {
		_selectedDog = (Dog)e.SelectedItem;
        SetDisplayData(_selectedDog);
        ScrollTo();
    }

    private void ScrollToSelected(object sender, EventArgs e)
    {
        ScrollTo();
    }

    private void ScrollTo()
    {
        var scrollItem = lstViewDatabase.ItemsSource.Cast<Dog>().FirstOrDefault(
            x => x.BreedName == _selectedDog.BreedName &&
            x.SubBreed == _selectedDog.SubBreed);
        lstViewDatabase.ScrollTo(scrollItem, ScrollToPosition.MakeVisible, true);
        lstViewDatabase.SelectedItem = scrollItem;
    }

    public void SetDisplayData(Dog selection)
    {
        if (selection != null)
        {
            _selectedDog = App.Data.FindSelected(selection);
            SetInformationFromSelected(_selectedDog);
            SetRatingVisibility(_selectedDog);
        }
    }

    private async void SetInformationFromSelected(Dog selection)
    {
        _selectedDog = selection;
        string imgSource = await App.Client.AsyncFetchBreedImage(_selectedDog.BreedName, _selectedDog.SubBreed);
        if (imgSource == "default")
            DatabaseDogPhotoImg.Source = "default_dogs.png";
        else
            DatabaseDogPhotoImg.Source = imgSource;

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
    }

    // Method for passing data from selected dog on mainpage to databasepage
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("selected"))
        {
            string message = query["selected"].ToString();
            string[] breed = message.Split("-");
            if (breed.Count() == 1)
                _selectedDog = new Dog(breed[0], "");
            else
                _selectedDog = new Dog(breed[0], breed[1]);

            SetDisplayData(_selectedDog);
        }
    }

    private async void SearchDatabase(object sender, TextChangedEventArgs e)
    {
        if (Regex.IsMatch(e.NewTextValue, @"^[a-zA-Z]+\-?\s?[a-zA-Z]*$"))
        {
            lstViewDatabase.ItemsSource = await App.Data.SearchBreedList(e.NewTextValue);
            SetRatingVisibility(_selectedDog);
        }       
    }

    private void AddRating(object sender, EventArgs e)
    {
        var button = (DogViewer.Models.RatingButton)sender;
        Decimal rating = App.Data.UpdateRating(button.RatingValue, _selectedDog);
        lblDataScore.Text = String.Format("{0:N1} stars", (double)rating);
        SetRatingVisibility(_selectedDog);
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
                    _ratingButtons.ForEach(x => x.IsEnabled = false);     
            }
        }
    }
}