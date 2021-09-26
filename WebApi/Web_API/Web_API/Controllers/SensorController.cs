using DataLayer;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorController : ControllerBase
    {
        [HttpGet]
        [Route("get_all")]
        public IEnumerable<Sensor> GetAll()
        {
            return SensorManager.GetAllSensors();
        }

        [HttpPost]
        public int Post([FromQuery] string sensorName)
        {
            int sensorId;
            try
            {
                Sensor sensor = new()
                {
                    Name = sensorName
                };

                sensorId = SensorManager.AddSensor(sensor);
            }
            catch (Exception)
            {
                sensorId = -1;
            }

            return sensorId;
        }
    }
}
