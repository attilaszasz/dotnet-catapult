using Interfaces;
using Types;

namespace Helpers
{
    public static class SuppliersHelper
    {
        public static IWeatherSupplier GetSupplier(this IEnumerable<Lazy<IWeatherSupplier, SupplierMetadata>> suppliers, string supplierName)
        {
            var supplier = suppliers.FirstOrDefault(s => s.Metadata.Name == supplierName);
            if (supplier == null)
            {
                throw new ArgumentException($"Unknown supplier: {supplierName}");
            }
            return supplier.Value;
        }
    }
}
