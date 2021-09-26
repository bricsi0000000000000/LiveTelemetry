﻿using DataLayer;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;

namespace Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageController : ControllerBase
    {
        [HttpGet]
        [Route("get_single/{packageId}/{sessionId}")]
        public List<SensorValue> Get(int packageId, int sessionId)
        {
            return PackageManager.GetPackageSensorValues(packageId, sessionId);
        }

        /// <returns>All package, that is after <paramref name="lastPackageId"/>.</returns>
        [HttpGet]
        [Route("get_after/{lastPackageId}/{sessionId}")]
        public IEnumerable<SensorValue> GetAfter(int lastPackageId, int sessionId)
        {
            return PackageManager.GetPackagesSensorValues(lastPackageId, sessionId);
        }

        [HttpGet]
        [Route("get_all/{sessionId}")]
        public IEnumerable<SensorValue> GetAll(int sessionId)
        {
            return PackageManager.GetAllPackagesSensorValues(sessionId);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public HttpStatusCode Post([FromQuery] string packageJson)
        {
            try
            {
                dynamic json = JsonConvert.DeserializeObject(packageJson);

                for (int i = 0; i < json.sensor_values.Count; i++)
                {
                    SensorValue sensorValue = new()
                    {
                        SensorId = json.sensor_values[i].sensor_id,
                        SessionId = json.sensor_values[i].session_id,
                        Value = json.sensor_values[i].value,
                        PackageId = json.package_id
                    };

                    SensorManager.AddSensorValue(sensorValue);
                }
            }
            catch (Exception e)
            {
                return HttpStatusCode.InternalServerError;
            }

            return HttpStatusCode.OK;
        }
    }
}
