using System.ComponentModel.DataAnnotations;

namespace  PruebaApi.Models.Dto
{
    public class VillaDto
    {
        public  int Id { get; set; }  
        [Required]   
        public string? Nombre { get; set; }
        public  int Ocupantes { get; set; }         
        public int MetrosCuadrados { get; set; }
         public string Detalle { get; set; }   
        [Required]
        public double Tarifa { get; set; }
        public string ImagenUrl { get; set; }
        public string Amenidad { get; set; }
        public DateTime FechaActualizacion { get; set; }    
    }
}
