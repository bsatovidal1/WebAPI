using ArtworksClient_BrunoVidal.Data;
using ArtworksClient_BrunoVidal.Models;
using ArtworksClient_BrunoVidal.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ArtworksClient_BrunoVidal
{
    /// <summary>
    /// Details page, can be used to insert, update and delete an Artwork.
    /// </summary>
    public sealed partial class ArtworkDetailPage : Page
    {
        Artwork view;
        IArtTypeRepository artTypeRepository;
        IArtworkRepository artworkRepository;
        bool InsertMode;

        public ArtworkDetailPage()
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
                List<ArtType> arttypes = await artTypeRepository.GetArtTypes();
                //Bind to the ComboBox
                ArtTypeCombo.ItemsSource = arttypes.OrderBy(a => a.Type);
                this.DataContext = view;
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            view = (Artwork)e.Parameter;

            if (view.ID == 0) //Inserting
            {
                //Disable the delete button if adding
                btnDelete.IsEnabled = false;
                InsertMode = true;
            }
            else
            {
                InsertMode = false;
            }
        }
        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string errMessage = "Please fix the following errors:"
                + Environment.NewLine + Environment.NewLine;

            var context = new ValidationContext(view, null, null);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(view, context, results, true);
            if (!isValid)
            {
                foreach (var error in results)
                {
                    foreach (var memberName in error.MemberNames)
                    {
                        errMessage += memberName + " - "
                            + error.ErrorMessage + Environment.NewLine;
                    }
                }
                Jeeves.ShowMessage("Validaiton", errMessage);
            }
            else //We are good to try and save
            {
                try
                {
                    if (InsertMode)
                    {
                        await artworkRepository.AddArtwork(view);
                    }
                    else
                    {
                        await artworkRepository.UpdateArtwork(view);
                    }
                    Frame.GoBack();
                }
                catch (AggregateException aex)
                {
                    string errMsg = "Errors:" + Environment.NewLine;
                    foreach (var exception in aex.InnerExceptions)
                    {
                        errMsg += Environment.NewLine + exception.Message;
                    }
                    Jeeves.ShowMessage("One or more exceptions has occurred:", errMsg);
                }
                catch (ApiException apiEx)
                {
                    string errMsg = "Errors:" + Environment.NewLine;
                    foreach (var error in apiEx.Errors)
                    {
                        errMsg += Environment.NewLine + "-" + error;
                    }
                    Jeeves.ShowMessage("Problem Saving the Record:", errMsg);
                }
                catch (Exception ex)
                {
                    if (ex.GetBaseException().Message.Contains("connection with the server"))
                    {
                        Jeeves.ShowMessage("Error", "No connection with the server.");
                    }
                    else
                    {
                        Jeeves.ShowMessage("Error", "Could not complete operation.");
                    }
                }
            }
            
        }
        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            string strTitle = "Confirm Delete";
            string strMsg = "Are you certain that you want to delete " + view.Summary + "?";
            ContentDialogResult result = await Jeeves.ConfirmDialog(strTitle, strMsg);
            if (result == ContentDialogResult.Secondary)
            {
                try
                {
                    await artworkRepository.DeleteArtwork(view);
                    Frame.GoBack();
                }
                catch (AggregateException aex)
                {
                    string errMsg = "Errors:" + Environment.NewLine;
                    foreach (var exception in aex.InnerExceptions)
                    {
                        errMsg += Environment.NewLine + exception.Message;
                    }
                    Jeeves.ShowMessage("One or more exceptions has occurred:", errMsg);
                }
                catch (ApiException apiEx)
                {
                    string errMsg = "Errors:" + Environment.NewLine;
                    foreach (var error in apiEx.Errors)
                    {
                        errMsg += Environment.NewLine + "-" + error;
                    }
                    Jeeves.ShowMessage("Problem Saving the Record:", errMsg);
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
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
