using PruebaApi.Models.Dto;

namespace PruebaApi.Datos
{
    public static class VillasStore
    {
        public static List<VillaDto> villaList = new List<VillaDto>
        {
            new VillaDto{Id=1,Nombre="Vista a la piscina",Ocupantes=3,MetrosCuadrados=50},
            new VillaDto{Id=2,Nombre="Vista a la playa",Ocupantes=4,MetrosCuadrados=80}
        };
    }

}