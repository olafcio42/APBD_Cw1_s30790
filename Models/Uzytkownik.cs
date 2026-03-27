using System;
using System.Text.Json.Serialization;

namespace APBD_Cw1_s30790.Models
{
    [JsonDerivedType(typeof(Student), typeDiscriminator: "Student")]
    [JsonDerivedType(typeof(Pracownik), typeDiscriminator: "Pracownik")]
    public abstract class Uzytkownik
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        
        [JsonIgnore]
        public abstract int LimitWypozyczen { get; }
    }
}