using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobiMart.Model;

namespace MobiMart.ViewModel
{
    class SupplierInfoViewModel
    {
        public ObservableCollection<ContactInfo> contacts { get; set; }

        public SupplierInfoViewModel()
        {
            contacts = new ObservableCollection<ContactInfo>
            {
                new ContactInfo
                {
                    sContactId=1,
                    supplierId=1,
                    cName="Max",
                    cEmail = "email1@sample.com",
                    cNumber="1234567891",
                    cSocials = "facebook.com/contact1",
                },

                new ContactInfo
                {
                    sContactId=2,
                    supplierId=2,
                    cName="Versta",
                    cEmail = "email2@sample.com",
                    cNumber="1234567892",
                    cSocials = "facebook.com/contact2",
                },

                new ContactInfo
                {
                    sContactId=3,
                    supplierId=1,
                    cName="Phen",
                    cEmail = "email3@sample.com",
                    cNumber="1234567893",
                    cSocials = "facebook.com/contact3",
                },

                new ContactInfo
                {
                    sContactId=4,
                    supplierId=3,
                    cName="Wah",
                    cEmail = "email4@sample.com",
                    cNumber="1234567894",
                    cSocials = "facebook.com/contact4",
                },
            };
        }
    }
}
