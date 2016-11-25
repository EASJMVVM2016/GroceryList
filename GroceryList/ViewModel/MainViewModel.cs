using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroceryList.ViewModel.Commands;
using GroceryList.Model;
using Windows.Storage;
using Newtonsoft.Json;
using Windows.UI.Popups;

namespace GroceryList.ViewModel
{
    class MainViewModel : INotifyPropertyChanged
    {
        //Til INotifyPropetyChanged interface
        public event PropertyChangedEventHandler PropertyChanged;

        //Prop til RelayCommand
        public RelayCommand AddItemCommand { get; private set; }
        public RelayCommand RemoveItemCommand { get; private set; }
        public RelayCommand SaveListCommand { get; private set; }
        public RelayCommand LoadListCommand { get; private set; }
        public RelayCommand ClearListCommand { get; private set; }

        //Konstant filnavn til at gemme data
        const String fileName = "saveFile.json";

        //Observable Collector prop
        private obsGrocList _grocList;
        public obsGrocList GrocList
        {
            get { return _grocList; }
            set { _grocList = value;
                NotifyView(nameof(GrocList));
                }
        }

        //Item selected prop
        private GrocItem _selectedGrocItem;
        public GrocItem SelectedGrocItem
        {
            get { return _selectedGrocItem; }
            set { _selectedGrocItem = value;
                NotifyView(nameof(SelectedGrocItem));
            }
        }

        //New Item prop - Bruges til at hente data fra TextBoxes
        private GrocItem _newGrocItem;
        public GrocItem NewGrocItem
        {
            get { return _newGrocItem; }
            set { _newGrocItem = value; }
        }

        //Prop til beskeder til brugeren, evt fejlbeskeder
        private String _outputToUser;
        public String OutputToUser
        {
            get { return _outputToUser; }
            set { _outputToUser = value;
                NotifyView(nameof(OutputToUser));
            }
        }


        //END OF PROPERTIES

        public MainViewModel()
        {
            
            GrocList = new obsGrocList();
            SelectedGrocItem = new GrocItem();
            NewGrocItem = new GrocItem();
            //Instanser RelayCommands
            AddItemCommand = new RelayCommand(AddNew, null);
            RemoveItemCommand = new RelayCommand(Remove, null);
            SaveListCommand = new RelayCommand(SaveList_Async, null);
            LoadListCommand = new RelayCommand(GetList_Async, null);
            ClearListCommand = new RelayCommand(Clear, null);
        }

        //Add ny item til liste
        public void AddNew()
        {
            //Test på værdier
            if(!String.IsNullOrWhiteSpace(NewGrocItem.grocItem) && !String.IsNullOrWhiteSpace(NewGrocItem.itemQnty)) {
                if(NewGrocItem.itemQnty.All(Char.IsDigit)) {
                    //Instancer et container objekt
                    OutputToUser = String.Empty;
                    GrocItem tempGrocItem = new GrocItem();
                    tempGrocItem.grocItem = NewGrocItem.grocItem;
                    tempGrocItem.itemQnty = NewGrocItem.itemQnty;

                    //Add til list
                    GrocList.Add(tempGrocItem);
                } else
                {
                    OutputToUser = "Quanitity must be number!";
                }
            }
            else
            {
                OutputToUser = "Fields must not be blank!";
            }

        }

        //Remove item fra liste
        public void Remove()
        {
            if(GrocList.Count == 0) {
                OutputToUser = "Grocery list is empty!";
            } else {
                if ( SelectedGrocItem != null )
                {
                    OutputToUser = String.Empty;
                    GrocList.Remove(SelectedGrocItem);
                }
                else
                {
                    OutputToUser = "Select item to remove!";
                }
           }
        }

        //Til at clear list
        public void Clear()
        {
            if(GrocList.Count > 0)
            {
                GrocList.Clear();
                OutputToUser = String.Empty;
            }
            else
            {
                OutputToUser = "You can't clear an empty list";
            }
        }

        //Metode til at gemme i fil - Json format!
        private async void SaveList_Async()
        {
            if(GrocList.Count > 0) {

            OutputToUser = String.Empty;
            StorageFile localFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

            await FileIO.WriteTextAsync(localFile, GrocList.GetJson());
            }
            else
            {
                OutputToUser = "You can't save an empty list!";
            }
        }

        //Metode til at hente gemt data
        private async void GetList_Async()
        {

            try {
            StorageFile localFile = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
            String jsonData = await FileIO.ReadTextAsync(localFile);
            GrocList = JsonConvert.DeserializeObject<obsGrocList>(jsonData);

            } catch (Exception)
            {
                MessageDialog noDataDialog = new MessageDialog("No save file found!");
                noDataDialog.Commands.Add(new UICommand { Label = "OK" });
                await noDataDialog.ShowAsync();
            }
        }

        //Metode til INotifyPropetyChanged interface
        protected void NotifyView(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
