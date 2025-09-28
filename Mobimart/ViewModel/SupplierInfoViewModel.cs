using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MobiMart.Model;
using MobiMart.Service;
using MobiMart.View;

namespace MobiMart.ViewModel
{
    public partial class SupplierInfoViewModel : BaseViewModel
    {
        // public ObservableCollection<ContactInfo> contacts { get; set; }
        UserService userService;
        SupplierService supplierService;

        [ObservableProperty]
        Supplier supplier;

        public SupplierInfoViewModel(UserService userService, SupplierService supplierService)
        {
            this.userService = userService;
            this.supplierService = supplierService;

            // contacts = new ObservableCollection<ContactInfo>
            // {
            //     new ContactInfo
            //     {
            //         sContactId=1,
            //         supplierId=1,
            //         cName="Max",
            //         cEmail = "email1@sample.com",
            //         cNumber="1234567891",
            //         cSocials = "facebook.com/contact1",
            //     },

            //     new ContactInfo
            //     {
            //         sContactId=2,
            //         supplierId=2,
            //         cName="Versta",
            //         cEmail = "email2@sample.com",
            //         cNumber="1234567892",
            //         cSocials = "facebook.com/contact2",
            //     },

            //     new ContactInfo
            //     {
            //         sContactId=3,
            //         supplierId=1,
            //         cName="Phen",
            //         cEmail = "email3@sample.com",
            //         cNumber="1234567893",
            //         cSocials = "facebook.com/contact3",
            //     },

            //     new ContactInfo
            //     {
            //         sContactId=4,
            //         supplierId=3,
            //         cName="Wah",
            //         cEmail = "email4@sample.com",
            //         cNumber="1234567894",
            //         cSocials = "facebook.com/contact4",
            //     },
            // };
        }

        [RelayCommand]
        public async Task EditSupplier()
        {
            if (IsBusy) return;
            IsBusy = true;

            var navParameter = new Dictionary<string, object>()
            {
                {"Supplier", Supplier }
            };

            await Shell.Current.GoToAsync(nameof(AddSupplier), navParameter);
            IsBusy = false;
        }


        [RelayCommand]
        public async Task CheckInventory()
        {
            if (IsBusy) return;

            IsBusy = true;
            var navParameter = new Dictionary<string, object>()
            {
                {"Supplier", Supplier }
            };
            await Shell.Current.GoToAsync(nameof(SupplierInventory), true, navParameter);
            IsBusy = false;
        }
    }
}
