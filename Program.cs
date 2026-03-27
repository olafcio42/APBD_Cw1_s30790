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
            var kalkulator = new StalyKalkulatorKar(10.0m);
            var wypozyczalnia = new WypozyczalniaService(kalkulator);
            var plikDanych = "baza.json";

            while (true)
            {
                Console.WriteLine("\n=== MENU ===");
                Console.WriteLine("1. Wyświetl sprzęt");
                Console.WriteLine("2. Wyświetl dostępny sprzęt");
                Console.WriteLine("3. Wyświetl użytkowników");
                Console.WriteLine("4. Wypożycz sprzęt");
                Console.WriteLine("5. Zwróć sprzęt");
                Console.WriteLine("6. Generuj raport");
                Console.WriteLine("7. Zapisz dane do JSON");
                Console.WriteLine("8. Wczytaj dane z JSON");
                Console.WriteLine("9. Załaduj testowe dane");
                Console.WriteLine("0. Wyjście");
                Console.Write("Wybierz opcję: ");

                var opcja = Console.ReadLine();

                try
                {
                    switch (opcja)
                    {
                        case "1":
                            foreach (var s in wypozyczalnia.PobierzCalySprzet())
                                Console.WriteLine($"[{s.Id}] {s.PobierzInformacje()} | Status: {s.Status}");
                            break;
                        case "2":
                            foreach (var s in wypozyczalnia.PobierzDostepnySprzet())
                                Console.WriteLine($"[{s.Id}] {s.PobierzInformacje()} | Status: {s.Status}");
                            break;
                        case "3":
                            foreach (var u in wypozyczalnia.PobierzUzytkownikow())
                                Console.WriteLine($"[{u.Id}] {u.Imie} {u.Nazwisko} (Limit: {u.LimitWypozyczen})");
                            break;
                        case "4":
                            Console.Write("ID użytkownika: ");
                            var idU = Guid.Parse(Console.ReadLine() ?? string.Empty);
                            Console.Write("ID sprzętu: ");
                            var idS = Guid.Parse(Console.ReadLine() ?? string.Empty);
                            Console.Write("Dni: ");
                            var dni = int.Parse(Console.ReadLine() ?? "0");
                            wypozyczalnia.Wypozycz(idU, idS, dni, DateTime.Now);
                            Console.WriteLine("Wypożyczono.");
                            break;
                        case "5":
                            Console.WriteLine("Aktywne wypożyczenia:");
                            foreach(var w in wypozyczalnia.PobierzWypozyczenia().Where(x => x.CzyAktywne))
                                Console.WriteLine($"ID: {w.Id} | Kto: {w.Uzytkownik.Nazwisko} | Co: {w.Sprzet.Nazwa}");
                            Console.Write("ID wypożyczenia: ");
                            var idW = Guid.Parse(Console.ReadLine() ?? string.Empty);
                            wypozyczalnia.Zwroc(idW, DateTime.Now);
                            Console.WriteLine("Zwrócono.");
                            break;
                        case "6":
                            Console.WriteLine("Filtr: 1-Wszystko, 2-Tylko dostępne, 3-Przeterminowane");
                            var filtrOpcja = Console.ReadLine();
                            var filtr = filtrOpcja == "2" ? "Dostepne" : (filtrOpcja == "3" ? "Przeterminowane" : "Wszystko");
                            Console.WriteLine(wypozyczalnia.GenerujRaport(DateTime.Now, filtr));
                            break;
                        case "7":
                            wypozyczalnia.ZapiszDoJson(plikDanych);
                            Console.WriteLine("Zapisano.");
                            break;
                        case "8":
                            wypozyczalnia.WczytajZJson(plikDanych);
                            Console.WriteLine("Wczytano.");
                            break;
                        case "9":
                            wypozyczalnia.DodajSprzet(new Laptop { Nazwa = "Dell XPS", PamiecRamGB = 16, Procesor = "i7" });
                            wypozyczalnia.DodajSprzet(new Kamera { Nazwa = "GoPro", FormatNagrywania = "4K" });
                            wypozyczalnia.DodajUzytkownika(new Student { Imie = "Adam", Nazwisko = "Nowak" });
                            Console.WriteLine("Dodano.");
                            break;
                        case "0":
                            return;
                        default:
                            Console.WriteLine("Nieznana opcja.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"BŁĄD: {ex.Message}");
                }
            }
        }
    }
}