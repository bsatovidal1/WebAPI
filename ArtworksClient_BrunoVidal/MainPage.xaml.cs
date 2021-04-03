using ArtworksClient_BrunoVidal.Data;
using ArtworksClient_BrunoVidal.Models;
using ArtworksClient_BrunoVidal.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ArtworksClient_BrunoVidal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        IArtTypeRepository artTypeRepository;
        IArtworkRepository artworkRepository;

        public MainPage()
        {
            this.InitializeComponent();
            artTypeRepository = new ApiArtTypeRepository();
            artworkRepository = new ApiArtworkRepository();
            fillDropDown();
        }

        private async void fillDropDown()
        {
            try
            {
                btnAdd.IsEnabled = true;
                List<ArtType> arttypes = await artTypeRepository.GetArtTypes();
                //Add the All Option
                arttypes.Insert(0, new ArtType { ID = 0, Type = " - All ArtTypes" });
                //Bind to the ComboBox
                ArtTypeCombo.ItemsSource = arttypes;
                showArtworks(null);
            }
            catch (Exception ex)
            {
                if (ex.GetBaseException().Message.Contains("connection with the server"))
                {
                    Jeeves.ShowMessage("Error", "No connection with the server.");
                }
                else
                {
                    Jeeves.ShowMessage("Error", "Could not complete operation");
                }
            }
        }
        private async void showArtworks(int? ArtTypeID)
        {
            try
            {
                List<Artwork> artworks;
                if (ArtTypeID.GetValueOrDefault() > 0)
                {
                    artworks = await artworkRepository.GetArtworksByArtType(ArtTypeID.GetValueOrDefault());
                }
                else
                {
                    artworks = await artworkRepository.GetArtworks();
                }
                artworkList.ItemsSource = artworks;

            }
            catch (Exception ex)
            {
                if (ex.GetBaseException().Message.Contains("connection with the server"))
                {
                    Jeeves.ShowMessage("Error", "No connection with the server.");
                }
                else
                {
                    Jeeves.ShowMessage("Error", "Could not complete operation");
                }
            }
        }
        private void ArtTypeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ArtType selType = (ArtType)ArtTypeCombo.SelectedItem;
            showArtworks(selType?.ID);
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            fillDropDown();
        }

        private void artworkList_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Navigate to the detail page
            Frame.Navigate(typeof(ArtworkDetailPage), (Artwork)e.ClickedItem);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Artwork newArt = new Artwork();
            newArt.Completed = DateTime.Now;

            // Navigate to the detail page
            Frame.Navigate(typeof(ArtworkDetailPage), newArt);
        }
    }
}
