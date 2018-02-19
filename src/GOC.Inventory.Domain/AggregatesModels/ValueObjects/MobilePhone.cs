using GOC.Inventory.Domain.Enums;

namespace GOC.Inventory.Domain.AggregatesModels.ValueObjects
{
    public class MobilePhone : ValueObject<MobilePhone>
    {
        public string Color { get; private set; }
        public string Carrier { get; private set; }
        public string StorageSize { get; private set; }
        public string Imei { get; private set; }
        public string Manufacturer { get; private set; }
        public ConditionTypes Condition { get; private set; }

        public MobilePhone (string color, string carrier, string storageSize, string imei, ConditionTypes condition, string manufacturer)
        {
            Color = color;
            Carrier = carrier;
            StorageSize = storageSize;
            Imei = imei;
            Condition = condition;
            Manufacturer = manufacturer;
        }

        private MobilePhone() { }
    }
}
