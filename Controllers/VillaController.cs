using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using PruebaApi.Datos;
using PruebaApi.Models;
using PruebaApi.Models.Dto;
using AutoMapper;

namespace PruebaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController:ControllerBase
    {
       private readonly ILogger<VillaController> _logger;
       private readonly ApplicationDbContext _dbContext;
       private readonly IMapper _mapper;
        public VillaController(ILogger<VillaController> logger, ApplicationDbContext db,IMapper mapper)
        {
            _logger=logger;
            _dbContext=db;
            _mapper=mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VillaDto>>> GetVillas()
        {
            _logger.LogInformation("Obtener las villas");
            IEnumerable<Villa> villaList=await _dbContext.Villas.ToListAsync();
              //return Ok(VillasStore.villaList); 
              return Ok(_mapper.Map<IEnumerable<VillaDto>>(villaList)); 
        }
        [HttpGet("id:int",Name ="GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDto>> GetVilla(int id)
        {
            if(id==0)
            {
                _logger.LogError("Error con la villa de id "+ id);
                return BadRequest();
            }
            
            var villa=await _dbContext.Villas.FirstOrDefaultAsync(v=>v.Id==id);
            if(villa==null)
            return NotFound();
            return Ok(_mapper.Map<VillaDto>(villa));
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VillaDto>> CrearVilla([FromBody]VillaDtoCreate villadtoCreate)
        {
            if(!ModelState.IsValid)
            {
                
                return BadRequest(ModelState);
            }
            

            if(await _dbContext.Villas.FirstOrDefaultAsync(v=>v.Nombre.ToLower()==villadtoCreate.Nombre.ToLower())!=null)
            {
            ModelState.AddModelError("NombreExiste","La villa con ese nombre ya existe!");
            return BadRequest(ModelState);
            }
            if(villadtoCreate==null)
            return BadRequest(villadtoCreate);

            //if(villadto.Id>0)
            //return StatusCode(StatusCodes.Status500InternalServerError);

           // villadto.Id=_dbContext.Villas.OrderByDescending(v=>v.Id).FirstOrDefault().Id+1;
           //Villa model= new Villa();
           //model.Nombre=villadto.Nombre;
           //model.Detalle=villadto.Detalle;
           //model.ImagenUrl=villadto.ImagenUrl;
           //model.Ocupantes=villadto.Ocupantes;
           //model.Tarifa=villadto.Tarifa;
           //model.MetrosCuadrados=villadto.MetrosCuadrados;
           //model.Amenidad=villadto.Amenidad;
            Villa model= _mapper.Map<Villa>(villadtoCreate);

            await _dbContext.Villas.AddAsync(model);
            await _dbContext.SaveChangesAsync();
            return CreatedAtRoute("GetVilla",new {id=model.Id},model);


        }
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            if(id==0)
            return BadRequest();
            var villa =await  _dbContext.Villas.FirstOrDefaultAsync(v=>v.Id==id);
            if(villa==null)
            {
                return NotFound();
            }
           _dbContext.Villas.Remove(villa);
           await _dbContext.SaveChangesAsync();
            return NoContent();
        }
         [HttpPut("id:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateVilla(int id,[FromBody]VillaDtoUpdate villaDto)
        {
            if(villaDto==null || id==0)
            return BadRequest();
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            // Villa model= new (){
            //     Id=villaDto.Id,
            //     Nombre=villaDto.Nombre,
            //     Detalle=villaDto.Detalle,
            //     ImagenUrl=villaDto.ImagenUrl,
            //     Ocupantes=villaDto.Ocupantes,
            //     Tarifa=villaDto.Tarifa,
            //     MetrosCuadrados=villaDto.MetrosCuadrados,
            //     Amenidad=villaDto.Amenidad,
            // };
            Villa model =_mapper.Map<Villa>(villaDto);
            _dbContext.Villas.Update(model);
           await _dbContext.SaveChangesAsync();
            return NoContent();
        } 
        [HttpPatch("id:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialVilla(int id,JsonPatchDocument<VillaDtoUpdate> patchDto)
        {
            if(patchDto==null || id==0)
            return BadRequest();
            
            var villa=await _dbContext.Villas.AsNoTracking().FirstOrDefaultAsync(v=>v.Id==id);
            // VillaDtoUpdate villaDto= new (){
            //     Id=villa.Id,
            //     Nombre=villa.Nombre,
            //     Detalle=villa.Detalle,
            //     ImagenUrl=villa.ImagenUrl,
            //     Ocupantes=villa.Ocupantes,
            //     Tarifa=villa.Tarifa,
            //     MetrosCuadrados=villa.MetrosCuadrados,
            //     Amenidad=villa.Amenidad,
            // };

            VillaDtoUpdate villaDto=_mapper.Map<VillaDtoUpdate>(villa);

            if(villa==null)
            return BadRequest();
            patchDto.ApplyTo(villaDto,ModelState);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Villa model= new (){
            //     Id=villaDto.Id,
            //     Nombre=villaDto.Nombre,
            //     Detalle=villaDto.Detalle,
            //     ImagenUrl=villaDto.ImagenUrl,
            //     Ocupantes=villaDto.Ocupantes,
            //     Tarifa=villaDto.Tarifa,
            //     MetrosCuadrados=villaDto.MetrosCuadrados,
            //     Amenidad=villaDto.Amenidad,
            // };
            Villa model= _mapper.Map<Villa>(villaDto);
            _dbContext.Villas.Update(model);
           await _dbContext.SaveChangesAsync();
            

            return NoContent();
        } 
    }
    
}
