using Grammatika.Common;
using Grammatika.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Media.SpeechRecognition;
using Windows.Media.SpeechSynthesis;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Pivot Application template is documented at http://go.microsoft.com/fwlink/?LinkID=391641

namespace Grammatika
{
    public sealed partial class PivotPage : Page
    {
        //private const string FirstGroupName = "FirstGroup";
        //private const string SecondGroupName = "SecondGroup";

        private readonly NavigationHelper navigationHelper;
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();
        private readonly ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView("Resources");

        private SpeechRecognizer _speechRecognizer;
        private SpeechSynthesizer _speechSynth;

        private AutoResetEvent _syncEvent = new AutoResetEvent(false);

        public PivotPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;

            InitializeSpeechRecognizer();
            InitializeSpeechSynthesizer();

            mediaElement.MediaEnded += MediaElement_MediaEnded;
        }

        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            _syncEvent.Set();
        }

        private void InitializeSpeechSynthesizer()
        {
            _speechSynth = new SpeechSynthesizer();

            var voice = SpeechSynthesizer.AllVoices.FirstOrDefault(v =>
                v.Language == SpeechRecognizer.SystemSpeechLanguage.LanguageTag);

            if (voice != null)
                _speechSynth.Voice = voice;
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        public IEnumerable<VWPItem> Verbs { get; set; }

        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>.
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            //// TODO: Create an appropriate data model for your problem domain to replace the sample data
            Verbs = await VWPDataSource.GetAllVerbsAsync();
            this.DefaultViewModel["List"] = Verbs;
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache. Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/>.</param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            // TODO: Save the unique state of the page here.
        }



        /// <summary>
        /// Adds an item to the list when the app bar button is clicked.
        /// </summary>
        private async void AddAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            // Start recognition (load the dictation grammar by default).
            SpeechRecognitionResult r;
            await Say("Ich werde die verben nennen, und du sollst die Preposition sagen.");
            //if (r.Status != SpeechRecognitionResultStatus.Success ||
            //    !string.IsNullOrEmpty(r.Text) && (
            //        !"Ja".Equals(r.Text, StringComparison.OrdinalIgnoreCase) ||
            //        !"Yes".Equals(r.Text, StringComparison.OrdinalIgnoreCase) ||
            //        !"OK".Equals(r.Text, StringComparison.OrdinalIgnoreCase)))
            //{
            //    await Say("Ich dachte, dass du \"ja\", \"yes\" oder \"Okey\", sagst. Ruf mich an, wenn du Lust zu spielen haben wirst.");
            //    return;
            //}
            await Say("Dann fangen wir an!");

            int maxAttempts = 3;

            string[] prepositions;
            string[] systemAnswers = "ende, vertig, stop, weiter, beispiel".Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);

            Regex pExp = new Regex("(j(?:n|m)|etw(?:A|D)|\\||\\s+)");

            string[] prepAnswers = string.Concat(
                "als,", // N
                "ab, außer, aus, bei, gegenüber, mit, nach, seit, von, zu,", // D
                "an, auf, in, über, unter, vor, hinter, zwischen, neben,", // Akk. 
                "wegen, während, infolge, statt, trotz" // G
            ).Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries)
            .Distinct()
            .ToArray();

            var answers = new List<string>(prepAnswers);
            answers.AddRange(systemAnswers);

            foreach (var v in Verbs.Shuffle())
            {
                prepositions = v.Prepositions.Select(p => pExp.Replace(p, "")).ToArray();

                string phrase = v.ToPhrase();
                r = await Ask(phrase, answers); // 1 attempt

                for (var i = 1; i <= maxAttempts; i++)
                {
                    if (r.Status == SpeechRecognitionResultStatus.UserCanceled ||
                        !string.IsNullOrEmpty(r.Text) &&
                           (r.Text.IndexOf("ende", StringComparison.OrdinalIgnoreCase) != -1 ||
                            r.Text.IndexOf("vertig", StringComparison.OrdinalIgnoreCase) != -1))
                    {
                        await Say("Leider, ich möchte noch ein bischen spielen.");
                        return;
                    }

                    if (r.Status == SpeechRecognitionResultStatus.Success && !string.IsNullOrEmpty(r.Text))
                    {
                        if ("weiter".Equals(r.Text, StringComparison.OrdinalIgnoreCase))
                        {
                            string answer = "Richtig Antwort: " + phrase + string.Join(" oder ", prepositions);
                            await Say(answer);
                            break;
                        }
                        if ("beispiel".Equals(r.Text, StringComparison.OrdinalIgnoreCase))
                        {
                            string beispeiel = v.Examples.FirstOrDefault();
                            if (string.IsNullOrEmpty(beispeiel))
                            {
                                r = await Ask("Leider steht kein Beispiel zur Verfügung.", answers);
                            }
                            else
                            {
                                var mockExpression = new Regex(string.Format("(\\b{0}\\b)", string.Join("|", prepositions)));
                                beispeiel = mockExpression.Replace(beispeiel, "omnomnom");
                                r = await Ask(beispeiel, answers);
                            }
                            i--;
                            continue;
                        }

                        var prep = prepositions.FirstOrDefault(p => p.IndexOf(r.Text) != -1);
                        if (prep != null)
                        {
                            await Say("Richtig!");
                            if (prepositions.Length > 1)
                            {
                                string answer = phrase + ": " + string.Join(" oder ", prepositions);
                                await Say(answer);
                            }
                            break;
                        }
                        else
                        {
                            if (i != maxAttempts)
                                r = await Ask("Nein.", answers);
                            else
                            {
                                string answer = "Auch Falsch. Richtig Antwort: " + phrase + string.Join(" oder ", prepositions);
                                await Say(answer);
                            }
                        }
                    }
                    else
                    {
                        if (i != maxAttempts)
                            r = await Ask("Nich verstanden, sag bitte noch ein mal.", answers);
                        else
                            await Say("Sag mal ein bischen deutlicher. Kommen zu nächstem Verb.");
                    }
                }
            }

            await Say("Danke dir. Das war alles.");

            //string groupName = this.pivot.SelectedIndex == 0 ? FirstGroupName : SecondGroupName;
            //var group = this.DefaultViewModel[groupName] as SampleDataGroup;
            //var nextItemId = group.Items.Count + 1;
            //var newItem = new SampleDataItem(
            //    string.Format(CultureInfo.InvariantCulture, "Group-{0}-Item-{1}", this.pivot.SelectedIndex + 1, nextItemId),
            //    string.Format(CultureInfo.CurrentCulture, this.resourceLoader.GetString("NewItemTitle"), nextItemId),
            //    string.Empty,
            //    string.Empty,
            //    this.resourceLoader.GetString("NewItemDescription"),
            //    string.Empty);

            //group.Items.Add(newItem);

            //// Scroll the new item into view.
            //var container = this.pivot.ContainerFromIndex(this.pivot.SelectedIndex) as ContentControl;
            //var listView = container.ContentTemplateRoot as ListView;
            //listView.ScrollIntoView(newItem, ScrollIntoViewAlignment.Leading);
        }

        private async Task Say(string text)
        {
            var stream = await _speechSynth.SynthesizeTextToStreamAsync(text);
            
            CoreDispatcher dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                mediaElement.SetSource(stream, stream.ContentType);
                mediaElement.Play();
            });
            await Task.Run(() =>
            {
                _syncEvent.WaitOne();
            });
            _syncEvent.Reset();
        }

        private async Task<SpeechRecognitionResult> Ask(string question, IEnumerable<string> responses = null)
        {
            return await Ask(question, 0, responses);
        }


        private async Task<SpeechRecognitionResult> Ask(string question, int timeout = 0, IEnumerable<string> responses = null)
        {
            await Say(question);
            _speechRecognizer.UIOptions.AudiblePrompt = question;
            if (timeout != 0)
            {
                _speechRecognizer.Timeouts.InitialSilenceTimeout = new TimeSpan(0, 0, timeout);
            }
            else
            {
                // Set this timeout as Default
                _speechRecognizer.Timeouts.InitialSilenceTimeout = new TimeSpan(0, 0, 30);
            }
            if (responses != null)
            {
                var listConstraint = new Windows.Media.SpeechRecognition.SpeechRecognitionListConstraint(responses, "answers");
                _speechRecognizer.Constraints.Clear();
                _speechRecognizer.Constraints.Add(listConstraint);
                await _speechRecognizer.CompileConstraintsAsync();
            }
            else if (_speechRecognizer.Constraints.Any())
            {
                _speechRecognizer.Constraints.Clear();
                await _speechRecognizer.CompileConstraintsAsync();
            }
            var result = await _speechRecognizer.RecognizeAsync();
            return result;
        }

        /// <summary>
        /// Invoked when an item within a section is clicked.
        /// </summary>
        private void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            var itemId = ((VWPItem)e.ClickedItem).Id;
            if (!Frame.Navigate(typeof(ItemPage), itemId))
            {
                throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
            }
        }

        /// <summary>
        /// Loads the content for the second pivot item when it is scrolled into view.
        /// </summary>
        private async void SecondPivot_Loaded(object sender, RoutedEventArgs e)
        {
            //var sampleDataGroup = await SampleDataSource.GetGroupAsync("Group-2");
            //this.DefaultViewModel[SecondGroupName] = sampleDataGroup;
        }

        private async void InitializeSpeechRecognizer()
        {
            if (_speechRecognizer == null)
            {
                _speechRecognizer = new SpeechRecognizer();
                await _speechRecognizer.CompileConstraintsAsync();
            }
            var options = _speechRecognizer.UIOptions;
            options.IsReadBackEnabled = false;
            options.AudiblePrompt = "I am ready to hear you. Say somethin.";
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
    }
}
