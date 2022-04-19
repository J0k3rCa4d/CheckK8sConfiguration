using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ApiOnK8s.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConfigurationController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ConfigurationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("heartbeat")]
        public string Heartbeat() => DateTime.Now.ToString(CultureInfo.InvariantCulture);
        
        [HttpGet("{key}")]
        public IActionResult Get(string key)
        {
            var value = _configuration.GetValue<object>(key);

            dynamic result = new ExpandoObject();
            result.key = key;
            result.value = value;
            
            return Ok(result);
        }
        
        public IActionResult GetAll()
        {
            var configuration = (IConfigurationRoot) _configuration;
            var configurationList = configuration.AsEnumerable().ToList();

            return Ok(JsonSerializer.Serialize(configurationList));
        }
    }
}