using System;
using System.Collections.Generic;
using System.Linq;
using APBD_Cw1_s30790.Exceptions;
using APBD_Cw1_s30790.Models;

namespace APBD_Cw1_s30790.Services
{
    public class WypozyczalniaService
    {
        private readonly List<Uzytkownik> _uzytkownicy = new List<Uzytkownik>();
        private readonly List<Sprzet> _sprzet = new List<Sprzet>();
        private readonly List<Wypozyczenie> _wypozyczenia = new List<Wypozyczenie>();
        private readonly IKalkulatorKar _kalkulatorKar;

        public WypozyczalniaService(IKalkulatorKar kalkulatorKar)
        {
            _kalkulatorKar = kalkulatorKar;
        }

        public void DodajUzytkownika(Uzytkownik uzytkownik) => _uzytkownicy.Add(uzytkownik);
        
        public void DodajSprzet(Sprzet sprzet) => _sprzet.Add(sprzet);

        public IEnumerable<Sprzet> PobierzCalySprzet() => _sprzet;

        public IEnumerable<Sprzet> PobierzDostepnySprzet() => _sprzet.Where(s => s.Status == StatusSprzetu.Dostepny);

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
                throw new RegulaBiznesowaException("Nie można zmienić statusu sprzętu, który jest obecnie wypożyczony.");
                
            sprzet.Status = StatusSprzetu.Niedostepny;
        }

        public IEnumerable<Wypozyczenie> PobierzAktywneWypozyczenia(Guid idUzytkownika) =>
            _wypozyczenia.Where(w => w.Uzytkownik.Id == idUzytkownika && w.CzyAktywne);

        public IEnumerable<Wypozyczenie> PobierzPrzeterminowaneWypozyczenia(DateTime aktualnaData) =>
            _wypozyczenia.Where(w => w.CzyPrzeterminowane(aktualnaData));

        public string GenerujRaport(DateTime aktualnaData)
        {
            var calkowitaIlosc = _sprzet.Count;
            var dostepne = _sprzet.Count(s => s.Status == StatusSprzetu.Dostepny);
            var niedostepne = _sprzet.Count(s => s.Status == StatusSprzetu.Niedostepny);
            var aktywneWyp = _wypozyczenia.Count(w => w.CzyAktywne);
            var przeterminowaneWyp = _wypozyczenia.Count(w => w.CzyPrzeterminowane(aktualnaData));
            var sumaKar = _wypozyczenia.Sum(w => w.Kara);

            return $"Raport wypożyczalni:\nSprzęt łącznie: {calkowitaIlosc}\nDostępny sprzęt: {dostepne}\nSprzęt wycofany/w serwisie: {niedostepne}\nAktywne wypożyczenia: {aktywneWyp}\nPrzeterminowane: {przeterminowaneWyp}\nZainkasowane kary: {sumaKar} PLN";
        }
    }
}