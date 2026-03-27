namespace APBD_Cw1_s30790.Models
{
    public class Laptop : Sprzet
    {
        public string Procesor { get; set; }
        public int PamiecRamGB { get; set; }

        public override string PobierzInformacje() => $"Laptop: {Nazwa}, Procesor: {Procesor}, RAM: {PamiecRamGB}GB";
    }
}