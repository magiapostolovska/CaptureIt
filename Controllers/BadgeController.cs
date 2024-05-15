﻿using CaptureIt.Repos;
using CaptureIt.Models;
using Microsoft.AspNetCore.Mvc;
using CaptureIt.DTOs.Bagde;
using CaptureIt.Services;

namespace CaptureIt.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class BadgeController : ControllerBase
    {
        private readonly IBadgeService _badgeService;
    
        private readonly ILogger<AlbumController> _logger;

        public BadgeController(IBadgeService badgeService, ILogger<AlbumController> logger)
        {
            _badgeService = badgeService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BadgeResponse>>> Get()
        {
            var badges = await _badgeService.GetAll();
            if (badges == null)
            {
                _logger.LogInformation("No badges found.");
                return NotFound("No badges found.");
            }
            return Ok(badges);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BadgeResponse>> Get(int id)
        {
            var badge = await _badgeService.GetById(id);
            if (badge == null)
            {
                _logger.LogError($"Badge with id {id} not found.");
                return NotFound($"Badge with id {id} not found.");
            }
            return Ok(badge);
        }

        [HttpPost]
        public async Task<ActionResult<BadgeResponse>> Post(BadgeRequest badgeRequest)
        {
           
            var badgeResponse = await _badgeService.Add(badgeRequest);
            if (badgeResponse == null)
            {
                _logger.LogError($"Failed to add badge.");
                return StatusCode(500, $"Failed to add badge.");
            }
            return CreatedAtAction(nameof(Get), new { id = badgeResponse.BadgeId }, badgeResponse);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, BadgeUpdate badgeUpdate)
        {
           

            var result = await _badgeService.Update(id, badgeUpdate);

            if (result == null)
            {
                _logger.LogError($"Failed to update badge with ID {id}.");
                return StatusCode(500, $"Failed to update badge with ID {id}.");
            }

            return Ok(result);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            bool deleted = await _badgeService.Delete(id);
            if (!deleted)
            {
                _logger.LogError($"Badge with ID {id} not found.");
                return NotFound($"Badge with ID {id} not found.");
            }

            return Ok($"Badge with ID {id} successfully deleted.");
        }

    }
}