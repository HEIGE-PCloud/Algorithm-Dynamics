﻿using Microsoft.UI.Xaml;
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
    public sealed partial class AssignmentDetailsPage : Page
    {
        public AssignmentDetailsPage()
        {
            InitializeComponent();
        }
        public enum Mode
        {
            Student,
            Teacher
        }
        private Mode PageMode = Mode.Student;
        private bool _isStudentMode { get => PageMode == Mode.Student; }
        private bool _isTeacherMode { get => PageMode == Mode.Teacher; }
        private void ProblemListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            App.NavigateTo(typeof(CodingPage));
        }

        private void EditAssignment(object sender, RoutedEventArgs e)
        {
            App.NavigateTo(typeof(CreateNewProblemListPage), Tuple.Create(CreateNewProblemListPage.Mode.EditAssignment));
        }
    }
}