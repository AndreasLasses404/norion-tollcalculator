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
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Norion.TollCalculator.Infrastructure.Repository
{
    public class TollRepository: ITollRepository
    {
        private readonly List<TollFee> TollFees = new();
        private readonly List<Vehicle> vehicles = new List<Vehicle>();

        public TollRepository()
        {
            TollFees = ReadTollFees("../Norion.TollCalculator.Infrastructure/TollCalculator.csv");
        }

        public async Task<int> GetTotalTollFee(Guid id)
        {
            var vehicle = vehicles.FirstOrDefault(x => x.Id == id);

            if (vehicle == null)
                throw new NullReferenceException();

            DateTime intervalStart = vehicle.TotalDailyPassages[0];
            int tempFee = GetTollFee(intervalStart);
            int totalFee = 0;
            foreach (DateTime date in vehicle.TotalDailyPassages)
            {
                int nextFee = GetTollFee(date);

                TimeSpan timeBetweenPassages = DateTime.Now - vehicle.LastPassage;

                if (timeBetweenPassages.TotalMinutes <= 60)
                {
                    if (totalFee > 0)
                    {
                        totalFee -= tempFee;
                    }

                    //Make sure that the highest of the two tollpassages gets debited to driver
                    if (nextFee >= tempFee) tempFee = nextFee;
                    {
                        totalFee += tempFee;
                    }

                    //Change the value of the interval to refresh the 60 minute timer for vehicle to pass next toll
                    //"En bil som passerar flera betalstationer inom 60 minuter bara beskattas en gång."
                    intervalStart = date;
                }
                else
                {
                    totalFee += nextFee;

                    //Change the value of the interval to refresh the 60 minute timer for vehicle to pass next toll
                    //"En bil som passerar flera betalstationer inom 60 minuter bara beskattas en gång."
                    intervalStart = date;
                }

                //Updating tempfee to value of current fee (nextFee)
                //This way lets us skip doing the method more than once
                tempFee = nextFee;

                //Moved if-statement inside loop to break out of loop if true.
                if (totalFee > 60) return 60;
            }
            return totalFee;
        }

        public async Task AddPassage(Guid id, DateTime passageTime)
        {
            var vehicle = vehicles.FirstOrDefault(x => x.Id == id);
            if (vehicle == null)
                throw new NullReferenceException(nameof(vehicle));

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

        private List<TollFee> ReadTollFees(string filePath)
        {
            List<TollFee> tollFees = new List<TollFee>();

            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');

                        if (parts.Length == 3)
                        {
                            TimeSpan startTime;
                            if (TimeSpan.TryParseExact(parts[0], "hh\\:mm\\:ss", CultureInfo.InvariantCulture, out startTime))
                            {
                                TimeSpan endTime;
                                if (TimeSpan.TryParseExact(parts[1], "hh\\:mm\\:ss", CultureInfo.InvariantCulture, out endTime))
                                {
                                    int amount;
                                    if (int.TryParse(parts[2].Split(',')[0], out amount)) // Remove " kr" and parse amount
                                    {
                                        tollFees.Add(new TollFee { StartTime = startTime, EndTime = endTime, Amount = amount });
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
            }

            return tollFees;
        }

        public int GetTollFee(DateTime date)
        {
            if (IsTollFreeDate(date)) return 0;
            return TollFees.Where(t => t.StartTime <= date.TimeOfDay && t.EndTime >= date.TimeOfDay).Select(t => t.Amount).FirstOrDefault();
            //int hour = date.Hour;
            //int minute = date.Minute; 
            //
            //if (hour == 6 && minute >= 0 && minute <= 29) return 8;
            //else if (hour == 6 && minute >= 30 && minute <= 59) return 13;
            //else if (hour == 7 && minute >= 0 && minute <= 59) return 18;
            //else if (hour == 8 && minute >= 0 && minute <= 29) return 13;
            //else if (hour >= 8 && hour <= 14 && minute >= 30 && minute <= 59) return 8;
            //else if (hour == 15 && minute >= 0 && minute <= 29) return 13;
            //else if (hour == 15 && minute >= 0 || hour == 16 && minute <= 59) return 18;
            //else if (hour == 17 && minute >= 0 && minute <= 59) return 13;
            //else if (hour == 18 && minute >= 0 && minute <= 29) return 8;
            //else return 0;
        }

        private bool IsTollFreeDate(DateTime date)
        {
            int year = date.Year;
            int month = date.Month;
            int day = date.Day;

            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return true;

            if (year == 2013)
            {
                if (month == 1 && day == 1 ||
                    month == 3 && (day == 28 || day == 29) ||
                    month == 4 && (day == 1 || day == 30) ||
                    month == 5 && (day == 1 || day == 8 || day == 9) ||
                    month == 6 && (day == 5 || day == 6 || day == 21) ||
                    month == 7 ||
                    month == 11 && day == 1 ||
                    month == 12 && (day == 24 || day == 25 || day == 26 || day == 31))
                {
                    return true;
                }
            }
            return false;
        }

    }
}
