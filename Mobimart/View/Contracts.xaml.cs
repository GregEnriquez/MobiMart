using System.Globalization;
using System.Text.RegularExpressions;
using CommunityToolkit.Maui.Core;
using MobiMart.ViewModel;

namespace MobiMart.View;

public partial class Contracts : ContentPage
{
    public Contracts(ContractsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }


    // private async void CaptureButtonClicked(object sender, EventArgs e)
    // {
    //     await Camera.CaptureImage(CancellationToken.None);
    // }

    // private async void CameraMediaCaptured(object sender, MediaCapturedEventArgs e)
    // {
    //     if (e.Media is Stream capturedStream)
    //     {
    //         if (BindingContext is ContractsViewModel vm)
    //         {
    //             await vm.CameraMediaCaptured(capturedStream);
    //             Camera.IsVisible = false;
    //             CapturedImage.IsVisible = true;
    //         }
    //         // using var memoryStream = new MemoryStream();
    //         // await capturedStream.CopyToAsync(memoryStream);
    //         // byte[] imageData = memoryStream.ToArray();

    //         // if (BindingContext is ContractsViewModel vm)
    //         // {
    //         //     vm.ImageBytes = [.. imageData];
    //         // }
    //         // Camera.IsVisible = false;
    //         // CapturedImage.IsVisible = true;
    //         // Now you can save the `imageData` to your object
    //     }
    // }


    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is ContractsViewModel vm)
        {
            await vm.OnAppearing();
        }
    }
}


// public class BytesToStreamConverter : IValueConverter
// {
//     public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
//     {
//         if (value is byte[] bytes)
//             return new MemoryStream(bytes);
//         return null;
//     }

//     public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
// }