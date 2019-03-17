using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using ODataEfCoreWebApp.Models;

namespace ODataEfCoreWebApp.Controllers
{
    [ODataRoutePrefix("Person")]
    public class PersonController : ODataController
    {
        public PersonController(SampleODataDbContext sampleODataDbContext)
        {
            SampleODataDbContext = 
                sampleODataDbContext ?? 
                throw new ArgumentNullException(nameof(sampleODataDbContext));

            SampleODataDbContext.Database.EnsureCreated();
        }

        public SampleODataDbContext SampleODataDbContext { get; }

        [ODataRoute]
        [EnableQuery(MaxTop=100, PageSize = 20, AllowedQueryOptions = AllowedQueryOptions.All)]
        public IActionResult Get()
        {
            var queryable = SampleODataDbContext.Persons.AsQueryable();
            var items = queryable.ToList();

            return Ok(queryable);
        }

        [ODataRoute("({key})")]
        [EnableQuery(PageSize = 20, AllowedQueryOptions = AllowedQueryOptions.All)]
        public IActionResult Get([FromODataUri] int key)
        {
            var result = SampleODataDbContext.Persons.Find(key);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        //[EnableQuery(PageSize = 20, AllowedQueryOptions = AllowedQueryOptions.All)]
        //[ODataRoute("Default.MyFirstFunction")]
        //[HttpGet]
        //public IActionResult MyFirstFunction()
        //{
        //    return Ok(SampleODataDbContext.Persons.Where(t => t.Name.StartsWith("K")));
        //}
    }
}