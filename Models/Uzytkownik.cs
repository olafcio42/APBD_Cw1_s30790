using System;

namespace APBD_Cw1_s30790.Models
{
    public abstract class Uzytkownik
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        
        public abstract int LimitWypozyczen { get; }
    }
}