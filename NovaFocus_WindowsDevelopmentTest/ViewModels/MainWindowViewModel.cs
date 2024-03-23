using ModernWpf.Controls;
using NovaFocus_WindowsDevelopmentTest.Helpers;
using NovaFocus_WindowsDevelopmentTest.Models;
using NovaFocus_WindowsDevelopmentTest.Views.Dialogs;
using System.Collections.ObjectModel;

namespace NovaFocus_WindowsDevelopmentTest.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        //ObservableCollection automatically updates UI when an item is inserted, internally implements INotifyPropertyChanged

        public ObservableCollection<ToDoItemModel> ToDoItems { get; set; }

        public RelayCommand OpenToDoItemContentDialogCommand { get; set; }

        public RelayCommand DeleteItemCommand { get; set; }

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


        public MainWindowViewModel()
        {
            ToDoItems = new();
            OpenToDoItemContentDialogCommand = new RelayCommand(AddToDoItemContentDialog);
            DeleteItemCommand = new RelayCommand(DeleteToDoItem);

            ToDoItems.Add(new ToDoItemModel() { Title = "TEST", Description = "dshfshfskdhgfhsiuthgriewgtu TESTSTSTSS " });
        }


        private async void AddToDoItemContentDialog(object obj)
        {
            ToDoItemDialog dialog = new();
            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary && !string.IsNullOrEmpty(dialog.title.Text) && !string.IsNullOrEmpty(dialog.description.Text))
            {
                ToDoItems.Add(new ToDoItemModel() { Title = dialog.title.Text, Description = dialog.description.Text });

            }

            // fix when todoitem is empty

        }

        private void DeleteToDoItem(object obj)
        {
            var item = obj as ToDoItemModel;

            if (item != null)
            {
                ToDoItems.Remove(item);
            }
        }

        private void UpdateButtonEnabled()
        {
            AddToDoContentDialogButtonEnabled = !string.IsNullOrEmpty(ToDoTitleText) && !string.IsNullOrEmpty(ToDoDescriptionText);
        }

    }
}
