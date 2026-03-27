using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using APBD_Cw1_s30790.Exceptions;
using APBD_Cw1_s30790.Models;

namespace APBD_Cw1_s30790.Services
{
    public class WypozyczalniaService
    {
        private List<Uzytkownik> _uzytkownicy = new List<Uzytkownik>();
        private List<Sprzet> _sprzet = new List<Sprzet>();
        private List<Wypozyczenie> _wypozyczenia = new List<Wypozyczenie>();
        private readonly IKalkulatorKar _kalkulatorKar;

        public WypozyczalniaService(IKalkulatorKar kalkulatorKar)
        {
            _kalkulatorKar = kalkulatorKar;
        }

        public void DodajUzytkownika(Uzytkownik uzytkownik) => _uzytkownicy.Add(uzytkownik);
        
        public void DodajSprzet(Sprzet sprzet) => _sprzet.Add(sprzet);

        public IEnumerable<Uzytkownik> PobierzUzytkownikow() => _uzytkownicy;

        public IEnumerable<Sprzet> PobierzCalySprzet() => _sprzet;

        public IEnumerable<Sprzet> PobierzDostepnySprzet() => _sprzet.Where(s => s.Status == StatusSprzetu.Dostepny);
        
        public IEnumerable<Wypozyczenie> PobierzWypozyczenia() => _wypozyczenia;

        public Wypozyczenie Wypozycz(Guid idUzytkownika, Guid idSprzetu, int dni, DateTime aktualnaData)
        {
            var uzytkownik = _uzytkownicy.FirstOrDefault(u => u.Id == idUzytkownika) ?? throw new ArgumentException("Nie znaleziono użytkownika");
            var sprzet = _sprzet.FirstOrDefault(s => s.Id == idSprzetu) ?? throw new ArgumentException("Nie znaleziono sprzętu");

            if (sprzet.Status != StatusSprzetu.Dostepny)
                throw new RegulaBiznesowaException("Sprzęt nie jest dostępny do wypożyczenia.");

            var liczbaAktywnychWypozyczen = _wypozyczenia.Count(w => w.Uzytkownik.Id == idUzytkownika && w.CzyAktywne);
            if (liczbaAktywnychWypozyczen >= uzytkownik.LimitWypozyczen)
                throw new RegulaBiznesowaException("Przekroczono limit aktywnych wypożyczeń dla tego użytkownika.");

            sprzet.Status = StatusSprzetu.Wypozyczony;

            var wypozyczenie = new Wypozyczenie
            {
                Uzytkownik = uzytkownik,
                Sprzet = sprzet,
                DataWypozyczenia = aktualnaData,
                PlanowanaDataZwrotu = aktualnaData.AddDays(dni)
            };
            
            _wypozyczenia.Add(wypozyczenie);
            return wypozyczenie;
        }

        public void Zwroc(Guid idWypozyczenia, DateTime dataZwrotu)
        {
            var wypozyczenie = _wypozyczenia.FirstOrDefault(w => w.Id == idWypozyczenia) ?? throw new ArgumentException("Nie znaleziono wypożyczenia");
            
            if (!wypozyczenie.CzyAktywne) 
                throw new RegulaBiznesowaException("Wypożyczenie zostało już zakończone.");

            wypozyczenie.RzeczywistaDataZwrotu = dataZwrotu;
            wypozyczenie.Sprzet.Status = StatusSprzetu.Dostepny;
            wypozyczenie.Kara = _kalkulatorKar.ObliczKare(wypozyczenie.PlanowanaDataZwrotu, dataZwrotu);
        }

        public void OznaczJakoNiedostepny(Guid idSprzetu)
        {
            var sprzet = _sprzet.FirstOrDefault(s => s.Id == idSprzetu) ?? throw new ArgumentException("Nie znaleziono sprzętu");
            
            if (sprzet.Status == StatusSprzetu.Wypozyczony) 
                throw new RegulaBiznesowaException("Nie można zmienić statusu sprzętu, który jest wypożyczony.");
                
            sprzet.Status = StatusSprzetu.Niedostepny;
        }

        public string GenerujRaport(DateTime aktualnaData, string filtr = "Wszystko")
        {
            IEnumerable<Sprzet> filtrowanySprzet = _sprzet;
            IEnumerable<Wypozyczenie> filtrowaneWypozyczenia = _wypozyczenia;

            if (filtr == "Dostepne") filtrowanySprzet = _sprzet.Where(s => s.Status == StatusSprzetu.Dostepny);
            if (filtr == "Przeterminowane") filtrowaneWypozyczenia = _wypozyczenia.Where(w => w.CzyPrzeterminowane(aktualnaData));

            var sprzetIlosc = filtrowanySprzet.Count();
            var aktywneWyp = filtrowaneWypozyczenia.Count(w => w.CzyAktywne);
            var sumaKar = filtrowaneWypozyczenia.Sum(w => w.Kara);

            return $"Raport ({filtr}):\nSprzęt: {sprzetIlosc}\nAktywne/Przeterminowane wypożyczenia: {aktywneWyp}\nSuma kar: {sumaKar} PLN";
        }

        public void ZapiszDoJson(string sciezka)
        {
            var dto = new BazaDanychDto { Uzytkownicy = _uzytkownicy, Sprzet = _sprzet, Wypozyczenia = _wypozyczenia };
            var options = new JsonSerializerOptions { WriteIndented = true, ReferenceHandler = ReferenceHandler.Preserve };
            var json = JsonSerializer.Serialize(dto, options);
            File.WriteAllText(sciezka, json);
        }

        public void WczytajZJson(string sciezka)
        {
            if (!File.Exists(sciezka)) throw new FileNotFoundException("Brak pliku do wczytania.");
            var json = File.ReadAllText(sciezka);
            var options = new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve };
            var dto = JsonSerializer.Deserialize<BazaDanychDto>(json, options);
            
            if (dto != null)
            {
                _uzytkownicy = dto.Uzytkownicy ?? new List<Uzytkownik>();
                _sprzet = dto.Sprzet ?? new List<Sprzet>();
                _wypozyczenia = dto.Wypozyczenia ?? new List<Wypozyczenie>();
            }
        }
    }
}