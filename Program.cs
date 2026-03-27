using System;
using System.Linq;
using APBD_Cw1_s30790.Exceptions;
using APBD_Cw1_s30790.Models;
using APBD_Cw1_s30790.Services;

namespace APBD_Cw1_s30790
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var dzisiaj = DateTime.Now;
            var kalkulator = new StalyKalkulatorKar(10.0m);
            var wypozyczalnia = new WypozyczalniaService(kalkulator);

            var laptop1 = new Laptop { Nazwa = "Dell XPS", PamiecRamGB = 16, Procesor = "i7" };
            var kamera1 = new Kamera { Nazwa = "Sony Alpha", FormatNagrywania = "4K", WymiennyObiektyw = true };
            var projektor1 = new Projektor { Nazwa = "Epson", JasnoscANSI = 3000, Rozdzielczosc = "1080p" };
            var uszkodzonyLaptop = new Laptop { Nazwa = "Lenovo ThinkPad", PamiecRamGB = 8, Procesor = "i5" };
            
            
            wypozyczalnia.DodajSprzet(laptop1);
            wypozyczalnia.DodajSprzet(kamera1);
            wypozyczalnia.DodajSprzet(projektor1);
            wypozyczalnia.DodajSprzet(uszkodzonyLaptop);

            var student = new Student { Imie = "Jan", Nazwisko = "Kowalski" };
            var pracownik = new Pracownik { Imie = "Anna", Nazwisko = "Nowak" };

            wypozyczalnia.DodajUzytkownika(student);
            wypozyczalnia.DodajUzytkownika(pracownik);
            
            Console.WriteLine("--- Oznaczanie sprzętu jako niedostępnego ---");
            wypozyczalnia.OznaczJakoNiedostepny(uszkodzonyLaptop.Id);
            Console.WriteLine($"Status {uszkodzonyLaptop.Nazwa}: {uszkodzonyLaptop.Status}");

            Console.WriteLine("\n--- Poprawne wypożyczenie ---");
            var wyp1 = wypozyczalnia.Wypozycz(pracownik.Id, laptop1.Id, 7, dzisiaj);
            Console.WriteLine($"Pracownik wypożyczył: {wyp1.Sprzet.Nazwa} do {wyp1.PlanowanaDataZwrotu.ToShortDateString()}");

            Console.WriteLine("\n--- Próba wypożyczenia niedostępnego sprzętu ---");
            try
            {
                wypozyczalnia.Wypozycz(student.Id, uszkodzonyLaptop.Id, 3, dzisiaj);
            }
            catch (RegulaBiznesowaException ex)
            {
                Console.WriteLine($"Oczekiwany błąd: {ex.Message}");
            }

            Console.WriteLine("\n--- Próba przekroczenia limitu przez studenta ---");
            try
            {
                wypozyczalnia.Wypozycz(student.Id, kamera1.Id, 3, dzisiaj);
                var kamera2 = new Kamera { Nazwa = "GoPro", FormatNagrywania = "1080p" };
                wypozyczalnia.DodajSprzet(kamera2);
                wypozyczalnia.Wypozycz(student.Id, kamera2.Id, 3, dzisiaj);
                
                wypozyczalnia.Wypozycz(student.Id, projektor1.Id, 2, dzisiaj);
            }
            catch (RegulaBiznesowaException ex)
            {
                Console.WriteLine($"Oczekiwany błąd: {ex.Message}");
            }
            
            Console.WriteLine("\n--- Zwrot sprzętu w terminie ---");
            wypozyczalnia.Zwroc(wyp1.Id, dzisiaj.AddDays(5));
            Console.WriteLine($"Zwrócono. Naliczone kary: {wyp1.Kara} PLN");

            Console.WriteLine("\n--- Zwrot opóźniony skutkujący naliczeniem kary ---");
            var wypStudent = wypozyczalnia.PobierzAktywneWypozyczenia(student.Id).First();
            wypozyczalnia.Zwroc(wypStudent.Id, dzisiaj.AddDays(5)); 
            Console.WriteLine($"Zwrócono po terminie. Naliczone kary (2 dni): {wypStudent.Kara} PLN");

            Console.WriteLine("\n--- Raport końcowy ---");
            Console.WriteLine(wypozyczalnia.GenerujRaport(dzisiaj.AddDays(5)));
            
            Console.ReadLine();
        }
    }
}