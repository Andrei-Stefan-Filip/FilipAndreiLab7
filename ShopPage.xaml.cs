using FilipAndreiLab7.Models;
using Microsoft.Maui.Devices.Sensors;
using Plugin.LocalNotification;
namespace FilipAndreiLab7;

public partial class ShopPage : ContentPage
{
	public ShopPage()
	{
		InitializeComponent();
	}

    async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var shop = (Shop)BindingContext;
        await App.Database.SaveShopAsync(shop);
        await Navigation.PopAsync();
    }
    async void OnShowMapButtonClicked(object sender, EventArgs e)
    {
        var shop = (Shop)BindingContext;
        var address = shop.Adress;
        var locations = await Geocoding.GetLocationsAsync(address);

        var options = new MapLaunchOptions
        {
            Name = "Magazinul meu preferat" 
        };

        var shoplocation = locations?.FirstOrDefault();
        var myLocation = await Geolocation.GetLocationAsync();
        //var myLocation = new Location(46.7731796289, 23.6213886738);
        var distance = myLocation.CalculateDistance(shoplocation, 
            DistanceUnits.Kilometers);
        if (distance < 25)
        {
            var request = new NotificationRequest
            {
                Title = "Ai de facut cumparaturi in apropiere!",
                Description = address,
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = DateTime.Now.AddSeconds(1)
                }
            };
            LocalNotificationCenter.Current.Show(request);
        }

        await Map.OpenAsync(shoplocation, options);
        }

    async void OnDeleteShopButtonClicked(object sender, EventArgs e)
    {
        var shop = (Shop)BindingContext;
        bool confirm = await DisplayAlert("Confirm Delete",
        "Are you sure you want to delete this shop?", "Yes", "No");

        if (confirm)
        {
            await App.Database.DeleteShopAsync(shop);
            await Navigation.PopAsync();
        }
    }
}