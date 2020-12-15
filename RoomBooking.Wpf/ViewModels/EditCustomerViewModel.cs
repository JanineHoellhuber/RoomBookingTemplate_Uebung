using RoomBooking.Core.Contracts;
using RoomBooking.Core.Entities;
using RoomBooking.Core.Validations;
using RoomBooking.Persistence;
using RoomBooking.Wpf.Common;
using RoomBooking.Wpf.Common.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Windows.Input;

namespace RoomBooking.Wpf.ViewModels
{
    class EditCustomerViewModel : BaseViewModel
    {
        public Customer _customer;
        public string _firstname;
        public string _lastname;
        public string _iban;

        public Customer Customers
        {
            get => _customer;
            set
            {
                _customer = value;
                OnPropertyChanged(nameof(Customers));
            }
        }

        public string Firstname
        {
            get => _firstname;
            set
            {
                _firstname = value;
                OnPropertyChanged(nameof(Firstname));
            }
        }
        [Required(ErrorMessage ="Lastname is required")]
        [MinLength(2, ErrorMessage ="Minimum length of Lastname is 2")]
        public string Lastname
        {
            get => _lastname;
            set
            {
                _lastname = value;
                OnPropertyChanged(nameof(Lastname));
                Validate();
            }
        }

        public string Iban
        {
            get => _iban;
            set
            {
                _iban = value;
                OnPropertyChanged(nameof(Iban));
                Validate();
            }
        }
        public EditCustomerViewModel(IWindowController controller, Customer customer) : base(controller)
        {
            Customers = customer;
            Init();
        }

        private void Init()
        {
            Firstname = Customers.FirstName;
            Lastname = Customers.LastName;
            Iban = Customers.Iban;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Lastname))
            {
                yield return new ValidationResult(
                    "Lastname is required",
                    new string[] { nameof(Lastname) });

            }
            else if (Lastname.Length < 2)
            {
                yield return new ValidationResult(
                  "Minimum length of Lastname is 2",
                  new string[] { nameof(Lastname) });

            }
            else if (!IbanChecker.CheckIban(Iban))
            {
                yield return new ValidationResult(
                 "Iban must be valid",
                 new string[] { nameof(Lastname) });

            }
        }

        private ICommand _cmdSave;

        public ICommand CmdSave
        {
            get
            {
                if (_cmdSave == null)
                {
                    _cmdSave = new RelayCommand(
                        execute: async _ =>
                        {
                            try
                            {
                                using IUnitOfWork uow = new UnitOfWork();
                                Customer customerInDB = await uow.Customers.GetById(Customers.Id);
                                customerInDB.FirstName = Firstname;
                                customerInDB.LastName = Lastname;
                                customerInDB.Iban = Iban;
                                uow.Customers.Update(_customer);
                                await uow.SaveAsync();
                                Controller.CloseWindow(this);
                            }
                            catch (ValidationException ve)
                            {
                                if (ve.Value is IEnumerable<string> properties)
                                {
                                    foreach (var property in properties)
                                    {
                                        Errors.Add(property, new List<string>
                                        {
                                            ve.ValidationResult.ErrorMessage
                                        });
                                    }
                                }
                                else
                                {
                                    DbError = ve.ValidationResult.ToString();
                                }
                            }
                        },
                        canExecute: _ => IsValid);

                }
                return _cmdSave;
            }
            set { _cmdSave = value; }
        }


            private ICommand _cmdUndo;

        public ICommand CmdUndo
        {
            get
            {
                if(_cmdUndo == null)
                {
                    _cmdUndo = new RelayCommand(
                        execute: _ =>
                        {
                            Init();
                        },
                        canExecute: _ => true);
                    
                }
                return _cmdUndo;
            }
        }
     
    }
}
