using AlexNitter.Todo.Lib.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlexNitter.Todo.Lib.ViewModels
{
    public class TodoViewModel : BaseViewModel
    {
        public List<TodoListe> Listen { get; set; }
        public TodoListe SelectedListe { get; set; }
    }
}
