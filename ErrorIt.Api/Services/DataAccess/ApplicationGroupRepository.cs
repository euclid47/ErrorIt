﻿using ErrorIt.Api.Data.Models;
using ErrorIt.Api.Interfaces;
using ErrorIt.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ErrorIt.Api.Services.DataAccess
{
	public class ApplicationGroupRepository : IApplicationGroupRepository
	{
		private readonly ILogger<ApplicationGroupRepository> _logger;
		private readonly ICacher _cacher;
		private readonly AppDbContext _dbContext;

		public ApplicationGroupRepository(ILogger<ApplicationGroupRepository> logger, ICacher cacher, AppDbContext dbContext)
		{
			_logger = logger;
			_cacher = cacher;
			_dbContext = dbContext;
		}

		public async Task<List<ApplicationGroup>> Get()
		{
			try
			{
				var result = await _dbContext.ApplicationGroups.AsNoTracking().ToListAsync();

				SetCache(result);				

				return result;
			}
			catch (Exception e)
			{
				_logger.LogError($"[{System.Reflection.MethodBase.GetCurrentMethod().Name}] {e.Message ?? ""}", e);
				throw;
			}
		}

		public async Task<ApplicationGroup> Get(int id)
		{
			try
			{
				ApplicationGroup result = await GetCache(id);
				
				if(result is null)
				{
					result = await _dbContext.ApplicationGroups.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);

					if(result != null)
						SetCache(result);
				}

				return result;
			}
			catch (Exception e)
			{
				_logger.LogError($"[{System.Reflection.MethodBase.GetCurrentMethod().Name}] {e.Message ?? ""}", e);
				throw;
			}
		}

		public async Task<ApplicationGroup> Get(string name)
		{
			try
			{

				return await _dbContext.ApplicationGroups.AsNoTracking().SingleOrDefaultAsync(x => x.Name == name);
			}
			catch (Exception e)
			{
				_logger.LogError($"[{System.Reflection.MethodBase.GetCurrentMethod().Name}] {e.Message ?? ""}", e);
				throw;
			}
		}

		public async Task<ApplicationGroup> Create(string name, string description)
		{
			try
			{
				var result = new ApplicationGroup { Description = description, Name = name };

				_dbContext.ApplicationGroups.Add(result);
				await _dbContext.SaveChangesAsync();

				return result;
			}
			catch (Exception e)
			{
				_logger.LogError($"[{System.Reflection.MethodBase.GetCurrentMethod().Name}] {e.Message ?? ""}", e);
				throw;
			}
		}

		public async Task<ApplicationGroup> Update(int id, string name, string description)
		{
			try
			{
				var result = await _dbContext.ApplicationGroups.SingleOrDefaultAsync(x => x.Id == id);

				if(result.Id > 0)
				{
					result.Name = name;
					result.Description = description;

					_dbContext.ApplicationGroups.Update(result);
					await _dbContext.SaveChangesAsync();

					return result;
				}

				throw new Exception($"The group, {id}, cannot be found.");
			}
			catch (Exception e)
			{
				_logger.LogError($"[{System.Reflection.MethodBase.GetCurrentMethod().Name}] {e.Message ?? ""}", e);
				throw;
			}
		}

		public async Task<bool> Delete(int id)
		{
			try
			{
				var result = await _dbContext.ApplicationGroups.SingleOrDefaultAsync(x => x.Id == id);

				if (result is null)
					return false;

				_dbContext.ApplicationGroups.Remove(result);
				await _dbContext.SaveChangesAsync();

				return true;
			}
			catch (Exception e)
			{
				_logger.LogError($"[{System.Reflection.MethodBase.GetCurrentMethod().Name}] {e.Message ?? ""}", e);
				throw;
			}
		}

		public async Task<bool> Exists(int id)
		{
			try
			{
				var group = await Get(id);

				return group != null;
			}
			catch (Exception e)
			{
				_logger.LogError($"[{System.Reflection.MethodBase.GetCurrentMethod().Name}] {e.Message ?? ""}", e);
				throw;
			}
		}

		public Task<bool> Exists(string name)
		{
			throw new NotImplementedException();
		}

		private async Task<ApplicationGroup> GetCache(int key)
		{
			try
			{
				return await _cacher.Get<ApplicationGroup>($"ApplicationGroup:{key}");
			}
			catch (Exception e)
			{
				_logger.LogError($"[{System.Reflection.MethodBase.GetCurrentMethod().Name}] {e.Message ?? ""}", e);
				return null;
			}		
		}

		private void SetCache(IEnumerable<ApplicationGroup> applicationGroups)
		{
			Task.Factory.StartNew(() =>
			{
				try
				{
					Task.Factory.StartNew(() => {
						foreach (var group in applicationGroups)
						{
							SetCache(group);
						}
					});
				}
				catch (Exception e)
				{
					_logger.LogError($"[{System.Reflection.MethodBase.GetCurrentMethod().Name}] {e.Message ?? ""}", e);
				}
			});
		}

		private void SetCache(ApplicationGroup applicationGroup)
		{
			Task.Factory.StartNew(() => 
			{
				try
				{
					_cacher.Set($"ApplicationGroup:{applicationGroup.Id}", applicationGroup, TimeSpan.FromMinutes(DotNetEnv.Env.GetInt("cachetimeout", 10)));
				}
				catch (Exception e)
				{
					_logger.LogError($"[{System.Reflection.MethodBase.GetCurrentMethod().Name}] {e.Message ?? ""}", e);
				}
			});			
		}

		private void RemoveCache(ApplicationGroup applicationGroup)
		{
			Task.Factory.StartNew(() =>
			{
				try
				{
					_cacher.Remove($"ApplicationGroup:{applicationGroup.Id}");
				}
				catch (Exception e)
				{
					_logger.LogError($"[{System.Reflection.MethodBase.GetCurrentMethod().Name}] {e.Message ?? ""}", e);
				}
			});
		}
	}
}
