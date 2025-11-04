using System;
using System.Text;

namespace Kino
{
    public delegate void CinemaD();   
    public delegate void SecurityD(); 
    public delegate void LightD(); 

    class Cinema
    {
        private int seatsCount;
        private int viewersInside = 0;

        public event CinemaD NotPlaces;

        public Cinema(int seats)
        {
            seatsCount = seats;
        }

        public void PushViewer(Viewer viewer)
        {
            viewersInside++;
            Console.WriteLine($"Глядач {viewer.Number} зайняв місце ({viewersInside}/{seatsCount})");

            if (viewersInside >= seatsCount)
            {
                Console.WriteLine("Зал заповнено повністю!");
                NotPlaces?.Invoke(); 
            }
        }
    }

    class Viewer
    {
        public int Number { get; }

        public Viewer(int number)
        {
            Number = number;
        }
    }

    class Security
    {
        public event SecurityD SwitchOff;

        public void CloseZal()
        {
            Console.WriteLine("Дежурний закрив зал.");
            SwitchOff?.Invoke();
        }
    }

    class Light
    {
        public event LightD Begin;

        public void Turn()
        {
            Console.WriteLine("Світло вимкнено!");
            Begin?.Invoke();
        }
    }

    class Hardware
    {
        private string filmName;

        public Hardware(string film)
        {
            filmName = film;
        }

        public void FilmOn()
        {
            Console.WriteLine($"Починається фільм «{filmName}»!");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.Write("Введіть кількість місць у залі: ");
            int seats;
            while (!int.TryParse(Console.ReadLine(), out seats) || seats <= 0)
            {
                Console.Write("Некоректне значення! Введіть додатне число: ");
            }

            Console.Write("Введіть назву фільму: ");
            string film = Console.ReadLine();

            Cinema cinema = new Cinema(seats);
            Security security = new Security();
            Light light = new Light();
            Hardware hardware = new Hardware(film);

            cinema.NotPlaces += security.CloseZal;  
            security.SwitchOff += light.Turn;       
            light.Begin += hardware.FilmOn;         

            for (int i = 1; i <= seats; i++)
            {
                Viewer v = new Viewer(i);
                cinema.PushViewer(v);
            }

            Console.ReadKey();
        }
    }
}
