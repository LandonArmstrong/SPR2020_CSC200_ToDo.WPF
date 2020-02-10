using System;
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
            DataContext = new ViewModels.MainWindowViewModel();
        }

        private void OnAddTodoTaskButtonClick(object sender, RoutedEventArgs e)
        {
            string line = TodoTaskNameText.Text;
            TodoTask item = new TodoTask();
            item.Description = line;
            TodoTaskListView.Items.Add(item);
        }

        private void TodoTaskNameText_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void OnRemoveTodoTaskButtonClick(object sender, RoutedEventArgs e)
        {
            int index = TodoTaskListView.SelectedIndex;
            if (index >= 0 && index < TodoTaskListView.Items.Count)
                TodoTaskListView.Items.RemoveAt(index);
        }

        private void OnTodoTaskListViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = TodoTaskListView.SelectedIndex;
            bool enabled = (index >= 0 && index < TodoTaskListView.Items.Count);
            RemoveTodoTaskButton.IsEnabled = enabled;
        }

        private void TodoTaskNameText_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string path = "task.txt";
            
                    System.IO.FileStream fileStream = System.IO.File.Open(
                    path,
                    System.IO.FileMode.Append,
                    System.IO.FileAccess.Write,
                    System.IO.FileShare.None);
                    System.IO.StreamWriter writer = new System.IO.StreamWriter(fileStream);

                    
                    foreach (var item in TodoTaskListView.Items)
                    {
                        TodoTask boxitem = item as TodoTask;
                        writer.WriteLine(boxitem.Description);
                    }
                    writer.Close();
                    fileStream.Close();
                
                
                    
                
            
        }
        private void Load_Click(object sender, RoutedEventArgs e)
        {
            
            string path = "task.txt";

            if (System.IO.File.Exists(path))
            {
                System.IO.FileStream fileStream = System.IO.File.Open(
                path,
                System.IO.FileMode.Open,               
                System.IO.FileAccess.Read,
                System.IO.FileShare.None);

                System.IO.StreamReader reader = new System.IO.StreamReader(fileStream);
               
                
                
                while(reader.EndOfStream == false)
                {
                    string line = reader.ReadLine();
                    TodoTask item = new TodoTask();
                    item.Description = line;
                    this.TodoTaskListView.Items.Add(item);
                                      
                    
                }




                reader.Close();
                fileStream.Close();
            }
            else
            {
                Console.WriteLine("File not found!");


            }
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //private bool CanRemoveTodoTask(int selectedIndex)
        //{
        //return(selectedIndex >= 0 && selectedIndex)
        //}
    }
}
