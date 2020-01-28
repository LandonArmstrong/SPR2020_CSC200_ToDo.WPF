﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ToDoApp.Wpf.Models;

namespace ToDoAppWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnAddTodoTaskButtonClick(object sender, RoutedEventArgs e)
        {
            TodoTask item = new TodoTask();
            item.Description = TodoTaskNameText.Text;
            TodoTaskListView.Items.Add(item);
        }

        private void TodoTaskNameText_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
