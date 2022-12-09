using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace vehiclePostApi.Controllers
{
    [Route("api/[controller]")]
    public class vehicleController : ControllerBase
    {
        public vehicleController(AppDb db)
        {
            Db = db;
        }

        // GET api/vehicle
        [HttpGet]
        public async Task<IActionResult> GetLatest()
        {
            await Db.Connection.OpenAsync();
            var query = new vehiclePostQuery(Db);
            var result = await query.LatestPostsAsync();
            return new OkObjectResult(result);
        }

        // GET api/vehicle/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new vehiclePostQuery(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }

        // POST api/vehicle
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] vehiclePost body)
        {
            await Db.Connection.OpenAsync();
            body.Db = Db;
            await body.InsertAsync();
            return new OkObjectResult(body);
        }

        // PUT api/vehicle/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOne(int id, [FromBody] vehiclePost body)
        {
            await Db.Connection.OpenAsync();
            var query = new vehiclePostQuery(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            result.Brand = body.Brand;
            result.Vin = body.Vin;
            await result.UpdateAsync();
            return new OkObjectResult(result);
        }

        // DELETE api/vehicle/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new vehiclePostQuery(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            await result.DeleteAsync();
            return new OkResult();
        }

        // DELETE api/vehicle
        [HttpDelete]
        public async Task<IActionResult> DeleteAll()
        {
            await Db.Connection.OpenAsync();
            var query = new vehiclePostQuery(Db);
            await query.DeleteAllAsync();
            return new OkResult();
        }

        public AppDb Db { get; }
    }
}