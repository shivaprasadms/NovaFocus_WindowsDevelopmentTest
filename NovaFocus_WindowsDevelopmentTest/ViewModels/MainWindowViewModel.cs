using ModernWpf.Controls;
using NovaFocus_WindowsDevelopmentTest.Helpers;
using NovaFocus_WindowsDevelopmentTest.Models;
using NovaFocus_WindowsDevelopmentTest.Views.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace NovaFocus_WindowsDevelopmentTest.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        //ObservableCollection automatically updates UI when an item is inserted, internally implements INotifyPropertyChanged

        public ObservableCollection<ToDoItemModel> ToDoItems { get; set; }

        public RelayCommand OpenToDoItemContentDialogCommand { get; set; }

        public RelayCommand OpenAboutDialogCommand { get; set; }

        public RelayCommand DeleteItemCommand { get; set; }

        public string JSONFilePath { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "todo.json");


        #region PRIVATE_FIELDS
        private string _toDoTitleText;
        public string ToDoTitleText
        {
            get { return _toDoTitleText; }
            set
            {
                SetProperty(ref _toDoTitleText, value);
                UpdateButtonEnabled();
            }
        }

        private string _toDoDescriptionText;
        public string ToDoDescriptionText
        {
            get { return _toDoDescriptionText; }
            set
            {
                SetProperty(ref _toDoDescriptionText, value);
                UpdateButtonEnabled();
            }
        }

        private bool _isAddToDoContentDialogButtonEnabled;
        public bool AddToDoContentDialogButtonEnabled
        {
            get { return _isAddToDoContentDialogButtonEnabled; }
            set { SetProperty(ref _isAddToDoContentDialogButtonEnabled, value); }
        }
        #endregion

        public MainWindowViewModel()
        {
            ToDoItems = new();
            OpenToDoItemContentDialogCommand = new RelayCommand(AddToDoItemContentDialog);
            OpenAboutDialogCommand = new RelayCommand(OpenAboutContentDialog);
            DeleteItemCommand = new RelayCommand(DeleteToDoItem);

            RetrieveToDoItemIfExists();
        }

        private async void AddToDoItemContentDialog(object obj)
        {
            ToDoItemDialog dialog = new();
            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary && !string.IsNullOrEmpty(dialog.title.Text) && !string.IsNullOrEmpty(dialog.description.Text))
            {
                var item = new ToDoItemModel() { Title = dialog.title.Text, Description = dialog.description.Text };
                ToDoItems.Add(item);
                dialog.title.Text = dialog.description.Text = "";

                using (StreamWriter sw = new StreamWriter(JSONFilePath))
                {
                    foreach (ToDoItemModel model in ToDoItems)
                    {
                        string json = JsonSerializer.Serialize(model);
                        sw.WriteLine(json);
                    }
                }
            }

        }

        private void RetrieveToDoItemIfExists()
        {
            string[] jsonObjects;

            if (File.Exists(JSONFilePath))
            {
                jsonObjects = File.ReadAllLines(JSONFilePath);

                foreach (string jsonObject in jsonObjects)
                {
                    var model = JsonSerializer.Deserialize<ToDoItemModel>(jsonObject);
                    ToDoItems.Add(model);
                }
            }
            else
            {
                File.Create(JSONFilePath).Close();
            }
        }

        private void DeleteToDoItem(object obj)
        {
            var item = obj as ToDoItemModel;

            if (item != null)
            {
                ToDoItems.Remove(item);
            }

            string[] models;
            List<ToDoItemModel> items = new();

            if (File.Exists(JSONFilePath))
            {
                models = File.ReadAllLines(JSONFilePath);

                foreach (string jsonObject in models)
                {
                    items.Add(JsonSerializer.Deserialize<ToDoItemModel>(jsonObject));

                }

                items.RemoveAll(model => model.Title == item.Title);

                string updatedJson = JsonSerializer.Serialize(items);

                using (StreamWriter sw = new StreamWriter(JSONFilePath))
                {
                    foreach (ToDoItemModel model in items)
                    {
                        string json = JsonSerializer.Serialize(model);
                        sw.WriteLine(json);
                    }
                }
            }
            else
            {
                MessageBox.Show("To-Do items save file not found!");
                File.Create(JSONFilePath);
            }


        }

        private void UpdateButtonEnabled()
        {
            AddToDoContentDialogButtonEnabled = !string.IsNullOrEmpty(ToDoTitleText) && !string.IsNullOrEmpty(ToDoDescriptionText);
        }

        private async void OpenAboutContentDialog(object obj)
        {
            AboutDialog dialog = new();
            await dialog.ShowAsync();

        }

    }
}
