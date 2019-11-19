using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrorIt.Api.Interfaces;
using ErrorIt.Api.Models;
using ErrorIt.Api.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ErrorIt.Api.Controllers
{
    public class ErrorTemplateController : Controller
    {
		private readonly ILogger<ErrorTemplateController> _logger;
		private readonly IApplicationGroupRepository _groupRepository;
		private readonly IApplicationRepository _applicationRepository;
		private readonly IErrorTemplateRepository _errorTemplateRepository;

		public ErrorTemplateController(ILogger<ErrorTemplateController> logger, IApplicationGroupRepository groupRepository, IApplicationRepository applicationRepository, IErrorTemplateRepository errorTemplateRepository)
		{
			_logger = logger;
			_groupRepository = groupRepository;
			_applicationRepository = applicationRepository;
			_errorTemplateRepository = errorTemplateRepository;
		}

		[HttpGet]
		[Route("/Group/{applicationGroupId}/Application/{applicationId}/ErrorTemplates")]
		public async Task<IActionResult> Get(int applicationGroupId, int applicationId)
		{
			try
			{
				return Json(await _errorTemplateRepository.Get(applicationId));
			}
			catch(Exception e)
			{
				_logger.LogError($"[{System.Reflection.MethodBase.GetCurrentMethod().Name}] {e.Message ?? ""}", e);
				return BadRequest(e.Message ?? "");
			}
		}

		[HttpGet]
		[Route("/Group/{applicationGroupId}/Application/{applicationId}/ErrorTemplates/{id}")]
		public async Task<IActionResult> Get(int applicationGroupId, int applicationId, int id)
		{
			try
			{
				return Json(await _errorTemplateRepository.Get(applicationId, id));
			}
			catch (Exception e)
			{
				_logger.LogError($"[{System.Reflection.MethodBase.GetCurrentMethod().Name}] {e.Message ?? ""}", e);
				return BadRequest(e.Message ?? "");
			}
		}

		[HttpGet]
		[Route("/ErrorTemplate")]
		public async Task<IActionResult> Get(ErrorRequest errorRequest)
		{
			try
			{
				var group = await _groupRepository.Get(errorRequest.ApplicationGroup);

				if(group != null)
				{
					var application = await _applicationRepository.Get(group.Id, errorRequest.ApplicationName);

					if(application != null)
					{
						return Json(await _errorTemplateRepository.Get(application.Id, errorRequest.ApplicationErrorCode));
					}
				}

				return Json(null);
			}
			catch (Exception e)
			{
				_logger.LogError($"[{System.Reflection.MethodBase.GetCurrentMethod().Name}] {e.Message ?? ""}", e);
				return BadRequest(e.Message ?? "");
			}
		}

		[HttpPost]
		[Route("/Group/{applicationGroupId}/Application/{applicationId}/ErrorTemplates/Create")]
		public async Task<IActionResult> Create(int applicationGroupId, int applicationId, [FromBody] ErrorPost errorCreate)
		{
			try
			{
				
				return Json(await _errorTemplateRepository.Create(applicationId, errorCreate));
			}
			catch (Exception e)
			{
				_logger.LogError($"[{System.Reflection.MethodBase.GetCurrentMethod().Name}] {e.Message ?? ""}", e);
				return BadRequest(e.Message ?? "");
			}
		}

		[HttpPost]
		[Route("/Group/{applicationGroupId}/Application/{applicationId}/ErrorTemplates/{id}/Update")]
		public async Task<IActionResult> Update(int applicationGroupId, int applicationId, int id, [FromBody] ErrorPost errorUpdate)
		{
			try
			{
				return Json(await _errorTemplateRepository.Update(applicationId, id, errorUpdate));
			}
			catch (Exception e)
			{
				_logger.LogError($"[{System.Reflection.MethodBase.GetCurrentMethod().Name}] {e.Message ?? ""}", e);
				return BadRequest(e.Message ?? "");
			}
		}
	}
}