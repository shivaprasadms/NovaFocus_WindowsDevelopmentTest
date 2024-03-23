using ModernWpf.Controls;
using NovaFocus_WindowsDevelopmentTest.Helpers;
using NovaFocus_WindowsDevelopmentTest.Models;
using NovaFocus_WindowsDevelopmentTest.Views.Dialogs;
using System.Collections.ObjectModel;

namespace NovaFocus_WindowsDevelopmentTest.ViewModels
{
    public class MainWindowViewModel
    {
        //ObservableCollection automatically updates UI when an item is inserted, internally implements INotifyPropertyChanged

        public ObservableCollection<ToDoItemModel> ToDoItems { get; set; }

        public RelayCommand OpenToDoItemContentDialogCommand { get; set; }

        public RelayCommand DeleteItemCommand { get; set; }

        public MainWindowViewModel()
        {
            ToDoItems = new();
            OpenToDoItemContentDialogCommand = new RelayCommand(ToDoItemContentDialog);
            DeleteItemCommand = new RelayCommand(DeleteToDoItem);
        }


        private async void ToDoItemContentDialog(object obj)
        {
            ToDoItemDialog dialog = new();
            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
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
    }
}
