using FilipAndreiLab7.Models;
namespace FilipAndreiLab7;

public partial class ListPage : ContentPage
{
    public ListPage()
    {
        InitializeComponent();
    }

    async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var slist = (ShopList)BindingContext;
        slist.Date = DateTime.UtcNow;
        await App.Database.SaveShopListAsync(slist);
        await Navigation.PopAsync();
    }
    async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var slist = (ShopList)BindingContext;
        await App.Database.DeleteShopListAsync(slist);
        await Navigation.PopAsync();
    }

    async void OnChooseButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ProductPage((ShopList)
        this.BindingContext)
        {
            BindingContext = new Product()
        });

    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var shopl = (ShopList)BindingContext;

        listView.ItemsSource = await App.Database.GetListProductsAsync(shopl.ID);
    }

    async void OnDeleteItemButtonClicked(object sender, EventArgs e)
    {
        var selectedProduct = listView.SelectedItem as Product;
        var shopList = (ShopList)BindingContext;

        if (selectedProduct != null)
        {
            var listProduct = await App.Database.GetListProductAsync(shopList.ID, selectedProduct.ID);
            if (listProduct != null)
            {
                await App.Database.DeleteListProductAsync(listProduct);
                listView.ItemsSource = await App.Database.GetListProductsAsync(shopList.ID);
            }
            else
            {
                await DisplayAlert("Error", "Product not found in the list", "OK");
            }
        }
        else
        {
            await DisplayAlert("Error", "No product selected", "OK");
        }

    }
}