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
using System.Net.Http;
using System.Diagnostics;
using System.Globalization;

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


        [Route("GetAllEventShadowEvents")]
        public IActionResult GetAllEventShadowEvents()
        {
            return new ObjectResult(_context.Events.AsNoTracking());
        }

        [Route( "GetDevicesByEventShadowDeviceID/{eventshadowdeviceid}" )]
        public IActionResult GetDevicesByEventShadowDeviceID( int eventshadowdeviceid )
        {
            return new ObjectResult( _context.Devices.Where( d => d.SonarDeviceId == eventshadowdeviceid ).OrderBy( d => d.TimeStamp ).AsNoTracking() );
        }

        [Route( "GetDevicesByEvent/{eventId}" )]
        public IActionResult GetDevicesByEvent( int eventId )
        {
            // need to get all the devices that are at the particular event
            return new ObjectResult( _context.Devices.Where( d => d.EventId == eventId ).OrderBy( d => d.TimeStamp ).OrderBy( d => d.EventId ).AsNoTracking() );
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

                        // need to make sure that the file was successfully copied to the drive. Once that is verified
                        // need to open the file and insert the records into the database
                        ProcessFile(fullPath);
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                return Ok();
            }
            catch ( Exception ex )
            {
                return StatusCode( 500 , ex.Message );
            }
        }

        private void ProcessFile(string fullPath)
        {
            char[] charSeparators = new char[] { ',' };
            try
            {
                // check to make sure the file exists on the drive
                if (System.IO.File.Exists(fullPath))
                {
                    // need to open the file and read the contents line by line and insert them into the database
                    string[] readText = System.IO.File.ReadAllLines(fullPath);
                    try
                    {
                        foreach (string s in readText)
                        {
                            Devices newDevice = new Devices();
                            string[] values = s.Split(charSeparators);

                            Debug.WriteLine("*************************************************************************************************************");
                            Debug.WriteLine("*************************************************************************************************************");
                            Debug.WriteLine(fullPath);
                            Debug.WriteLine($"Bluetooth Address: {values[0]}  Length: {values[0].Length}");
                            Debug.WriteLine($"TimeStamp: {DateTimeOffset.Parse(values[1]).DateTime}  Length: {DateTimeOffset.Parse(values[1]).DateTime.ToString().Length}");
                            Debug.WriteLine($"Advertisement: {values[2]}  Length: {values[2].Length}");
                            Debug.WriteLine($"RSSI: {values[3]}  Length: {values[3].Length}");
                            Debug.WriteLine($"Local Name: {values[4]}  Length: {values[4].Length}");
                            Debug.WriteLine($"ManufacturerDataString: {values[5]}  Length: {values[5].Length}");
                            Debug.WriteLine($"SonarDeviceID: {values[6]}  Length: {values[6].Length}");
                            Debug.WriteLine($"EventID: {values[7]}  Length:  {values[7].Length}");
                            Debug.WriteLine($"TimeStampDate: {Convert.ToDateTime(values[8]).ToShortDateString()}  Length: {Convert.ToDateTime(values[8]).ToShortDateString().Length}");
                            Debug.WriteLine($"TimeStampHour: {values[9]}  Length: {values[9].Length}");
                            Debug.WriteLine("*************************************************************************************************************");
                            Debug.WriteLine("*************************************************************************************************************");

                            newDevice.BluetoothAddress = values[0];
                            newDevice.TimeStamp = DateTimeOffset.Parse(values[1]).DateTime;
                            newDevice.Advertisement = values[2];
                            newDevice.Rssi = values[3];
                            newDevice.LocalName = values[4];
                            newDevice.ManufacturerDataString = values[5];
                            newDevice.SonarDeviceId = long.Parse(values[6]);
                            newDevice.EventId = long.Parse(values[7]);
                            newDevice.TimeStampDate = Convert.ToDateTime(values[8]).ToShortDateString();
                            newDevice.TimeStampHour = values[9];
                            _context.Devices.Add(newDevice);
                        }
                        try
                        {
                            
                            _context.SaveChanges();
                            DeleteFile(fullPath);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                            Debug.WriteLine(ex.InnerException);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                        Debug.WriteLine(ex.InnerException);
                    }
                }
                else
                {
                    // do nothing since the file does not exist
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                Debug.WriteLine("All the files have been transferred and saved to database");
            }
        }

        private void DeleteFile(string path)
        {

            try
            {
                System.IO.File.Delete(path);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error from DeleteFile");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.InnerException);
            }
        }
    }
}