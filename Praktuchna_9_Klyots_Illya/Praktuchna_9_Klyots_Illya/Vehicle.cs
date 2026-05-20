using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Praktuchna_9_Klyots_Illya
{
    public interface IVehicle
    {
        string Model { get; set; }
        string LicensePlate { get; set; }
        void Drive();
    }

    public abstract class Vehicle : IVehicle
    {
        public string Model { get; set; }
        public string LicensePlate { get; set; }

        protected Vehicle(string model, string plate)
        {
            Model = model;
            LicensePlate = plate;
        }

        public abstract void Drive();
    }

    // Похідні класи
    public class Car : Vehicle
    {
        public Car(string model, string plate) : base(model, plate) { }
        public override void Drive() => Console.WriteLine($"Легковик {Model} виїхав на дорогу.");
    }

    public class Bus : Vehicle
    {
        public Bus(string model, string plate) : base(model, plate) { }
        public override void Drive() => Console.WriteLine($"Автобус {Model} вирушив за маршрутом.");
    }

    public class Truck : Vehicle
    {
        public Truck(string model, string plate) : base(model, plate) { }
        public override void Drive() => Console.WriteLine($"Вантажівка {Model} везе товар.");
    }

    public class ElectricCar : Vehicle
    {
        public int BatteryCapacity { get; set; }
        public ElectricCar(string model, string plate, int battery) : base(model, plate)
        {
            BatteryCapacity = battery;
        }
        public override void Drive() => Console.WriteLine($"Електромобіль {Model} (заряд {BatteryCapacity} кВт) їде безшумно.");
    }
}
