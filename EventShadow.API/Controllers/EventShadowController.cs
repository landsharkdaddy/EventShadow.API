using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EventShadow.API.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.AspNetCore.Hosting.Internal;
using System.Net.Http.Headers;

namespace EventShadow.API.Controllers
{
    [Produces( "application/json" )]
    [Route( "api/EventShadow" )]
    [ApiController]
    public class EventShadowController : Controller
    {
        private EventShadowContext _context;
        private readonly List<string> colors = new List<string>();

        public EventShadowController( EventShadowContext context )
        {
            _context = context;
        }

        [Route( "GetEventShadowDevices" )]
        public IActionResult GetEventShadowDevices()
        {
            return new ObjectResult( _context.EventShadowDevices );
        }

        [Route( "GetEventShadowEvents/{eventId}" )]
        public IActionResult GetEventShadowEvents( int eventId )
        {
            return new ObjectResult( _context.Events.Where( e => e.Id == eventId ).OrderBy( e => e.StartDate ).AsNoTracking() );
        }

        [Route( "GetDevicesByEventShadowDeviceID/{eventshadowdeviceid}" )]
        public IActionResult GetDevicesByEventShadowDeviceID( int eventshadowdeviceid )
        {
            return new ObjectResult( _context.Devices.Where( d => d.SonarDeviceId == eventshadowdeviceid ).OrderBy( d => d.Timestamp ).AsNoTracking() );
        }

        [Route( "GetDevicesByEvent/{eventId}" )]
        public IActionResult GetDevicesByEvent( int eventId )
        {
            // need to get all the devices that are at the particular event
            return new ObjectResult( _context.Devices.Where( d => d.EventId == eventId ).OrderBy( d => d.Timestamp ).OrderBy( d => d.EventId ).AsNoTracking() );
        }

        [HttpPost]
        [Route("UploadFiles")]
        public IActionResult UploadFile()
        {
            try
            {
                var files = Request.Form.Files;
                var folderName = Path.Combine( "Resources" , "ScanLogs" );
                var pathToSave = Path.Combine( Directory.GetCurrentDirectory() , folderName );

                foreach ( var file in files )
                {
                    if ( file.Length > 0 )
                    {
                        var fileName = ContentDispositionHeaderValue.Parse( file.ContentDisposition ).FileName.Trim( '"' );
                        var fullPath = Path.Combine( pathToSave , fileName );
                        var dbPath = Path.Combine( folderName , fileName );

                        using ( var stream = new FileStream( fullPath , FileMode.Create ) )
                        {
                            file.CopyTo( stream );
                        }
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                return Ok();
                //return Ok( new { dbPath } );
            }
            catch ( Exception ex )
            {
                return StatusCode( 500 , "Internal server error" );
            }
        }
    }
}