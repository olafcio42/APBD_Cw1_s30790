namespace APBD_Cw1_s30790.Models
{
    public class Student : Uzytkownik
    {
        public override int LimitWypozyczen => 2;
    }
}