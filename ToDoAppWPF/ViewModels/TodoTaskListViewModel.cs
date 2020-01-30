using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Wpf.Models;
using System.Windows.Input;


namespace ToDoAppWPF.ViewModel
{
    class TodoTaskViewModel
    {
        public TodoTaskViewModel(TodoTask todoTask)
        {
            this.TodoTask = todoTask;
            
        }


        public ICommand AddCommand { get; }
        public TodoTask TodoTask { get; }

    }
}
