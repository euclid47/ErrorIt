using ErrorIt.Api.Data.Models;
using ErrorIt.Api.Interfaces;
using ErrorIt.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ErrorIt.Api.Services.DataAccess
{
	public class ApplicationRepository : IApplicationRepository
	{
		private readonly ILogger<ApplicationRepository> _logger;
		private readonly ICacher _cacher;
		private readonly AppDbContext _dbContext;

		public ApplicationRepository(ILogger<ApplicationRepository> logger, ICacher cacher, AppDbContext dbContext)
		{
			_logger = logger;
			_cacher = cacher;
			_dbContext = dbContext;
		}

		public async Task<List<Application>> Get(int applicationGroupId)
		{
			try
			{
				return await _dbContext.Applications.Where(x => x.ApplicationGroupId == applicationGroupId).AsNoTracking().ToListAsync();
			}
			catch (Exception e)
			{
				_logger.LogError($"[{System.Reflection.MethodBase.GetCurrentMethod().Name}] {e.Message ?? ""}", e);
				throw;
			}
		}

		public async Task<Application> Get(int applicationGroupId, int id)
		{
			try
			{
				//var result = await GetCache(id);

				return await _dbContext.Applications.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
			}
			catch (Exception e)
			{
				_logger.LogError($"[{System.Reflection.MethodBase.GetCurrentMethod().Name}] {e.Message ?? ""}", e);
				throw;
			}
		}

		public async Task<Application> Get(int applicationGroupId, string name)
		{
			try
			{
				return await _dbContext.Applications.AsNoTracking().SingleOrDefaultAsync(x => x.Name == name);
			}
			catch (Exception e)
			{
				_logger.LogError($"[{System.Reflection.MethodBase.GetCurrentMethod().Name}] {e.Message ?? ""}", e);
				throw;
			}
		}

		public async Task<Application> Create(int applicationGroupId, string name, string description)
		{
			try
			{
				var result = new Application { ApplicationGroupId = applicationGroupId, Description = description, Name = name };

				_dbContext.Applications.Add(result);
				await _dbContext.SaveChangesAsync();

				return result;
			}
			catch (Exception e)
			{
				_logger.LogError($"[{System.Reflection.MethodBase.GetCurrentMethod().Name}] {e.Message ?? ""}", e);
				throw;
			}
		}

		public async Task<Application> Update(int applicationGroupId, int id, string name, string description)
		{
			try
			{
				var result = await _dbContext.Applications.SingleOrDefaultAsync(x => x.Id == id && x.ApplicationGroupId == applicationGroupId);

				result.Name = name;
				result.Description = description;

				_dbContext.Applications.Update(result);
				await _dbContext.SaveChangesAsync();

				return result;
			}
			catch (Exception e)
			{
				_logger.LogError($"[{System.Reflection.MethodBase.GetCurrentMethod().Name}] {e.Message ?? ""}", e);
				throw;
			}
		}

		public async Task<bool> Delete(int applicationGroupId, int id)
		{
			try
			{
				var result = await _dbContext.Applications.SingleOrDefaultAsync(x => x.Id == id && x.ApplicationGroupId == applicationGroupId);

				if (result is null)
					return false;

				_dbContext.Applications.Remove(result);
				await _dbContext.SaveChangesAsync();

				return true;
			}
			catch (Exception e)
			{
				_logger.LogError($"[{System.Reflection.MethodBase.GetCurrentMethod().Name}] {e.Message ?? ""}", e);
				throw;
			}
		}

		public async Task<bool> Exists(int applicationGroupId, int id)
		{
			try
			{
				var application = await Get(applicationGroupId, id);

				return application != null;
			}
			catch (Exception e)
			{
				_logger.LogError($"[{System.Reflection.MethodBase.GetCurrentMethod().Name}] {e.Message ?? ""}", e);
				throw;
			}
		}
	}
}
