using DatabaseLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interfaces
{
    public interface IAdressRL
    {
        AddAddress AddAddress(AddAddress addAddress, int userId);
        AddressModel UpdateAddress(AddressModel addressModel, int userId);
        string DeleteAddress(int addressId, int userId);
        AddressModel GetAddressById(int typeId, int addressId, int userId);
        List<AddressModel> GetAllAddresses(int userId);
    }
}
