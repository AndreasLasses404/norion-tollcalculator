/**
 * Varje passage genom en betalstation i Göteborg kostar 8, 13 eller 18 kronor beroende på tidpunkt. 
 * Det maximala beloppet per dag och fordon är 60 kronor.

Tider           Belopp
06:00–06:29     8 kr
06:30–06:59     13 kr
07:00–07:59     18 kr
08:00–08:29     13 kr
08:30–14:59     8 kr
15:00–15:29     13 kr
15:30–16:59     18 kr
17:00–17:59     13 kr
18:00–18:29     8 kr
18:30–05:59     0 kr

Trängselskatt tas ut för fordon som passerar en betalstation måndag till fredag mellan 06.00 och 18.29. 
Skatt tas inte ut lördagar, helgdagar, dagar före helgdag eller under juli månad. 
Vissa fordon är undantagna från trängselskatt. 
En bil som passerar flera betalstationer inom 60 minuter bara beskattas en gång. 
Det belopp som då ska betalas är det högsta beloppet av de passagerna.
*/

using Norion.TollCalculator.Domain.Models;
using Norion.TollCalculator.Domain.Repository;
using Norion.TollCalculator.Infrastructure.Models;
using System.Globalization;

namespace Norion.TollCalculator.Infrastructure.Repository
{
    public class TollRepository : ITollRepository
    {

        private readonly List<Vehicle> vehicles = new List<Vehicle>();

        public TollRepository()
        {

        }

        public async Task<Vehicle> GetVehicle(Guid id)
        {
            await Task.CompletedTask;
            return vehicles.FirstOrDefault(x => x.Id == id) ?? null;
        }

        public async Task AddPassage(Guid id, DateTime passageTime)
        {
            var vehicle = vehicles.FirstOrDefault(x => x.Id == id);
            if (vehicle == null)
                throw new NullReferenceException(nameof(vehicle));

            if(passageTime == default)
                throw new InvalidDataException(nameof(passageTime));

            if (passageTime > DateTime.Now)
                throw new InvalidDataException(nameof(passageTime));

            vehicle.LastPassage = passageTime;
            vehicle.TotalDailyPassages.Add(passageTime);

            await Task.CompletedTask;
        }

        public async Task<Guid> AddVehicle(Vehicle vehicle)
        {
            vehicle.Id = Guid.NewGuid();
            vehicles.Add(vehicle);
            await Task.CompletedTask;
            return vehicle.Id;

        }
    }
}
