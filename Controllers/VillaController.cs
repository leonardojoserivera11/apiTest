using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using PruebaApi.Datos;
using PruebaApi.Models;
using PruebaApi.Models.Dto;

namespace PruebaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController:ControllerBase
    {
       private readonly ILogger<VillaController> _logger;
       private readonly ApplicationDbContext _dbContext;
        public VillaController(ILogger<VillaController> logger, ApplicationDbContext db)
        {
            _logger=logger;
            _dbContext=db;
        }
        [HttpGet]
        public ActionResult<IEnumerable<VillaDto>> GetVillas()
        {
            _logger.LogInformation("Obtener las villas");
              //return Ok(VillasStore.villaList); 
              return Ok(_dbContext.Villas.ToList()); 
        }
        [HttpGet("id:int",Name ="GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDto> GetVilla(int id)
        {
            if(id==0)
            {
                _logger.LogError("Error con la villa de id "+ id);
                return BadRequest();
            }
            
            var villa=_dbContext.Villas.FirstOrDefault(v=>v.Id==id);
            if(villa==null)
            return NotFound();
            return Ok(villa);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDto> CrearVilla([FromBody]VillaDto villadto)
        {
            if(!ModelState.IsValid)
            {
                
                return BadRequest(ModelState);
            }
            

            if(_dbContext.Villas.FirstOrDefault(v=>v.Nombre.ToLower()==villadto.Nombre.ToLower())!=null)
            {
            ModelState.AddModelError("NombreExiste","La villa con ese nombre ya existe!");
            return BadRequest(ModelState);
            }
            if(villadto==null)
            return BadRequest(villadto);

            if(villadto.Id>0)
            return StatusCode(StatusCodes.Status500InternalServerError);

           // villadto.Id=_dbContext.Villas.OrderByDescending(v=>v.Id).FirstOrDefault().Id+1;
           Villa model= new Villa();
           model.Nombre=villadto.Nombre;
           model.Detalle=villadto.Detalle;
           model.ImagenUrl=villadto.ImagenUrl;
           model.Ocupantes=villadto.Ocupantes;
           model.Tarifa=villadto.Tarifa;
           model.MetrosCuadrados=villadto.MetrosCuadrados;
           model.Amenidad=villadto.Amenidad;

            _dbContext.Villas.Add(model);
            _dbContext.SaveChanges();
            return CreatedAtRoute("GetVilla",new {id=villadto.Id},villadto);


        }
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteVilla(int id)
        {
            if(id==0)
            return BadRequest();
            var villa = _dbContext.Villas.FirstOrDefault(v=>v.Id==id);
            if(villa==null)
            {
                return NotFound();
            }
            _dbContext.Villas.Remove(villa);
            _dbContext.SaveChanges();
            return NoContent();
        }
         [HttpPut("id:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateVilla(int id,[FromBody]VillaDto villaDto)
        {
            if(villaDto==null || id==0)
            return BadRequest();
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            Villa model= new (){
                Id=villaDto.Id,
                Nombre=villaDto.Nombre,
                Detalle=villaDto.Detalle,
                ImagenUrl=villaDto.ImagenUrl,
                Ocupantes=villaDto.Ocupantes,
                Tarifa=villaDto.Tarifa,
                MetrosCuadrados=villaDto.MetrosCuadrados,
                Amenidad=villaDto.Amenidad,
            };
            _dbContext.Villas.Update(model);
            _dbContext.SaveChanges();
            return NoContent();
        } 
        [HttpPatch("id:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePartialVilla(int id,JsonPatchDocument<VillaDto> patchDto)
        {
            if(patchDto==null || id==0)
            return BadRequest();
            
            var villa= _dbContext.Villas.AsNoTracking().FirstOrDefault(v=>v.Id==id);
            VillaDto villaDto= new (){
                Id=villa.Id,
                Nombre=villa.Nombre,
                Detalle=villa.Detalle,
                ImagenUrl=villa.ImagenUrl,
                Ocupantes=villa.Ocupantes,
                Tarifa=villa.Tarifa,
                MetrosCuadrados=villa.MetrosCuadrados,
                Amenidad=villa.Amenidad,
            };
            if(villa==null)
            return BadRequest();
            patchDto.ApplyTo(villaDto,ModelState);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Villa model= new (){
                Id=villaDto.Id,
                Nombre=villaDto.Nombre,
                Detalle=villaDto.Detalle,
                ImagenUrl=villaDto.ImagenUrl,
                Ocupantes=villaDto.Ocupantes,
                Tarifa=villaDto.Tarifa,
                MetrosCuadrados=villaDto.MetrosCuadrados,
                Amenidad=villaDto.Amenidad,
            };

            _dbContext.Villas.Update(model);
            _dbContext.SaveChanges();
            

            return NoContent();
        } 
    }
    
}
