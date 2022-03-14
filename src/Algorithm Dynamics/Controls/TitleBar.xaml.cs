using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Algorithm_Dynamics.Controls
{
    public sealed partial class TitleBar : UserControl
    {
        public event EventHandler BackButtonClick;

        public TitleBar()
        {
            InitializeComponent();
        }

        public bool IsShowBackButton
        {
            get { return (bool)GetValue(IsShowBackButtonProperty); }
            set { SetValue(IsShowBackButtonProperty, value); }
        }

        public static readonly DependencyProperty IsShowBackButtonProperty =
            DependencyProperty.Register(nameof(IsShowBackButton), typeof(bool), typeof(TitleBar), new PropertyMetadata(false));

        private void OnBackButtonClickAsync(object sender, RoutedEventArgs e)
        {
            BackButtonClick?.Invoke(this, EventArgs.Empty);
        }
    }
}
