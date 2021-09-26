using DataLayer;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;

namespace Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        [HttpGet]
        [Route("health_check")]
        public bool HealthCheck()
        {
            return true;
        }

        /// <returns>Returns the Id of the active session.</returns>
        [HttpGet]
        [Route("live")]
        public Session Get()
        {
            return SessionManager.ActiveSession;
        }

        [HttpGet]
        [Route("all")]
        public List<Session> GetAll()
        {
            return SessionManager.AllSessions;
        }

        [HttpGet]
        [Route("active_session_sensors")]
        public List<string> GetActiveSessionSensors()
        {
            return SessionManager.GetActiveSessionSensors;
        }

        /// <summary>
        /// Adds new session.
        /// You only have to add the name and date
        /// </summary>
        /// <param name="session">New session to add.</param>
        /// <response code="200">Sucessfully added.</response>
        /// <response code="409">There is already a session with <paramref name="session"/>s name.</response>       
        /// <response code="500">There was an error adding <paramref name="session"/>.</response>       
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public HttpStatusCode Post([FromBody] Session session)
        {
            return SessionManager.AddSession(session);
        }

        /// <summary>
        /// Changes the live session to the session with <paramref name="sessionId"/> if there no live session.
        /// </summary>
        /// <param name="sessionId">Id of the session.</param>
        /// <response code="200">Sucessfully changed.</response>
        /// <response code="409">There is already an active session.</response>       
        /// <response code="500">There was an error with the database.</response>       
        [HttpPut]
        [Route("change_live")]
        [ProducesResponseType(200)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public HttpStatusCode ChangeToLive([FromBody] int sessionId)
        {
            return SessionManager.ChangeActiveSession(sessionId, isLive: true);
        }

        /// <summary>
        /// Changes the session with <paramref name="sessionId"/> to offline.
        /// </summary>
        /// <param name="sessionId">Id of the session.</param>
        /// <response code="200">Sucessfully changed.</response>
        /// <response code="500">There was an error with the database.</response>       
        [HttpPut]
        [Route("change_offline")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public HttpStatusCode ChangeToOffline([FromBody] int sessionId)
        {
            return SessionManager.ChangeActiveSession(sessionId, isLive: false);
        }

        /// <summary>
        /// Changes the session's name.
        /// </summary>
        /// <param name="session">Session with new name.</param>
        /// <response code="200">Sucessfully changed.</response>
        /// <response code="500">There was an error with the database.</response>       
        [HttpPut]
        [Route("change_name")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public HttpStatusCode ChangeName([FromBody] Session session)
        {
            return SessionManager.ChangeName(session);
        }

        /// <summary>
        /// Deletes the session with <paramref name="sessionId"/>.
        /// </summary>
        /// <param name="sessionId">Id of the session.</param>
        /// <response code="200">Sucessfully changed.</response>
        /// <response code="500">There was an error with the database.</response>       
        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public HttpStatusCode Delete([FromQuery] int sessionId)
        {
            return SessionManager.Delete(sessionId);
        }
    }
}
