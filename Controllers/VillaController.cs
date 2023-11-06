using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PruebaApi.Datos;
using PruebaApi.Models;
using PruebaApi.Models.Dto;

namespace PruebaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController:ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<VillaDto>> GetVillas()
        {
              return Ok(VillasStore.villaList); 
        }
        [HttpGet("id:int",Name ="GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDto> GetVilla(int id)
        {
            if(id==0)
            return BadRequest();
            var villa=VillasStore.villaList.FirstOrDefault(v=>v.Id==id);
            if(villa==null)
            return NotFound();
            return Ok();
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDto> CrearVilla([FromBody]VillaDto villadto)
        {
            if(!ModelState.IsValid)
            return BadRequest(ModelState);

            if(VillasStore.villaList.FirstOrDefault(v=>v.Nombre.ToLower()==villadto.Nombre.ToLower())!=null)
            {
            ModelState.AddModelError("NombreExiste","La villa con ese nombre ya existe!");
            return BadRequest(ModelState);
            }
            if(villadto==null)
            return BadRequest(villadto);

            if(villadto.Id>0)
            return StatusCode(StatusCodes.Status500InternalServerError);

            villadto.Id=VillasStore.villaList.OrderByDescending(v=>v.Id).FirstOrDefault().Id+1;
            VillasStore.villaList.Add(villadto);
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
            var villa = VillasStore.villaList.FirstOrDefault(v=>v.Id==id);
            if(villa==null)
            {
                return NotFound();
            }
            VillasStore.villaList.Remove(villa);
            return NoContent();
        }
        [HttpPatch("id:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateVilla(int id,[FromBody] VillaDto villadto)
        {
            if(villadto==null || id!=villadto.Id)
            return BadRequest();
            var villa = VillasStore.villaList.FirstOrDefault(v=>v.Id==id);
            villa.Nombre=villadto.Nombre;
            villa.Ocupantes=villadto.Ocupantes;
            villa.MetrosCuadrados=villadto.MetrosCuadrados;

            return NoContent();
        } 
    }
    
}
