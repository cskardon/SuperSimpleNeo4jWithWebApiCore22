﻿namespace ExampleNeo4jMiddleware.Controllers
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;
    using Neo4j.Driver.V1;

    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IDriver _driver;

        public ValuesController(IDriver driver)
        {
            _driver = driver;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new[] {"Goto: api/values/<ID> tooo execute a query"};
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            using (var session = _driver.Session())
            {
                using (var tx = session.BeginTransaction())
                {
                    var results = tx.Run("MATCH (n:TestNode {id:$id}) RETURN n", new Dictionary<string, object> {{"id", id}});
                    foreach (var result in results)
                    {
                        var node = result["n"].As<INode>();
                        var returnedId = node.Properties["id"]?.As<long>();
                        return $"Got: {returnedId}";
                    }
                }
            }

            return "Errrrr";
        }

        //GET api/values/exports
        [HttpGet("exports/")]
        public ActionResult<IEnumerable<int>> NameDoesntMatter()
        {
            return new[] { 1, 2, 3 };
        }

        //GET api/values/exports/5
        [HttpGet("exports/{identifier:int}")]
        public ActionResult<int> NameReallyDoesntMatter(int identifier) //<-- except in the names, that has to match the {} bit
        {
            return identifier;
        }


        //GET exports
        [HttpGet("/exports/")]
        public ActionResult<IEnumerable<int>> Exports()
        {
            return new[] { 4,5,6 };
        }
    }
}