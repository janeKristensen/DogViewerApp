namespace DogViewer.Pages;

public class Test : ContentPage
{
    public Test()
    {
        InitializeControls();
    }

    private void InitializeControls()
    {
        Label title = Utils.LabelCreator.CreateLabel();
        title.Text = "Test title";

        Utils.LabelCreator = new Models.SubtitleLabelCreator();
        Label subtitle = Utils.LabelCreator.CreateLabel();
        subtitle.Text = "Test subtitle";

        var stackLayout = new StackLayout
        {
            HorizontalOptions = LayoutOptions.Center
        };

        stackLayout.Children.Add(title);
        stackLayout.Children.Add(subtitle);
        Content = stackLayout;
    }
}