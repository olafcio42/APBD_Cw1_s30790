using System;
using System.Text.Json.Serialization;

namespace APBD_Cw1_s30790.Models
{
    [JsonDerivedType(typeof(Laptop), typeDiscriminator: "Laptop")]
    [JsonDerivedType(typeof(Projektor), typeDiscriminator: "Projektor")]
    [JsonDerivedType(typeof(Kamera), typeDiscriminator: "Kamera")]
    public abstract class Sprzet
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Nazwa { get; set; }
        public StatusSprzetu Status { get; set; } = StatusSprzetu.Dostepny;

        public abstract string PobierzInformacje();
    }
}