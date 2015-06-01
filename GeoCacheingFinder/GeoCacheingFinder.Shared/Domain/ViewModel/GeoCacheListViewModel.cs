using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace GeoCacheingFinder.Domain.ViewModel
{
    public class GeoCacheListViewModel : INotifyPropertyChanged
    {
        //Constructor
        public GeoCacheListViewModel() { }

        public GeoCacheListViewModel(SearchOptionViewModel searchOptionViewModel) 
        {
            this.SearchOptionViewModel = searchOptionViewModel;
            this.GeoCacheModel = new List<GeoCacheModel>();
        }


        //Properties
        /// <summary>
        /// 
        /// </summary>
        private SearchOptionViewModel _searchOptionViewModel;
        public SearchOptionViewModel SearchOptionViewModel
        {
            get { return _searchOptionViewModel; }
            set { this.SetProperty(ref this._searchOptionViewModel, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        private List<GeoCacheModel> _geoCacheModel;
        public List<GeoCacheModel> GeoCacheModel
        {
            get { return _geoCacheModel; }
            set { this.SetProperty(ref this._geoCacheModel, value); }
        }


        // property changed logic by jump start
        public event PropertyChangedEventHandler PropertyChanged;
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] String propertyName = null)
        {
            if (object.Equals(storage, value)) return false;
            storage = value;
            this.OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var eventHandler = this.PropertyChanged;
            if (eventHandler != null)
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
