using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using ODataEfCoreWebApp.Models;

namespace ODataEfCoreWebApp.Controllers
{
    public class PersonController : Controller
    {
        public PersonController(SampleODataDbContext sampleODataDbContext)
        {
            SampleODataDbContext = sampleODataDbContext ?? throw new ArgumentNullException(nameof(sampleODataDbContext));
        }

        public SampleODataDbContext SampleODataDbContext { get; }

        [EnableQuery]
        public IActionResult Get()
        {
            SampleODataDbContext.Database.EnsureCreated();

            var results = SampleODataDbContext.Persons.ToList();

            return Ok(SampleODataDbContext.Persons.AsQueryable());
        }

        [EnableQuery]
        public IActionResult Get(int id)
        {
            SampleODataDbContext.Database.EnsureCreated();

            var result =
                SampleODataDbContext
                    .Persons
                    .SingleOrDefault(p => p.Id == id);

            return Ok(result);
        }
    }
}