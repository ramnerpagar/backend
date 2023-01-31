﻿using Microsoft.AspNetCore.Mvc;
using Staff_Data.DataContext;
using Staff_Data.Models;
using Staff_Data.Services;
using Pump_Data.Models;
using NLog;
using ILogger = NLog.ILogger;

namespace Staff_Data.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeniorStaffController : Controller
    {
        private SeniorStaffService staffService;

        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        public SeniorStaffController(StaffDBContext dbContext)
        {
            staffService = new SeniorStaffService(dbContext);
        }

        [HttpGet]
        public IActionResult GetStaff()
        {
            try
            {
                return Ok(staffService.GetStaff());
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        public IActionResult AddStaff(SeniorStaff staff)
        {
            try
            {
                return Ok(staffService.AddStaff(staff));
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return BadRequest(ex.ToString());
            }
        }

        [HttpPut]
        public IActionResult UpdateStaff(SeniorStaff staff)
        {
            try
            {
                JsonResponse jsonResponse = new JsonResponse();
                if (staffService.UpdateStaff(staff))
                {
                    jsonResponse.Result = true;
                    jsonResponse.Message = "Updated Successfully";
                }
                else
                {
                    jsonResponse.Result = false;
                    jsonResponse.Message = "Update Failed";
                }
                return Ok(jsonResponse);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return BadRequest(ex.ToString());
            }

        }

        [HttpPost]
        [Route("retrench")]
        public IActionResult RetrenchStaff(FormIdData data)
        {
            try
            {
                JsonResponse jsonResponse = new JsonResponse();
                if (staffService.RetrenchStaff(data.tblStaffID))
                {
                    jsonResponse.Result = true;
                    jsonResponse.Message = "Retrenched staff Successfully";
                }else
                {
                    jsonResponse.Result = false;
                    jsonResponse.Message = "Retrenched Staff failed";
                }
                return Ok(jsonResponse);
            }
            catch(Exception ex)
            {
                logger.Error(ex.ToString());
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        [Route("suspend")]
        public IActionResult SuspendStaff(FormIdData data)
        {
            try
            {
                JsonResponse jsonResponse = new JsonResponse();
                if (staffService.SuspendStaff(data.tblStaffID))
                {
                    jsonResponse.Result = true;
                    jsonResponse.Message = "Suspended Staff Successfully";
                }
                else
                {
                    jsonResponse.Result = false;
                    jsonResponse.Message = "Suspend Staff failed";
                }
                return Ok(jsonResponse);
            }
            catch(Exception ex)
            {
                logger.Error(ex.ToString());
                return BadRequest(ex.ToString());
            }

        }

        [HttpPost]
        [Route("recallRetrench")]
        public IActionResult RecallRetrenchStaff(FormIdData data)
        {
            try
            {
                JsonResponse jsonResponse = new JsonResponse();
                if (staffService.RecallRetrenchStaff(data.tblStaffID))
                {
                    jsonResponse.Result = true;
                    jsonResponse.Message = "Recall Retrench Successfully";
                }else
                {
                    jsonResponse.Result = false;
                    jsonResponse.Message = "Recall Retrench failed";
                }
                return Ok(jsonResponse);
            }
            catch(Exception ex)
            {
                logger.Error(ex.ToString());
                return BadRequest(ex.ToString());
            }

        }

        [HttpPost]
        [Route("recallSuspend")]
        public IActionResult RecallSuspendStaff(FormIdData data)
        {
            try
            {
                JsonResponse jsonResponse = new JsonResponse();
                if (staffService.RecallSuspendStaff(data.tblStaffID))
                {
                    jsonResponse.Result = true;
                    jsonResponse.Message = "Recall Suspend Successfully";
                }
                else
                {
                    jsonResponse.Result = true;
                    jsonResponse.Message = "Recall Suspend failed";
                }
                return Ok(jsonResponse);
            }
            catch(Exception ex)
            {
                logger.Error(ex.ToString());
                return BadRequest(ex.ToString());
            }

        }

        [HttpGet]
        [Route("getSuspendedStaffs")]
        public IActionResult GetSuspendedStaff()
        {
            try
            {
                return Ok(staffService.GetSuspendedStaff());
            }
            catch(Exception ex)
            {
                logger.Error(ex.ToString());
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet]
        [Route("getRetrenchedStaffs")]
        public IActionResult GetRetrenchedStaff()
        {
            try
            {
                return Ok(staffService.GetRetrenchedStaff());
            }
            catch(Exception ex)
            {
                logger.Error(ex.ToString());
                return BadRequest(ex.ToString());
            }
        }
    }
}
