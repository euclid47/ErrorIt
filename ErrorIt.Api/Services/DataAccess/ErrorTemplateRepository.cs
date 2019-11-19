using ErrorIt.Api.Data.Models;
using ErrorIt.Api.Extensions;
using ErrorIt.Api.Interfaces;
using ErrorIt.Api.Models;
using ErrorIt.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ErrorIt.Api.Services.DataAccess
{
	public class ErrorTemplateRepository : IErrorTemplateRepository
	{
		private readonly ILogger<ErrorTemplateRepository> _logger;
		private readonly ICacher _cacher;
		private readonly AppDbContext _dbContext;

		public ErrorTemplateRepository(ILogger<ErrorTemplateRepository> logger, ICacher cacher, AppDbContext dbContext)
		{
			_logger = logger;
			_cacher = cacher;
			_dbContext = dbContext;
		}

		public async Task<List<ErrorTemplate>> Get(int applicationId)
		{
			try
			{
				return await _dbContext.ErrorTemplates.Where(x => x.ApplicationId == applicationId).AsNoTracking().ToListAsync();
			}
			catch (Exception e)
			{
				_logger.LogError($"[{System.Reflection.MethodBase.GetCurrentMethod().Name}] {e.Message ?? ""}", e);
				throw;
			}
		}

		public async Task<ErrorTemplate> Get(int applicationId, int id)
		{
			try
			{
				return await _dbContext.ErrorTemplates.AsNoTracking().SingleOrDefaultAsync(x => x.ApplicationId == applicationId && x.Id == id);
			}
			catch (Exception e)
			{
				_logger.LogError($"[{System.Reflection.MethodBase.GetCurrentMethod().Name}] {e.Message ?? ""}", e);
				throw;
			}
		}

		public async Task<ErrorTemplate> Get(int applicationId, string applicationErrorCode)
		{
			try
			{
				return await _dbContext.ErrorTemplates.AsNoTracking().SingleOrDefaultAsync(x => x.ApplicationId == applicationId && x.ApplicationErrorCode == applicationErrorCode);
			}
			catch (Exception e)
			{
				_logger.LogError($"[{System.Reflection.MethodBase.GetCurrentMethod().Name}] {e.Message ?? ""}", e);
				throw;
			}
		}

		public async Task<ErrorTemplate> Create(int applicationId, IErrorRestBase errorResponse)
		{
			try
			{
				var result = new ErrorTemplate { ApplicationErrorCode = errorResponse.ApplicationErrorCode, ApplicationId = applicationId, ErrorDetail = errorResponse.SerializeJson() };

				_dbContext.ErrorTemplates.Add(result);
				await _dbContext.SaveChangesAsync();

				return result;
			}
			catch (Exception e)
			{
				_logger.LogError($"[{System.Reflection.MethodBase.GetCurrentMethod().Name}] {e.Message ?? ""}", e);
				throw;
			}
		}

		public async Task<ErrorTemplate> Update(int applicationId, int id, IErrorRestBase errorResponse)
		{
			try
			{
				var result = await _dbContext.ErrorTemplates.SingleOrDefaultAsync(x => x.Id == id && x.ApplicationId == applicationId);

				result.ApplicationErrorCode = errorResponse.ApplicationErrorCode;
				result.ErrorDetail = errorResponse.SerializeJson();

				_dbContext.ErrorTemplates.Update(result);
				await _dbContext.SaveChangesAsync();

				return result;
			}
			catch (Exception e)
			{
				_logger.LogError($"[{System.Reflection.MethodBase.GetCurrentMethod().Name}] {e.Message ?? ""}", e);
				throw;
			}
		}

		public async Task<bool> Delete(int applicationId, int id)
		{
			try
			{
				var result = await _dbContext.ErrorTemplates.SingleOrDefaultAsync(x => x.Id == id && x.ApplicationId == applicationId);

				if (result is null)
					return false;

				_dbContext.ErrorTemplates.Remove(result);
				await _dbContext.SaveChangesAsync();

				return true;
			}
			catch (Exception e)
			{
				_logger.LogError($"[{System.Reflection.MethodBase.GetCurrentMethod().Name}] {e.Message ?? ""}", e);
				throw;
			}
		}

		public async Task<bool> Exists(int applicationId, int id)
		{

			try
			{
				var errorTemplate = await Get(applicationId, id);

				return errorTemplate != null;
			}
			catch (Exception e)
			{
				_logger.LogError($"[{System.Reflection.MethodBase.GetCurrentMethod().Name}] {e.Message ?? ""}", e);
				throw;
			}
		}
	}
}
