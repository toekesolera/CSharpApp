using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace claimPostApi.Controllers
{
    [Route("api/[controller]")]
    public class claimController : ControllerBase
    {
        public claimController(AppDb db)
        {
            Db = db;
        }

        // GET api/claim
        [HttpGet]
        public async Task<IActionResult> GetLatest()
        {
            await Db.Connection.OpenAsync();
            var query = new claimPostQuery(Db);
            var result = await query.LatestPostsAsync();
            return new OkObjectResult(result);
        }

        // GET api/claim/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new claimPostQuery(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }

        // POST api/claim
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] claimPost body)
        {
            await Db.Connection.OpenAsync();
            body.Db = Db;
            await body.InsertAsync();
            return new OkObjectResult(body);
        }

        // PUT api/claim/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOne(int id, [FromBody] claimPost body)
        {
            await Db.Connection.OpenAsync();
            var query = new claimPostQuery(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            result.Description = body.Description;
            result.Status = body.Status;
            await result.UpdateAsync();
            return new OkObjectResult(result);
        }

        // DELETE api/claim/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new claimPostQuery(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            await result.DeleteAsync();
            return new OkResult();
        }

        // DELETE api/claim
        [HttpDelete]
        public async Task<IActionResult> DeleteAll()
        {
            await Db.Connection.OpenAsync();
            var query = new claimPostQuery(Db);
            await query.DeleteAllAsync();
            return new OkResult();
        }

        public AppDb Db { get; }
    }
}