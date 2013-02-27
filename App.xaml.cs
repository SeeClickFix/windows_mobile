using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Resources;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SeeClickFix.WP8.Resources;
using SeeClickFix.WP8.ViewModels;
using System.Windows.Media;
using System.Collections;
using SeeClickFix.WP8.Infrastructure;
using SeeClickFix.WP8.Common;
using SeeClickFix.WP8.Services;

namespace SeeClickFix.WP8
{
    public partial class App : Application
    {
        private static MainViewModel viewModel = null;

        /// <summary>
        /// A static ViewModel used by the views to bind against.
        /// </summary>
        /// <returns>The MainViewModel object.</returns>
        public static MainViewModel ViewModel
        {
            get
            {
                // Delay creation of the view model until necessary
                if (viewModel == null)
                    viewModel = new MainViewModel();

                return viewModel;
            }
        }

        /// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public static PhoneApplicationFrame RootFrame { get; private set; }

        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            // Global handler for uncaught exceptions.
            UnhandledException += Application_UnhandledException;

            // Standard XAML initialization
            InitializeComponent();

            // Change default styles
            InitializeStyleChanges();

            MergeCustomColors();

            // Phone-specific initialization
            InitializePhoneApplication();

            StateManager.Initialize();

            // Language display initialization
            InitializeLanguage();

            // Show graphics profiling information while debugging.
            if (Debugger.IsAttached)
            {
                // Display the current frame rate counters.
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // Show the areas of the app that are being redrawn in each frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode,
                // which shows areas of a page that are handed off to GPU with a colored overlay.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;

                // Prevent the screen from turning off while under the debugger by disabling
                // the application's idle detection.
                // Caution:- Use this under debug mode only. Application that disables user idle detection will continue to run
                // and consume battery power when the user is not using the phone.
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }
        }

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {   
            //var uri = AppSettingsService.Inst.Settings.WasLocationServicesConsentDisplayed ?
            //    Constants.MainPageUri : Constants.LocationServicesUserConsentUri;
            //RootFrame.Navigate(uri);
        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            // whether our app has been brought to life from tombstone or dormancy
            if (e.IsApplicationInstancePreserved)
            {
                // it’s coming to life from dormancy
                // we do not need to do anything special.
            }

            // Ensure that application state is restored appropriately
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {

            // Ensure that required application state is persisted here.
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
        }

        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                Debugger.Break();
            }
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            MessageBox.Show("Something unexpected just happened.\nSorry about that.", "SEECLICKFIX", MessageBoxButton.OK);

            if (Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                Debugger.Break();
            }
        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Handle reset requests for clearing the backstack
            RootFrame.Navigated += CheckForResetNavigation;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        private void CheckForResetNavigation(object sender, NavigationEventArgs e)
        {
            // If the app has received a 'reset' navigation, then we need to check
            // on the next navigation to see if the page stack should be reset
            if (e.NavigationMode == NavigationMode.Reset)
                RootFrame.Navigated += ClearBackStackAfterReset;
        }

        private void ClearBackStackAfterReset(object sender, NavigationEventArgs e)
        {
            // Unregister the event so it doesn't get called again
            RootFrame.Navigated -= ClearBackStackAfterReset;

            // Only clear the stack for 'new' (forward) and 'refresh' navigations
            if (e.NavigationMode != NavigationMode.New && e.NavigationMode != NavigationMode.Refresh)
                return;

            // For UI consistency, clear the entire page stack
            while (RootFrame.RemoveBackEntry() != null)
            {
                ; // do nothing
            }
        }

        #endregion

        // Initialize the app's font and flow direction as defined in its localized resource strings.
        //
        // To ensure that the font of your application is aligned with its supported languages and that the
        // FlowDirection for each of those languages follows its traditional direction, ResourceLanguage
        // and ResourceFlowDirection should be initialized in each resx file to match these values with that
        // file's culture. For example:
        //
        // AppResources.es-ES.resx
        //    ResourceLanguage's value should be "es-ES"
        //    ResourceFlowDirection's value should be "LeftToRight"
        //
        // AppResources.ar-SA.resx
        //     ResourceLanguage's value should be "ar-SA"
        //     ResourceFlowDirection's value should be "RightToLeft"
        //
        // For more info on localizing Windows Phone apps see http://go.microsoft.com/fwlink/?LinkId=262072.
        //
        private void InitializeLanguage()
        {
            try
            {
                // Set the font to match the display language defined by the
                // ResourceLanguage resource string for each supported language.
                //
                // Fall back to the font of the neutral language if the Display
                // language of the phone is not supported.
                //
                // If a compiler error is hit then ResourceLanguage is missing from
                // the resource file.
                RootFrame.Language = XmlLanguage.GetLanguage(AppResources.ResourceLanguage);

                // Set the FlowDirection of all elements under the root frame based
                // on the ResourceFlowDirection resource string for each
                // supported language.
                //
                // If a compiler error is hit then ResourceFlowDirection is missing from
                // the resource file.
                FlowDirection flow = (FlowDirection)Enum.Parse(typeof(FlowDirection), AppResources.ResourceFlowDirection);
                RootFrame.FlowDirection = flow;
            }
            catch
            {
                // If an exception is caught here it is most likely due to either
                // ResourceLangauge not being correctly set to a supported language
                // code or ResourceFlowDirection is set to a value other than LeftToRight
                // or RightToLeft.

                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }

                throw;
            }
        }

        private void MergeCustomColors()
        {
            string source = String.Format("/SeeClickFix.WP8;component/Assets/Styles.xaml");
            var dictionaries = new ResourceDictionary();
            var themeStyles = new ResourceDictionary { Source = new Uri(source, UriKind.Relative) };
            dictionaries.MergedDictionaries.Add(themeStyles);

            ResourceDictionary appResources = App.Current.Resources;
            foreach (DictionaryEntry entry in dictionaries.MergedDictionaries[0])
            {
                SolidColorBrush colorBrush = entry.Value as SolidColorBrush;
                SolidColorBrush existingBrush = appResources[entry.Key] as SolidColorBrush;
                if (existingBrush != null && colorBrush != null)
                {
                    existingBrush.Color = colorBrush.Color;
                }
            }
        }

        private void InitializeStyleChanges()
        {
            return;

            //(App.Current.Resources["PhoneAccentBrush"] as SolidColorBrush).Color = Color.FromArgb(255, 191, 70, 73); //red
            //(App.Current.Resources["PhoneForegroundBrush"] as SolidColorBrush).Color = Color.FromArgb(255, 61, 61, 62); //black
            //(App.Current.Resources["PhoneBackgroundBrush"] as SolidColorBrush).Color = Color.FromArgb(255, 250, 250, 245); //white
            //(App.Current.Resources["PhoneContrastBackgroundBrush"] as SolidColorBrush).Color = Color.FromArgb(255, 61, 61, 62); //black
            //(App.Current.Resources["PhoneContrastForegroundBrush"] as SolidColorBrush).Color = Color.FromArgb(255, 250, 250, 245); // white
            //(App.Current.Resources["PhoneDisabledBrush"] as SolidColorBrush).Color = Color.FromArgb(255, 102, 102, 102); //grey
            //(App.Current.Resources["PhoneSubtleBrush"] as SolidColorBrush).Color = Color.FromArgb(255, 250, 250, 245); //white
            //(App.Current.Resources["TransparentBrush"] as SolidColorBrush).Color = Color.FromArgb(255, 250, 250, 245); //white
            //(App.Current.Resources["PhoneSemitransparentBrush"] as SolidColorBrush).Color = Color.FromArgb(255, 250, 250, 245); //white
            //(App.Current.Resources["PhoneChromeBrush"] as SolidColorBrush).Color = Color.FromArgb(255, 250, 250, 245); //white

            //return;

            //87 percent Black - #DE000000
            (App.Current.Resources["PhoneForegroundBrush"] as SolidColorBrush).Color = Colors.Gray;

            //<Color x:Key="PhoneTextBoxColor">#BFFFFFFF</Color>
            (App.Current.Resources["PhoneTextBoxBrush"] as SolidColorBrush).Color = Color.FromArgb(0xBF, 0xFF, 0xFF, 0xFF);

            //Color c = (Color)App.Current.Resources["PhoneChromeColor"];
            (App.Current.Resources["PhoneAccentBrush"] as SolidColorBrush).Color = Colors.Orange; //red


            (App.Current.Resources["PhoneChromeBrush"] as SolidColorBrush).Color = Colors.Red;


            //(App.Current.Resources["PhoneTextBoxBrush"] as SolidColorBrush).Color = Color.FromArgb(0xBF, 0xFF, 0xFF, 0xFF);
            //PhoneTextBoxEditBackgroundBrush


            //100 percent White - #FFFFFFFF
            //(App.Current.Resources["PhoneBackgroundBrush"] as SolidColorBrush).Color = Colors.Black;
        }
    }
}