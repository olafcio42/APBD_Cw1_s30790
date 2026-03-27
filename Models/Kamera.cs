namespace APBD_Cw1_s30790.Models
{
    public class Kamera : Sprzet
    {
        public string FormatNagrywania { get; set; }
        public bool WymiennyObiektyw { get; set; }

        public override string PobierzInformacje() => $"Kamera: {Nazwa}, Format: {FormatNagrywania}, Wymienny obiektyw: {WymiennyObiektyw}";
    }
}