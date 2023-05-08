namespace Shop;

public partial class MainPage : ContentPage
{
	int count = 0;
    bool loaded = false;

    public MainPage()
	{
		InitializeComponent();
	}

	private void OnCounterClicked(object sender, EventArgs e)
	{
		count++;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

		SemanticScreenReader.Announce(CounterBtn.Text);
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (loaded == false)
        {
            loaded = true;
        }
    }
}

