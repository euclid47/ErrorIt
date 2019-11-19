using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrorIt.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ErrorIt.Api.Controllers
{
    public class ApplicationController : Controller
    {
		private readonly ILogger<ApplicationController> _logger;
		private readonly IApplicationGroupRepository _groupRepository;
		private readonly IApplicationRepository _applicationRepository;

		public ApplicationController(ILogger<ApplicationController> logger, IApplicationGroupRepository groupRepository, IApplicationRepository applicationRepository)
		{
			_logger = logger;
			_groupRepository = groupRepository;
			_applicationRepository = applicationRepository;
		}

		[HttpGet]
		[Route("/Group/{applicationGroupId}/Application")]
		public async Task<IActionResult> Get(int applicationGroupId)
		{
			if (!await _groupRepository.Exists(applicationGroupId))
				return NotFound();

			return Json(await _applicationRepository.Get(applicationGroupId));
		}

		[HttpGet]
		[Route("/Group/{applicationGroupId}/Application/{id}")]
		public async Task<IActionResult> Get(int applicationGroupId, int id)
		{
			if (!await _applicationRepository.Exists(applicationGroupId, id))
				return NotFound();

			return Json(await _applicationRepository.Get(applicationGroupId, id));
		}

		[HttpPost]
		[Route("/Group/{applicationGroupId}/Application/Create")]
		public async Task<IActionResult> Create(int applicationGroupId, string name, string description)
		{
			try
			{
				return Json(await _applicationRepository.Create(applicationGroupId, name, description));
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		[HttpPost]
		[Route("/Group/{applicationGroupId}/Application/{id}/Update")]
		public async Task<IActionResult> Update(int applicationGroupId, int id, string name, string description)
		{
			try
			{
				return Json(await _applicationRepository.Update(applicationGroupId, id, name, description));
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}
	}
}