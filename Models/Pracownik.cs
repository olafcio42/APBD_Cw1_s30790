namespace APBD_Cw1_s30790.Models
{
    public class Pracownik : Uzytkownik
    {
        public override int LimitWypozyczen => 5;
    }
}