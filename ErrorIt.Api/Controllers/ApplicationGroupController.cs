using System;
using System.Threading.Tasks;
using ErrorIt.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ErrorIt.Api.Controllers
{
    public class ApplicationGroupController : Controller
    {
		private readonly ILogger<ApplicationGroupController> _logger;
		private readonly IApplicationGroupRepository _groupRepository;

		public ApplicationGroupController(ILogger<ApplicationGroupController> logger, IApplicationGroupRepository groupRepository)
		{
			_logger = logger;
			_groupRepository = groupRepository;
		}

		[HttpGet]
		[Route("/Group")]
		public async Task<IActionResult> Get()
		{
			try
			{
				return Json(await _groupRepository.Get());
			}
			catch(Exception e)
			{
				return BadRequest(e.Message);
			}			
		}

		[HttpGet]
		[Route("/Group/{id}")]
		public async Task<IActionResult> Get(int id)
		{
			try
			{
				if (!await _groupRepository.Exists(id))
					return NotFound();

				return Json(await _groupRepository.Get(id));
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		[HttpPost]
		[Route("/Group/Create")]
		public async Task<IActionResult> Create(string name, string description)
		{
			try
			{
				return Json(await _groupRepository.Create(name, description));
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		[HttpPost]
		[Route("/Group/{id}/Update")]
		public async Task<IActionResult> Update(int id, string name, string description)
		{
			try
			{
				return Json(await _groupRepository.Update(id, name, description));
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}
	}
}