using System;
using System.IO;
using System.Collections.Generic;
using Xamarin.Forms;
using Plugin.Media;

namespace CameraTest
{
	public partial class CameraPage : ContentPage
	{
		public CameraPage ()
		{
			InitializeComponent ();
			takePhoto.Clicked += async (sender, args) =>
			{

				if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
				{
					await DisplayAlert("No Camera", "No camera available.", "OK");
					return;
				}
				try{
				var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
					{		
							Name = "test.jpg",
							SaveToAlbum = true
					});

					if (file == null){
						return;
					}

				await DisplayAlert("File Location", file.Path, "OK");
				//Display photo taken
				image.Source = ImageSource.FromStream(() =>
					{
						var stream = file.GetStream();
						file.Dispose();
						return stream;
					}); 
				}
				catch(Exception ex){
					await DisplayAlert("Error", "Some Error " + ex, "Ok");
				}
			};
			pickPhoto.Clicked += async (sender, args) =>
			{
				if(!CrossMedia.Current.IsPickPhotoSupported){
					await DisplayAlert("Photos Not Supported", "No permission to photos", "OK");
				}
				try{
					Stream stream = null;
					var file = await CrossMedia.Current.PickPhotoAsync().ConfigureAwait(true);

					if(file == null){
						return;
					}
					stream = file.GetStream();
					file.Dispose();

					image.Source = ImageSource.FromStream(() => stream);
				}
				catch(Exception ex){
					await DisplayAlert("Error", "Some Error " + ex, "OK");
				}
			};
			takeVideo.Clicked += async (sender, args) =>
			{
				if(!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakeVideoSupported)
				{
					await DisplayAlert("No video camera", "No video camera available", "OK");
					return;
				}
				try
				{
					var file = await CrossMedia.Current.TakeVideoAsync(new Plugin.Media.Abstractions.StoreVideoOptions
						{
							Name="test.mp4",
							SaveToAlbum=true
						});
					if (file == null){
						return;
					}
					await DisplayAlert("Video recorded", "Location: " + file.Path, "OK");
					file.Dispose();
				}
				catch(Exception ex)
				{
					await DisplayAlert("Error", "Some Error " + ex, "OK");
				}
			};
			pickVideo.Clicked += async (sender, args) => {
				if (!CrossMedia.Current.IsPickVideoSupported) {
					await DisplayAlert ("Videos not supported", "No permissions to videos", "OK");
					return;
				}
				try 
				{
					var file = await CrossMedia.Current.PickVideoAsync ();
					if (file == null) {
						return;
					}
					await DisplayAlert ("Video selected", "Location: " + file.Path, "OK");
					file.Dispose ();
				} 
				catch (Exception ex) 
				{
					await DisplayAlert ("Error", "Some error " + ex, "OK");
				}
			};
		}
	}
}

