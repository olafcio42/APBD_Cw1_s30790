namespace APBD_Cw1_s30790.Models
{
    public class Projektor : Sprzet
    {
        public string Rozdzielczosc { get; set; }
        public int JasnoscANSI { get; set; }

        public override string PobierzInformacje() => $"Projektor: {Nazwa}, Rozdzielczość: {Rozdzielczosc}, Jasność: {JasnoscANSI} ANSI";
    }
}