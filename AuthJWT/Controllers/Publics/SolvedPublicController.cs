﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Threading.Tasks;
using AuthJWT.Models;
using AuthJWT.Options;
using AuthJWT.Services.PublicPins;
using DTOs.DTOs;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AuthJWT.Controllers
{
    [Route("api/[controller]/[action]")]
    [EnableCors(CorsOrigins.FrontPolicy)]
    [ApiController]

    public class SolvedPublicController : ControllerBase
    {
        private readonly ILogger<SolvedPublicController> logger;
        private readonly SolvedPinService solvedPinService;
        public SolvedPublicController(ILogger<SolvedPublicController> logger, SolvedPinService solvedPinService)
        {
            this.logger = logger;
            this.solvedPinService = solvedPinService;

            logger.LogDebug(1, "NLog injected into SolvedPublicController");
        }

        [HttpGet]
        public async Task<IActionResult> GetSolvedMapPins()
        {
            try
            {
                List<SolvedPin> solvedPins = await solvedPinService.GetSolvedPins();
                return Ok(new { solvedPins, status = true});
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, new ResponseDTO()
                {
                    Message = "На данный момент на стороне сервера ошибка, пожалуйста повторите попытку позже",
                    Status = false
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSolvedMapPinById(Guid id)
        {
            try
            {
                SolvedPin solvedPin = await solvedPinService.GetSolvedPinById(id);
                if (solvedPin == null)
                {
                    logger.LogInformation($"GetSolvedMapPinById dont found Id: {id}");
                    return NotFound(new ResponseDTO() { Message = "Пин не найден", Status = false });
                }
                return Ok(new { solvedPin, status = false });
            }
            catch (ObjectNotFoundException ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(404, new ResponseDTO() { Message = "Пин не найден", Status = false });
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, new ResponseDTO()
                {
                    Message = "На данный момент на стороне сервера ошибка, пожалуйста повторите попытку позже",
                    Status = false
                });
            }
        }

    }
}