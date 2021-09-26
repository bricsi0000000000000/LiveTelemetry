using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace DataLayer
{
    public static class SessionManager
    {
        /// <summary>
        /// Returns the active session Id.
        /// If there isn't any, it returns -1.
        /// </summary>
        public static Session ActiveSession
        {
            get
            {
                try
                {
                    DatabaseContext database = new DatabaseContext();
                    database.Session.Load();

                    Session liveSession = database.Session.ToList().Find(x => x.IsLive);
                    return liveSession;
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }
        }

        public static List<Session> AllSessions
        {
            get
            {
                try
                {
                    DatabaseContext database = new DatabaseContext();
                    database.Session.Load();
                    return database.Session.ToList();
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }
        }

        public static List<string> GetActiveSessionSensors
        {
            get
            {
                try
                {
                    if (ActiveSession != null)
                    {
                        DatabaseContext database = new DatabaseContext();
                        database.SensorValue.Load();
                        database.Sensor.Load();

                        List<int> sensorIds = database.SensorValue.ToList().FindAll(x => x.SessionId == ActiveSession.SessionId).Select(x => x.SensorId).ToList();
                        List<string> sensorNames = database.Sensor.ToList().FindAll(x => sensorIds.Contains(x.SensorId)).Select(x => x.Name).ToList();

                        return sensorNames.ToList();
                    }
                    else
                    {
                        return default;
                    }
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }
        }

        /// <summary>
        /// Changes the session's state with <paramref name="sessionId"/> to <paramref name="isLive"/>.
        /// </summary>
        public static HttpStatusCode ChangeActiveSession(int sessionId, bool isLive)
        {
            try
            {
                DatabaseContext database = new DatabaseContext();
                database.Session.Load();

                if (isLive)
                {
                    if (database.Session.Where(x => x.IsLive).Any())
                    {
                        return HttpStatusCode.Conflict;
                    }
                }

                database.Session.ToList().Find(x => x.SessionId == sessionId).IsLive = isLive;
                database.SaveChanges();

                return HttpStatusCode.OK;
            }
            catch (Exception)
            {
                return HttpStatusCode.InternalServerError;
            }
        }

        public static HttpStatusCode AddSession(Session session)
        {
            try
            {
                DatabaseContext database = new DatabaseContext();
                database.Session.Load();

                if (database.Session.Where(x => x.Name.Equals(session.Name)).Any())
                {
                    return HttpStatusCode.Conflict;
                }

                Session newSession = new Session()
                {
                    Name = session.Name,
                    Date = session.Date
                };

                database.Session.Add(newSession);
                database.SaveChanges();

                database.Session.Load();

                ChangeActiveSession(newSession.SessionId, isLive: false); //newSession.SessionId will be the last inserted id

                return HttpStatusCode.OK;
            }
            catch (Exception)
            {
                return HttpStatusCode.InternalServerError;
            }
        }

        public static HttpStatusCode ChangeName(Session session)
        {
            try
            {
                DatabaseContext database = new DatabaseContext();
                database.Session.Load();

                database.Session.ToList().Find(x => x.SessionId == session.SessionId).Name = session.Name;

                database.SaveChanges();

                return HttpStatusCode.OK;
            }
            catch (Exception)
            {
                return HttpStatusCode.InternalServerError;
            }
        }

        public static HttpStatusCode Delete(int sessionId)
        {
            try
            {
                DatabaseContext database = new DatabaseContext();
                database.Session.Load();

                Session session = database.Session.ToList().Find(x => x.SessionId == sessionId);
                if (session != null)
                {
                    database.Session.Remove(session);
                    database.SaveChanges();
                }

                return HttpStatusCode.OK;
            }
            catch (Exception)
            {
                return HttpStatusCode.InternalServerError;
            }
        }
    }
}
