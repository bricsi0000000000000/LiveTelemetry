using DataLayer.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;

namespace DataLayer
{
    public static class PackageManager
    {
        public static List<SensorValue> GetPackageSensorValues(int packageId, int sessionId)
        {
            try
            {
                DatabaseContext database = new DatabaseContext();
                database.SensorValue.Load();

                List<SensorValue> sensorValues = database.SensorValue.Where(x => x.PackageId == packageId && x.SessionId == sessionId).ToList();
              
                return sensorValues;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public static List<SensorValue> GetPackagesSensorValues(int lastSentPackageId, int sessionId)
        {
            try
            {
                DatabaseContext database = new DatabaseContext();
                database.SensorValue.Load();

                IEnumerable<SensorValue> sessionSensorValues = database.SensorValue.Where(x => x.SessionId == sessionId);
                List<SensorValue> sensorValues = new List<SensorValue>();

                foreach (SensorValue value in sessionSensorValues)
                {
                    if (value.PackageId > lastSentPackageId)
                    {
                        sensorValues.Add(value);
                    }
                }

                return sensorValues;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public static List<SensorValue> GetAllPackagesSensorValues(int sessionId)
        {
            return GetPackagesSensorValues(0, sessionId); // 0 because the first id is 1
        }
    }
}
