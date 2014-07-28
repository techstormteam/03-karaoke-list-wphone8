using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Phone.Speech.Recognition;
using Windows.Phone.Speech.Synthesis;
using KaraokeList;

namespace KaraokeList.Views
{
    public partial class PageSongSearch : PhoneApplicationPage
    {
        private BitmapImage seachImageSource = new BitmapImage(new Uri(@"/Assets/Images/feature.search.png", UriKind.Relative));
        private BitmapImage clearImageSource = new BitmapImage(new Uri(@"/Assets/Images/feature.clear.png", UriKind.Relative));

        private SpeechSynthesizer synthesizer = Speech.synthesizer;
        private SpeechRecognizer recognizer = App.ViewModelSongProperty.Recognizer;

        private IAsyncOperation<SpeechRecognitionResult> _recoOperation;    // Used to canel the current asynchronous speech recognition operation
        private bool _recoEnabled = false;                                  // When this is true, we will continue to recognize 

        public PageSongSearch()
        {
            InitializeComponent();
        }

        private void btnActionIcon_Tapped(object sender, EventArgs e)
        {
            tbWordSearching.Text = "";
        }

        private void tbWordSearching_TextChanged(object sender, RoutedEventArgs e)
        {
            PhoneTextBox tbWordSearching = (PhoneTextBox)sender;
            string searchString = tbWordSearching.Text;


            if (String.IsNullOrEmpty(searchString))
            {
                tbWordSearching.ActionIcon = seachImageSource;
                //moveToItem(0);
                return;
            }
            tbWordSearching.ActionIcon = clearImageSource;

            //newHook = searchAPI.getHook(searchString);
            //if (newHook != null)
            //{
            //    moveToItem(newHook.Data.fromPosition);
            //}

        }

        private async void btnVoiceSearch_Tap(object sender, EventArgs e)
        {
            // Change the button text. 
            if (this._recoEnabled)
            {
                stopVoiceRecognition();
                return;
            }
            else
            {
                // Set the flag to say that we are in recognition mode
                _recoEnabled = true;

                // Update the UI
                grpInstruction.Visibility = System.Windows.Visibility.Visible;
            }

            try
            {
                // Perform speech recognition.  
                lblInstruction.Text = "Speak some words to search. Listening to you...";
                lblResult.Text = "";
                _recoOperation = recognizer.RecognizeAsync();
                var recoResult = await this._recoOperation;

                // Check the confidence level of the speech recognition attempt.
                if (recoResult.TextConfidence == SpeechRecognitionConfidence.Rejected)
                {
                    // If the confidence level of the speech recognition attempt is low, 
                    // ask the user to try again.
                    lblInstruction.Text = "Not sure what you said, please try again";
                    await synthesizer.SpeakTextAsync("Not sure what you said, please try again");
                }
                else
                {
                    // Output that the color of the rectangle is changing by updating
                    // the TextBox control and by using text-to-speech (TTS).
                    lblInstruction.Text = "Heard you say:  ";
                    lblResult.Text = recoResult.Text;
                    await synthesizer.SpeakTextAsync("Heard you say: " + recoResult.Text);

                    // Set the fill color of the rectangle to the recognized color. 
                    tbWordSearching.Text = recoResult.Text.ToLower();

                }
            }
            catch (System.Threading.Tasks.TaskCanceledException)
            {
                // Ignore the cancellation exception of the recoOperation.
                // When recoOperation.Cancel() is called to cancel the asynchronous speech recognition operation
                // initiated by RecognizeAsync(),  a TaskCanceledException is thrown to signify early exit.
            }
            catch (Exception)
            {

            }
            stopVoiceRecognition();
        }

        private void stopVoiceRecognition()
        {
            if (_recoEnabled == false)
            {
                return;
            }
            // Update the UI to the initial state
            _recoEnabled = false;
            grpInstruction.Visibility = System.Windows.Visibility.Collapsed;

            //btnVoiceSearch.Background = new SolidColorBrush(Colors.Red);

            // Cancel the outstanding recognition operation, if one exists
            if (_recoOperation != null && _recoOperation.Status == AsyncStatus.Started)
            {
                _recoOperation.Cancel();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            
        }

        private async void btnVoiceSearch_Click(object sender, EventArgs e)
        {
            // Change the button text. 
            if (this._recoEnabled)
            {
                stopVoiceRecognition();
                return;
            }
            else
            {
                // Set the flag to say that we are in recognition mode
                _recoEnabled = true;

                // Update the UI
                grpInstruction.Visibility = System.Windows.Visibility.Visible;
            }

            try
            {
                // Perform speech recognition.  
                lblInstruction.Text = "Nói những từ bạn muốn tìm. Đang lắng nghe bạn...";
                lblResult.Text = "";
                _recoOperation = recognizer.RecognizeAsync();
                var recoResult = await this._recoOperation;

                // Check the confidence level of the speech recognition attempt.
                if (recoResult.TextConfidence == SpeechRecognitionConfidence.Rejected)
                {
                    // If the confidence level of the speech recognition attempt is low, 
                    // ask the user to try again.
                    lblInstruction.Text = "Chưa nghe được lời bạn nói, vui lòng thử lại";
                    await synthesizer.SpeakTextAsync("Not sure what you said, please try again");
                }
                else
                {
                    // Output that the color of the rectangle is changing by updating
                    // the TextBox control and by using text-to-speech (TTS).
                    lblInstruction.Text = "Bạn đã nói:  ";
                    lblResult.Text = recoResult.Text;
                    await synthesizer.SpeakTextAsync("Heard you say: " + recoResult.Text);

                    // Set the fill color of the rectangle to the recognized color. 
                    tbWordSearching.Text = recoResult.Text.ToLower();

                }
            }
            catch (System.Threading.Tasks.TaskCanceledException)
            {
                // Ignore the cancellation exception of the recoOperation.
                // When recoOperation.Cancel() is called to cancel the asynchronous speech recognition operation
                // initiated by RecognizeAsync(),  a TaskCanceledException is thrown to signify early exit.
            }
            catch (Exception)
            {

            }
            stopVoiceRecognition();
        }

        private void tbWordSearching_KeyUp(object sender, KeyEventArgs e)
        {
            //if (isSearching() && e.Key == Key.Enter)
            //{
            //    App.EntryViewModelProperty.SelectedEntry = App.EntryViewModelProperty.Entries[newHook.Data.fromPosition];
            //    gotoMeanings();
            //}
        }

        private void llsWordList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (llsWordList.SelectedItem == null)
            {
                return;
            }
            //App.EntryViewModelProperty.SelectedEntry = (DictionaryEntry)llsWordList.SelectedItem;
            //gotoMeanings();

            llsWordList.SelectedItem = null;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Set the data context of the listbox control to the sample data
            llsWordList.ItemsSource = App.ViewModelSongProperty.EntriesData;
        }

    }
}