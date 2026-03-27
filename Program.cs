using System;
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
            
            
            
        }
    }
}