using DogDatabase;
using Microsoft.Data.SqlClient;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Windows.Input;

namespace DogViewer;

public partial class TopRatedPage : ContentPage
{
    private List<Dog> _topRatedDogs;

    public TopRatedPage()
	{
		InitializeComponent();
        crslView.PositionChanged += OnPositionChanged; 
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        try
        {
            _topRatedDogs = App.Data.SelectTopDogs(5);
            crslView.ItemsSource = _topRatedDogs;
            SetImageSource(_topRatedDogs[0]);
        }
        catch (SqlException ex)
        {
            crslView.ItemsSource = new List<Dog>() { App.DefaultDog };
            SetImageSource(App.DefaultDog);
        }   
    }

    public void OnPositionChanged(object sender, PositionChangedEventArgs e)
    {
		SetImageSource(_topRatedDogs[e.CurrentPosition]);
    }

    private void NavigateRight(object sender, EventArgs e)
    {
		if(crslView.Position < 4)
			crslView.ScrollTo(crslView.Position + 1);
    }

    private void NavigateLeft(object sender, EventArgs e)
    {
		if(crslView.Position > 0)
			crslView.ScrollTo(crslView.Position - 1);
    }

	private async void SetImageSource(Dog currentDog)
	{
        imgCrslView.Source = await App.Client.AsyncFetchBreedImage(currentDog.BreedName, currentDog.SubBreed);
    }
}