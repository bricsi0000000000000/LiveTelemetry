using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace DataLayer
{
    public static class SensorManager
    {
        public static List<Sensor> GetAllSensors()
        {
            try
            {
                DatabaseContext database = new DatabaseContext();
                database.Sensor.Load();

                return database.Sensor.ToList();
            }
            catch (Exception)
            {
                return new List<Sensor>();
            }
        }

        public static List<string> GetSensorNames(int sessionId)
        {
            return SessionManager.GetSessionSensors(sessionId);
        }

        public static int AddSensor(Sensor newSensor)
        {
            try
            {
                DatabaseContext database = new DatabaseContext();
                database.Sensor.Load();

                Sensor sensor = database.Sensor.ToList().Find(x => x.Name.Equals(newSensor.Name));
                if (sensor != null)
                {
                    return sensor.SensorId;
                }

                database.Sensor.Add(newSensor);

                database.SaveChanges();

                return newSensor.SensorId;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public static HttpStatusCode AddSensorValue(SensorValue value)
        {
            try
            {
                DatabaseContext database = new DatabaseContext();
                database.SensorValue.Load();

                database.SensorValue.Add(value);

                database.SaveChanges();

                return HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                return HttpStatusCode.InternalServerError;
            }
        }
    }
}
