using System;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xodium.Mvvm.Xamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PopupView
	{
		public PopupView()
		{
			InitializeComponent();
		}

        public static readonly BindableProperty IsButtonPanelVisibleProperty = BindableProperty.CreateAttached(
            nameof(IsButtonPanelVisible), typeof(bool), typeof(PopupView), true);

        private static readonly BindableProperty OkButtonTextProperty = BindableProperty.CreateAttached(
            nameof(OkButtonText), typeof(string), typeof(PopupView), default(string));

        private static readonly BindableProperty OkCommandProperty = BindableProperty.CreateAttached(
            nameof(OkCommand), typeof(ICommand), typeof(PopupView), default(ICommand));

        private static readonly BindableProperty CancelButtonTextProperty = BindableProperty.CreateAttached(
            nameof(CancelButtonText), typeof(string), typeof(PopupView), default(string));

        private static readonly BindableProperty CancelCommandProperty = BindableProperty.CreateAttached(
            nameof(CancelCommand), typeof(ICommand), typeof(PopupView), default(ICommand));

        public View Content
        {
            get => ContentView.Content;
            set
            {
                if (Content == value) return;
                ContentView.Content = value;
                OnContentChanged();
            }
        }

        private void OnContentChanged()
        {
            IsButtonPanelVisible = GetIsButtonPanelVisible(Content);
            OkButtonText = GetOkButtonText(Content) ?? "OK";
            CancelButtonText = GetCancelButtonText(Content) ?? "Cancel";
            OkCommand = GetOkCommand(Content);
            CancelCommand = GetCancelCommand(Content);
        }

        public bool IsButtonPanelVisible
        {
            get => ButtonPanel.IsVisible;
            set => ButtonPanel.IsVisible = value;
        }

        public string OkButtonText
        {
            get => OkButton.Text;
            set => OkButton.Text = value;
        }

        public ICommand OkCommand
        {
            get => OkButton.Command;
            set => OkButton.Command = value;
        }

        public string CancelButtonText
        {
            get => CancelButton.Text;
            set => CancelButton.Text = value;
        }

        public ICommand CancelCommand
        {
            get => CancelButton.Command;
            set => CancelButton.Command = value;
        }

        public static bool GetIsButtonPanelVisible(BindableObject bindable) => (bool)bindable.GetValue(IsButtonPanelVisibleProperty);
        public static void SetIsButtonPanelVisible(BindableObject bindable, bool value) => bindable.SetValue(IsButtonPanelVisibleProperty, value);

        public static string GetOkButtonText(BindableObject bindable) => (string)bindable.GetValue(OkButtonTextProperty);
        public static void SetOkButtonText(BindableObject bindable, string value) => bindable.SetValue(OkButtonTextProperty, value);

        public static ICommand GetOkCommand(BindableObject bindable) => (ICommand)bindable.GetValue(OkCommandProperty);
        public static void SetOkCommand(BindableObject bindable, ICommand value) => bindable.SetValue(OkCommandProperty, value);

        public static string GetCancelButtonText(BindableObject bindable) => (string)bindable.GetValue(CancelButtonTextProperty);
        public static void SetCancelButtonText(BindableObject bindable, string value) => bindable.SetValue(CancelButtonTextProperty, value);

        public static ICommand GetCancelCommand(BindableObject bindable) => (ICommand)bindable.GetValue(CancelCommandProperty);
        public static void SetCancelCommand(BindableObject bindable, ICommand value) => bindable.SetValue(CancelCommandProperty, value);

        private async void OnCancelButtonClicked(object sender, EventArgs args)
        {
            if (CancelCommand != null) return;

            if (BindingContext is INavigationSource source)
            {
                await source.ExecutionEnvironment.NavigationService.GoBack();
            }
        }
    }
}
