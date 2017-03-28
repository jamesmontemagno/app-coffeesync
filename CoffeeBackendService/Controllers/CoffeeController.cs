using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using CoffeeBackendService.DataObjects;
using CoffeeBackendService.Models;

namespace CoffeeBackendService.Controllers
{
    public class CoffeeController : TableController<Coffee>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            CoffeeBackendContext context = new CoffeeBackendContext();
            DomainManager = new EntityDomainManager<Coffee>(context, Request);
        }

        // GET tables/Coffee
        public IQueryable<Coffee> GetAllCoffees()
        {
            return Query();
        }

        // GET tables/Coffee/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Coffee> GetCoffee(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Coffee/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Coffee> PatchCoffee(string id, Delta<Coffee> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/Coffee
        public async Task<IHttpActionResult> PostCoffee(Coffee item)
        {
            Coffee current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Coffee/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteCoffee(string id)
        {
            return DeleteAsync(id);
        }
    }
}