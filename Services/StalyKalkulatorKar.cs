using System;

namespace APBD_Cw1_s30790.Services
{
    public class StalyKalkulatorKar : IKalkulatorKar
    {
        private readonly decimal _kwotaZaDzien;

        public StalyKalkulatorKar(decimal kwotaZaDzien)
        {
            _kwotaZaDzien = kwotaZaDzien;
        }

        public decimal ObliczKare(DateTime planowanaDataZwrotu, DateTime rzeczywistaDataZwrotu)
        {
            if (rzeczywistaDataZwrotu <= planowanaDataZwrotu) return 0;
            
            var liczbaDniOpoznienia = (rzeczywistaDataZwrotu - planowanaDataZwrotu).Days;
            return liczbaDniOpoznienia * _kwotaZaDzien;
        }
    }
}