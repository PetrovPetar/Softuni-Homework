

namespace EntityFrameworkHomework
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    class Startup
    {
        static void Main(string[] args)
        {
            var db = new SoftuniContext();

            var townToDelete = Console.ReadLine();
            var addressesToDelete = db.Addresses
                                        .Where(a => a.Town.Name == townToDelete)
                                        .Select(a => a.AddressID).ToList();
            var addressesCount = addressesToDelete.Count();

            foreach (var addressId in addressesToDelete)
            {
                db.Employees.Where(e => e.Address.AddressID == addressId)
                                .ToList()
                                .ForEach(e => e.AddressID = null);
                var currentAddress = db.Addresses.FirstOrDefault(a => a.AddressID == addressId);
                db.Addresses.Remove(currentAddress);
            }

            var town = db.Towns.FirstOrDefault(t => t.Name == townToDelete);
            db.Towns.Remove(town);

            if (addressesCount > 1)
            {
                Console.WriteLine($"{addressesCount} addresses in {townToDelete} were deleted");
            }
            else
            {
                Console.WriteLine($"{addressesCount} address in {townToDelete} was deleted");
            }

            db.SaveChanges();
        }
    }
}
