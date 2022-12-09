using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ownerPostApi.Controllers
{
    [Route("api/[controller]")]
    public class ownerController : ControllerBase
    {
        public ownerController(AppDb db)
        {
            Db = db;
        }

        // GET api/owner
        [HttpGet]
        public async Task<IActionResult> GetLatest()
        {
            await Db.Connection.OpenAsync();
            var query = new ownerPostQuery(Db);
            var result = await query.LatestPostsAsync();
            return new OkObjectResult(result);
        }

        // GET api/owner/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new ownerPostQuery(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }

        // POST api/owner
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]ownerPost body)
        {
            await Db.Connection.OpenAsync();
            body.Db = Db;
            await body.InsertAsync();
            return new OkObjectResult(body);
        }

        // PUT api/owner/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOne(int id, [FromBody]ownerPost body)
        {
            await Db.Connection.OpenAsync();
            var query = new ownerPostQuery(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            result.LastName = body.LastName;
            result.DriversLicence = body.DriversLicence;
            await result.UpdateAsync();
            return new OkObjectResult(result);
        }

        // DELETE api/owner/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new ownerPostQuery(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            await result.DeleteAsync();
            return new OkResult();
        }

        // DELETE api/owner
        [HttpDelete]
        public async Task<IActionResult> DeleteAll()
        {
            await Db.Connection.OpenAsync();
            var query = new ownerPostQuery(Db);
            await query.DeleteAllAsync();
            return new OkResult();
        }

        public AppDb Db { get; }
    }
}