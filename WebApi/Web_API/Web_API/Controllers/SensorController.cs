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
        /// <summary>
        /// Get all sensor
        /// </summary>
        [HttpGet]
        [Route("get_all")]
        public IEnumerable<Sensor> GetAll()
        {
            return SensorManager.GetAllSensors();
        }

        /// <summary>
        /// Get sensor names for <paramref name="sessionId"/>
        /// </summary>
        [HttpGet]
        [Route("get_sensor_names")]
        public List<string> GetSensorNames(int sessionId)
        {
            return SensorManager.GetSensorNames(sessionId);
        }

        /// <summary>
        /// Post a new sensor
        /// </summary>
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
