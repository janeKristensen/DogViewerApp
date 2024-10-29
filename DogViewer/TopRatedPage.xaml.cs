using DogDatabase;
using Microsoft.Data.SqlClient;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Windows.Input;

namespace DogViewer;

public partial class TopRatedPage : ContentPage
{
	private List<Dog> _topRatedDogs;
	private Dog _currentDog;

	public TopRatedPage()
	{
		InitializeComponent();
        _topRatedDogs = new();
        _currentDog = new();
        Load();
        crslView.PositionChanged += OnPositionChanged; 
    }

	private void Load()
	{
        try
        {
            var topDogs = App.DogContext.Dogs.OrderByDescending(d => d.Stars).Take(5);
            foreach (var dog in topDogs)
            {
                _topRatedDogs.Add(dog);
            }   
        }
        catch (SqlException ex)
        {
            _topRatedDogs.Add(App.DefaultDog);
        }

        crslView.ItemsSource = _topRatedDogs;
        _currentDog = _topRatedDogs[0];
        SetImageSource();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        var topDogs = App.DogContext.Dogs.OrderByDescending(d => d.Stars).Take(5);
        foreach (var dog in topDogs)
        {
            _topRatedDogs.Add(dog);
        }
        
        crslView.ItemsSource = _topRatedDogs;
        SetImageSource();
    }

    public void OnPositionChanged(object sender, PositionChangedEventArgs e)
    {
		_currentDog = _topRatedDogs[e.CurrentPosition];
		SetImageSource();
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

	private async void SetImageSource()
	{
        imgCrslView.Source = await App.Client.AsyncFetchBreedImage(_currentDog.BreedName, _currentDog.SubBreed);
    }
}