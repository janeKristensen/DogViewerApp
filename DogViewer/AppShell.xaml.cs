using DogViewer.Pages;

namespace DogViewer
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            var testContent = new ShellContent()
            {
                Title = "Test page",
                ContentTemplate = new DataTemplate(() => new Test()),
                Route = "test"
            };

            Items.Add(new FlyoutItem
            {
                Title = "Test",
                Items = { testContent }
            });

            var tabbar = new TabBar();
            var tab = new Tab()
            {
                Title = "",
                Items =
                {
                    new ShellContent
                    {
                        Title = "Test",
                        ContentTemplate = new DataTemplate(() => new Test()),
                        Route = "test"
                    }
                }
            };
            tabbar.Items.Add(tab);
            Items.Add(tabbar);
        }
    }
}
