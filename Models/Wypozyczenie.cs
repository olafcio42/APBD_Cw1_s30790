using System;

namespace APBD_Cw1_s30790.Models
{
    public class Wypozyczenie
    {
        public Guid Id { get; } = Guid.NewGuid();
        public Uzytkownik Uzytkownik { get; set; }
        public Sprzet Sprzet { get; set; }
        public DateTime DataWypozyczenia { get; set; }
        public DateTime PlanowanaDataZwrotu { get; set; }
        public DateTime? RzeczywistaDataZwrotu { get; set; }
        public decimal Kara { get; set; }

        public bool CzyAktywne => !RzeczywistaDataZwrotu.HasValue;
        
        public bool CzyPrzeterminowane(DateTime aktualnaData) => CzyAktywne && aktualnaData > PlanowanaDataZwrotu;
    }
}