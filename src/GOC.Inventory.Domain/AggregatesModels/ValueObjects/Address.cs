using Newtonsoft.Json;

namespace GOC.Inventory.Domain.AggregatesModels.ValueObjects
{
    public class Address : ValueObject<Address>
    {
        public string AddressLine1 { get; private set; }
        public string AddressLine2 { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string ZipCode { get; private set; }


        public Address(string addressLine1, string addressLine2, string city, string state, string zipCode)
        {
            AddressLine1 = addressLine1;
            AddressLine2 = addressLine2;
            City = city;
            State = state;
            ZipCode = zipCode;
        }

        private Address(){}
    }
}
