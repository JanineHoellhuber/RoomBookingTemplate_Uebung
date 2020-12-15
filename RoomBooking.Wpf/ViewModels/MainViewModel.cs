using RoomBooking.Core.Contracts;
using RoomBooking.Core.Entities;
using RoomBooking.Persistence;
using RoomBooking.Wpf.Common;
using RoomBooking.Wpf.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RoomBooking.Wpf.ViewModels
{
  public class MainViewModel : BaseViewModel
  {
        public ObservableCollection<Booking> _bookings;
        public ObservableCollection<Room> _room;
        public Room _selectedRoom;
        public Booking _selectedBooking;
        public Booking _selectedBookingNow;

        public ObservableCollection<Room> Rooms
        {
            get => _room;
            set
            {
                _room = value;
                OnPropertyChanged(nameof(Rooms));
            }

        }

        public ObservableCollection<Booking> Bookings
        {
            get => _bookings;
            set
            {
                _bookings = value;
                OnPropertyChanged(nameof(Bookings));
            }
        }

        public Room SelectedRoom
        {
            get => _selectedRoom;
            set
            {
                _selectedRoom = value;
                OnPropertyChanged(nameof(SelectedRoom));
                LoadBookingsAsync();
            }
        }

        public Booking SelectedBooking
        {
            get => _selectedBooking;
            set
            {
                _selectedBooking = value;
                OnPropertyChanged(nameof(SelectedBooking));

            }
        }
        public MainViewModel(IWindowController windowController) : base(windowController)
        {
        }

         private async Task LoadDataAsync()
         {
            using IUnitOfWork uow = new UnitOfWork();
            var rooms = await uow.Rooms.GetAllAsync();
            Rooms = new ObservableCollection<Room>(rooms);
            _selectedRoom = Rooms.First();
            await LoadBookingsAsync();
         }

        private async Task LoadBookingsAsync()
        {
            _selectedBookingNow = SelectedBooking;
            using IUnitOfWork uow = new UnitOfWork();
            var bookings = await uow.Bookings.GetByRoomsAsync(SelectedRoom.Id);
            Bookings = new ObservableCollection<Booking>(bookings);

            if(_selectedBookingNow == null)
            {
                SelectedBooking = Bookings.First();
            }
            else
            {
                SelectedBooking = _selectedBooking;
            }
        }

    public static async Task<MainViewModel> CreateAsync(IWindowController windowController)
    {
      var viewModel = new MainViewModel(windowController);
      await viewModel.LoadDataAsync();
      return viewModel;
    }

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
      throw new NotImplementedException();
    }

        private ICommand _cmdEditCustomer;
        public ICommand CmdEditCustomer
        {
            get
            {
                if(_cmdEditCustomer == null)
                {
                    _cmdEditCustomer = new RelayCommand(
                        execute: _ =>
                        {
                            Controller.ShowWindow(new EditCustomerViewModel(Controller, SelectedBooking.Customer), true);
                            LoadDataAsync();
                        },
                    canExecute: _ => SelectedBooking != null);
                }
                return _cmdEditCustomer;
            }
        }
  }
}
