using System;

namespace APBD_Cw1_s30790.Services
{
    public interface IKalkulatorKar
    {
        decimal ObliczKare(DateTime planowanaDataZwrotu, DateTime rzeczywistaDataZwrotu);
    }
}