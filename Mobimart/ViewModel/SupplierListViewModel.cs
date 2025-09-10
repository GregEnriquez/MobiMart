using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MobiMart.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobiMart.ViewModel
{
    class SupplierListViewModel
    {
        public ObservableCollection<Supplier> Suppliers { get; set; }

        public SupplierListViewModel()
        {
            Suppliers = new ObservableCollection<Supplier>
            {
                new Supplier
                {
                    Id=1,
                    BusinessId=1,
                    Type="Consignment",
                    Name="Marlboro",
                    Email = "email@sample.com",
                    Number="09123456789",
                    Socials="facebook.com/sampleSocials",
                    LastModified="2025/09/09"
                },

                new Supplier
                {
                    Id=2,
                    BusinessId=2,
                    Type="Wholesale",
                    Name="Zesto",
                    Email = "email1@sample.com",
                    Number="09123456781",
                    Socials="facebook.com/sampleSocials1",
                    LastModified="2025/09/09"
                },

                new Supplier
                {
                    Id=3,
                    BusinessId=3,
                    Type="Wholesale",
                    Name="Coca Cola",
                    Email = "email2@sample.com",
                    Number="09123456782",
                    Socials="facebook.com/sampleSocials2",
                    LastModified="2025/09/09"
                }
            };
        }
    }
}