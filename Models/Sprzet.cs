using System;

namespace APBD_Cw1_s30790.Models
{
    public abstract class Sprzet
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string Nazwa { get; set; }
        public StatusSprzetu Status { get; set; } = StatusSprzetu.Dostepny;

        public abstract string PobierzInformacje();
    }
}