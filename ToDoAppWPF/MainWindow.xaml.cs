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
using System.Runtime.Serialization.Formatters;

namespace ToDoAppWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private const string FilterTxt = "Text file (*.txt)|*.txt";
        private const string FilterJson = "JSON file (*.json)|*.json";
        private const string FilterXml = "XML file (*.xml)|*.xml";
        private const string FilterSoap = "SOAP file (*.soap)|*.soap";
        private const string FilterBinary = "Binary file (*.bin)|*.bin";
        private const string FilterAny = "Any files (*.*)|*.*";




        public MainWindow()
        {
            InitializeComponent();
            //DataContext = new ViewModels.MainWindowViewModel();


        }

        private const string DefaultFilePath = "task.txt";

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
            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();

            string path = "task.txt";
            dialog.Filter = FilterTxt + "|" +
                            FilterJson + "|" +
                            FilterXml + "|" +
                            FilterSoap + "|" +
                            FilterBinary;

            dialog.FilterIndex = 1;
            bool? dialogResult = dialog.ShowDialog();
            if (dialogResult == true)
            {
                string filePath = dialog.FileName;
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);

                }
                // get content from UI control
                string content = string.Empty;

                // determine which format the user want to save as
                if (dialog.FilterIndex == 1)
                {
                    // write as txt
                    System.IO.StringWriter writer = new System.IO.StringWriter();

                    ItemCollection items = this.TodoTaskListView.Items;
                    foreach (object item in items)
                    {
                        TodoTask todoTask = item as TodoTask;
                        writer.WriteLine(todoTask.LastSaved);
                        writer.WriteLine(todoTask.IsComplete);
                        writer.WriteLine(todoTask.Description);
                       
                    }

                    content = writer.ToString();
                    writer.Dispose();


                    foreach (var item in TodoTaskListView.Items)
                    {
                        TodoTask boxitem = item as TodoTask;
                        writer.WriteLine(boxitem.Description);
                    }
                    // write as txt
                    //System.IO.StringWriter writer = new System.IO.StringWriter();
                    //writer.WriteLine(DateTime.Now);   // write last saved
                    //writer.Write(content);            // write content 
                    //content = writer.ToString();      // assign assembled content

                    writer.Dispose();                 // release writer
                }
                else if (dialog.FilterIndex == 2)
                {
                    // write as JSON
                    // create object to serialize
                    List<TodoTask> list = new List<TodoTask>();
                    ItemCollection items = this.TodoTaskListView.Items;
                    foreach (var item in items)
                    {
                        TodoTask todoTask = item as TodoTask;
                        list.Add(todoTask);
                    }
                    // serialize type (Models.Document) to JSON string 
                    string json = Newtonsoft
                        .Json
                        .JsonConvert
                        .SerializeObject(list);
                    // set content to JSON result
                    content = json;                   // assign JSON string to content
                }
                else if (dialog.FilterIndex == 3)
                {
                    List<TodoTask> list = new List<ToDoApp.Wpf.Models.TodoTask>();
                    ItemCollection items = this.TodoTaskListView.Items;
                    // write as XML
                    // create object to serialize
                    foreach(var item in items)
                    {
                        TodoTask todoTask = item as TodoTask;
                        list.Add(todoTask);
                    }
                    // create serializer
                    System.Xml.Serialization.XmlSerializer serializer =
                        new System.Xml.Serialization.XmlSerializer(typeof(List<ToDoApp.Wpf.Models.TodoTask>));
                    // this serializer writes to a stream
                    System.IO.MemoryStream stream = new System.IO.MemoryStream();
                    serializer.Serialize(stream, list);
                    stream.Seek(0, System.IO.SeekOrigin.Begin);   // reset stream to start

                    // read content from stream
                    System.IO.StreamReader reader = new System.IO.StreamReader(stream);
                    content = reader.ReadToEnd();   // assign XML string to content

                    reader.Dispose();   // dispose the reader
                    stream.Dispose();   // dispose the stream
                }
                else if (dialog.FilterIndex == 4)
                {
                    // write as SOAP
                    // note: have to add a reference to a global assembly (dll) to use in project 
                    // create object to serialize
                    List<TodoTask> list = new List<TodoTask>();
                    ItemCollection items = this.TodoTaskListView.Items;

                    foreach(object item in items)
                    {
                        TodoTask todoTask = item as TodoTask;
                        list.Add(todoTask);
                    }
                    // create serializer
                    System.Runtime.Serialization.Formatters.Soap.SoapFormatter serializer =
                        new System.Runtime.Serialization.Formatters.Soap.SoapFormatter();
                    //this serializer writes to a stream
                    System.IO.MemoryStream stream = new System.IO.MemoryStream();
                    
                    serializer.Serialize(stream, list);
                    stream.Seek(0, System.IO.SeekOrigin.Begin);   // reset stream to start

                    // read content from stream
                    System.IO.StreamReader reader = new System.IO.StreamReader(stream);
                    content = reader.ReadToEnd();   // assign SOAP string to content

                    reader.Dispose();   // dispose the reader
                    stream.Dispose();   // dispose the stream

                }
                else // implies this is last one (binary)
                {
                    // write as binary
                    // create object to serialize
                    ToDoApp.Wpf.Models.TodoTask document = new ToDoApp.Wpf.Models.TodoTask(content);
                    // create serializer
                    System.Runtime.Serialization.Formatters.Binary.BinaryFormatter serializer =
                        new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    // this serializer writes to a stream
                    System.IO.MemoryStream stream = new System.IO.MemoryStream();
                    serializer.Serialize(stream, document);

                    stream.Seek(0, System.IO.SeekOrigin.Begin);   // reset stream to start
                    // reading and writing binary data directly as string has issues, try not to do it 
                    content = Convert.ToBase64String(stream.ToArray());    // assign base64 to content

                    stream.Dispose();   // dispose the stream
                }

                // write content to file

                System.IO.File.WriteAllText(filePath, content);

                


            }

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



                while (reader.EndOfStream == false)
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

        private void OnMainFileSaveMenuClicked(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            bool? dialogResult = dialog.ShowDialog();
            if (dialogResult == true)
            {
                string filePath = dialog.FileName;

                dialog.ShowDialog();
                string content = System.IO.File.ReadAllText(filePath);

                if (System.IO.File.Exists(DefaultFilePath))
                {
                    MessageBoxResult userSelected;
                    userSelected = System.Windows.MessageBox.Show(
                        messageBoxText: "Overwrite file?",
                        caption: "Confirm", MessageBoxButton.YesNo);
                    if (userSelected == MessageBoxResult.Yes)
                    {
                        System.IO.File.Delete(DefaultFilePath);
                    }
                }
            }
        }
        private void OnMainHelpAboutMenuClicked(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show(messageBoxText: "Landon Armstrong", caption: "About",
            MessageBoxButton.OK,
            icon: MessageBoxImage.Information);
        }

        private void OnMainFileOpenMenuClicked(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();

            dialog.Filter = FilterTxt + "|" +
                            FilterJson + "|" +
                            FilterXml + "|" +
                            FilterSoap + "|" +
                            FilterBinary + "|" +
                            FilterAny;

            dialog.FilterIndex = 1;

            bool? dialogResult = dialog.ShowDialog();
            if (dialogResult == true)
            {
                string filePath = dialog.FileName;
                if (System.IO.File.Exists(filePath))
                {
                    string content = System.IO.File.ReadAllText(filePath);

                    // determine which format the user want to save as
                    if (dialog.FilterIndex == 1)
                    {
                        // read as txt
                        System.IO.StringReader reader = new System.IO.StringReader(content);
                        string line = reader.ReadLine();
                        System.DateTime lastSaved;
                        if (System.DateTime.TryParse(line, out lastSaved))
                        {
                            // we read the line, so remove the line
                            line = string.Empty;
                        }
                        else
                        {
                            lastSaved = System.DateTime.Now;
                        }

                        content = line + reader.ReadToEnd();
                        ToDoApp.Wpf.Models.TodoTask document = new ToDoApp.Wpf.Models.TodoTask(content);
                        document.LastSaved = lastSaved;
                        content = document.Content;

                        reader.Dispose();
                    }
                    else if (dialog.FilterIndex == 2)
                    {
                        // read as JSON
                        string json = content;
                        // deserialize to expected type (Models.Document)
                        object jsonObject = Newtonsoft.Json
                            .JsonConvert
                            .DeserializeObject(json, typeof(ToDoApp.Wpf.Models.TodoTask));
                        // cast and assign JSON object to expected type (Models.Document)
                        ToDoApp.Wpf.Models.TodoTask document = (ToDoApp.Wpf.Models.TodoTask)jsonObject;
                        // assign content from deserialized Models.Document
                        content = document.Content;
                    }
                    else if (dialog.FilterIndex == 3)
                    {
                        // read as XML for type of Models.Document
                        System.Xml.Serialization.XmlSerializer serializer =
                            new System.Xml.Serialization.XmlSerializer(typeof(ToDoApp.Wpf.Models.TodoTask));

                        // convert content to byte array (sequence of bytes)
                        byte[] buffer = System.Text.Encoding.ASCII.GetBytes(content);
                        // make stream from buffer
                        System.IO.MemoryStream stream = new System.IO.MemoryStream(buffer);
                        // deserialize stream to an object
                        object xmlObject = serializer.Deserialize(stream);
                        // cast and assign XML object to actual type object
                        ToDoApp.Wpf.Models.TodoTask document = (ToDoApp.Wpf.Models.TodoTask)xmlObject;

                        content = document.Content;
                        stream.Dispose();   // release the resources
                    }
                    else if (dialog.FilterIndex == 4)
                    {
                        // read as soap
                        // System.Runtime.Serialization.Formatters.Soap.SoapFormatter serializer =
                        //new System.Runtime.Serialization.Formatters.Soap.SoapFormatter();

                        // convert content to byte array (sequence of bytes)
                        byte[] buffer = System.Text.Encoding.ASCII.GetBytes(content);
                        // make stream from buffer
                        System.IO.MemoryStream stream = new System.IO.MemoryStream(buffer);
                        // deserialize stream to an object
                        //object soapObject = serializer.Deserialize(stream);
                        // cast and assign SOAP object to actual type object
                        //ToDoApp.Wpf.Models.TodoTask document = (ToDoApp.Wpf.Models.TodoTask)soapObject;
                        // read content
                        //content = document.Content;

                        stream.Dispose();   // release the resources
                    }

                    else if (dialog.FilterIndex == 5)
                    {
                        // read as binary
                        System.Runtime.Serialization.Formatters.Binary.BinaryFormatter serializer =
                            new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                        // reading and writing binary data directly as string has issues, try not to do it 
                        byte[] buffer = Convert.FromBase64String(content);
                        System.IO.MemoryStream stream = new System.IO.MemoryStream(buffer);
                        // deserialize stream to object
                        object binaryObject = serializer.Deserialize(stream);
                        // assign binary object to actual type object
                        ToDoApp.Wpf.Models.TodoTask document = (ToDoApp.Wpf.Models.TodoTask)binaryObject;
                        // read the content
                        content = document.Content;
                        stream.Dispose();   // release the resources
                    }
                    else // imply this is any file 
                    {
                        // read as is
                    }

                    // assign content to UI control
                    //UserText.Text = content;
                }
            }



            //private bool CanRemoveTodoTask(int selectedIndex)
            //{
            //return(selectedIndex >= 0 && selectedIndex)
            //}
        }
    }
}
