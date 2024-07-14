using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers {
    [Route("/api/v1/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase {
        [HttpGet]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas() {
            return Ok(VillaStore.villaList);
        }

        [HttpGet("GetVilla/{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<VillaDTO> GetVillaById(int id) {
            if (id == 0) {
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            if (villa == null) {
                return NotFound();
            }
            return Ok(villa); 
        }

        [HttpPost]      
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<VillaDTO> CreateVilla([FromBody] VillaDTO villa) {
            /*            if (!ModelState.IsValid) {
                            return BadRequest(ModelState);
                        }*/
            if (villa == null) {
                return BadRequest(villa);
            }

            var nameAlreadyTaken = VillaStore.villaList.FirstOrDefault(v => v.Name.ToLower() == villa.Name.ToLower());
            if ( nameAlreadyTaken != null) {
                ModelState.AddModelError("CustomError", "Villa already exists");
                return BadRequest(ModelState);
            }
            else {
                villa.Id = VillaStore.villaList.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;
                VillaStore.villaList.Add(villa);
                return CreatedAtRoute("GetVilla", new { id = villa.Id }, villa);
            }
        }

        [HttpPut("UpdateVilla/{id:int}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDTO> UpdateVilla([FromBody] VillaDTO villa, int id) {
            if (villa == null) {
                return BadRequest(villa);
            }
            if (id == 0) {
                return BadRequest(id);
            }

            var villaToUpdate = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            if (villaToUpdate == null) {
                return NotFound();
            }
            else {
                villaToUpdate.Name = villa.Name;
                villaToUpdate.Occupancy = villa.Occupancy;
                villaToUpdate.Sqft = villa.Sqft;
                return CreatedAtRoute("GetVilla", new { id = villaToUpdate.Id }, villa);
            }
        }



        [HttpDelete("Delete/{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteVilla(int id) {
            if (id == 0) {
                return BadRequest(id);
            }

            var villaDelete = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            if (villaDelete == null) {
                return NotFound();
            }
            VillaStore.villaList.Remove(villaDelete);
            return NoContent();
        }
    }
}