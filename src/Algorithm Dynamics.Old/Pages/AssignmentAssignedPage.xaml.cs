using Algorithm_Dynamics.Core.Models;
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

namespace Algorithm_Dynamics.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AssignmentAssignedPage : Page
    {
        public AssignmentAssignedPage()
        {
            this.InitializeComponent();
            AssignmentList.Add(new("Assignment 1", "", DateTime.Today));
            AssignmentList.Add(new("Assignment 2", "", DateTime.Today));
            AssignmentList.Add(new("Assignment 3", "", DateTime.Today));
            AssignmentList.Add(new("Assignment 4", "", DateTime.Today));
            AssignmentList.Add(new("Assignment 5", "", DateTime.Today));
            AssignmentList.Add(new("Assignment 6", "", DateTime.Today));
            AssignmentList.Add(new("Assignment 7", "", DateTime.Today));
            AssignmentList.Add(new("Assignment 8", "", DateTime.Today));
            AssignmentList.Add(new("Assignment 9", "", DateTime.Today));
            AssignmentList.Add(new("Assignment 10", "", DateTime.Today));
            AssignmentList.Add(new("Assignment 11", "", DateTime.Today));
            AssignmentList.Add(new("Assignment 12", "", DateTime.Today));
            AssignmentList.Add(new("Assignment 13", "", DateTime.Today));
            AssignmentList.Add(new("Assignment 14", "", DateTime.Today));
            AssignmentList.Add(new("Assignment 15", "", DateTime.Today));
        }
        private List<Assignment> AssignmentList = new();
    }
}
