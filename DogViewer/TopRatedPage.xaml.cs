using DogDatabase;
using System.Windows.Input;

namespace DogViewer;

public partial class TopRatedPage : ContentPage
{
	private List<Dog> _topRatedDogs = new();
	private Dog _currentDog = new();

	public TopRatedPage()
	{
		InitializeComponent();
        Load();
        crslView.PositionChanged += OnPositionChanged;
    }

	private void Load()
	{
		using (var db = new DbContextDog())
		{
			var topDogs = db.Dogs.OrderByDescending(d => d.Stars).Take(5);
            foreach (var dog in topDogs)
            {
				_topRatedDogs.Add(dog);
            }
        }
        crslView.ItemsSource = _topRatedDogs;
        _currentDog = _topRatedDogs[0];
        this.SetImageSource();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        using (var db = new DbContextDog())
        {
            var topDogs = db.Dogs.OrderByDescending(d => d.Stars).Take(5);
            foreach (var dog in topDogs)
            {
                _topRatedDogs.Add(dog);
            }
        }
        crslView.ItemsSource = _topRatedDogs;
        this.SetImageSource();
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