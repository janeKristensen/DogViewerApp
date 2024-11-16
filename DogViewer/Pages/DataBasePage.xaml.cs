
using DogDatabase;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;


namespace DogViewer;

public partial class DataBasePage : ContentPage, IQueryAttributable
{
    private List<DogViewer.Models.RatingButton> _ratingButtons;

    public DataBasePage()
	{
        InitializeComponent();
		Load();
    }

    public async void Load()
	{
        List<Dog> breedList = await App.Client.GetBreedsList();
        if (breedList == null)
            breedList = new List<Dog>() { App.DefaultDog };

        lstViewDatabase.ItemsSource = breedList;
        _ratingButtons = new List<DogViewer.Models.RatingButton>()
        {
            imgBtnOneStar,
            imgBtnTwoStar,
            imgBtnThreeStar,
            imgBtnFourStar,
            imgBtnFiveStar,
        };

        SetRatingVisibility((Dog)lstViewDatabase.SelectedItem);
    }

    private void DisplayDataOnSelected(object sender, SelectedItemChangedEventArgs e)
    {
		Dog selection = (Dog)e.SelectedItem;
        SetDisplayData(selection);
    }

    public void SetDisplayData(Dog selection)
    {
        if (selection != null)
        {
            Dog selectedDog = App.Data.FindSelected(selection);
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

    // Method used for passing data from selected dog on mainpage to databasepage
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("selected"))
        {
            string message = query["selected"].ToString();
            string[] breed = message.Split("-");
            if (breed.Count() == 1)
                SetDisplayData(new Dog(breed[0], ""));
            else
                SetDisplayData(new Dog(breed[0], breed[1]));
        }
    }

    private async void SearchDatabase(object sender, TextChangedEventArgs e)
    {
        lstViewDatabase.ItemsSource = await App.Data.FindInDatabase(e.NewTextValue);
        SetRatingVisibility((Dog)lstViewDatabase.SelectedItem);
    }

    private void AddRating(object sender, EventArgs e)
    {
        var button = (DogViewer.Models.RatingButton)sender;
        Dog dog = App.Data.UpdateRating(button.RatingValue, (Dog)lstViewDatabase.SelectedItem);

        Decimal rating;
        if (dog == null)
            rating = App.DefaultDog.GetRating();
        else
            rating = dog.GetRating();

        lblDataScore.Text = String.Format("{0:N1} stars", (double)rating);
        SetRatingVisibility(dog);
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