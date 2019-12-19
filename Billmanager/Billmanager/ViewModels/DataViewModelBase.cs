﻿using System.Threading.Tasks;
using Billmanager.Database;
using Billmanager.Helper;
using Billmanager.Interfaces.Database;
using Prism.Navigation;
using Xamarin.Forms;

namespace Billmanager.ViewModels
{
    /// <summary> The data view Model base. </summary>
    public class DataViewModelBase<T> : ViewModelBase where T : class, IDatabaseTable, new()
    {
        private Command saveCommand;
        public IDataStore<T> DataStore => SqliteDatabase.GetTable<T>();

        public DataViewModelBase(INavigationService navigationService) : base(navigationService)
        {
            this.Model = new T();
        }

        public Command SaveCommand => this.saveCommand ?? (this.saveCommand = new Command(async () => await this.Save()));

        public T Model { get; set; }

        /// <summary>If Id of item is empty => new add otherwise update</summary>
        public async Task Save()
        {
            if (this.Model.Id.IsNullOrEmpty())
            {
                await this.DataStore.AddItemAsync(this.Model).ConfigureAwait(false);
            }
            else
            {
                await this.DataStore.UpdateItemAsync(this.Model).ConfigureAwait(false);
            }

            await NavigationService.GoBackAsync();
        }
    }
}