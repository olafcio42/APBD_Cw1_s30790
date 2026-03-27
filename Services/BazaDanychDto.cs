using System.Collections.Generic;
using APBD_Cw1_s30790.Models;

namespace APBD_Cw1_s30790.Services
{
    public class BazaDanychDto
    {
        public List<Uzytkownik> Uzytkownicy { get; set; } = new List<Uzytkownik>();
        public List<Sprzet> Sprzet { get; set; } = new List<Sprzet>();
        public List<Wypozyczenie> Wypozyczenia { get; set; } = new List<Wypozyczenie>();
    }
}