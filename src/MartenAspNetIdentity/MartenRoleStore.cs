using System;
using System.Threading;
using System.Threading.Tasks;
using Marten;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace MartenAspNetIdentity
{
	public class MartenRoleStore<TRole> : IRoleStore<TRole> where TRole : IdentityRole
	{
		private readonly IDocumentStore _documentStore;
		private readonly ILogger _logger;

		public MartenRoleStore(IDocumentStore documentStore, ILogger<MartenRoleStore<TRole>> logger)
		{
			_documentStore = documentStore;
			_logger = logger;
		}

		public void Dispose()
		{
		}

		public async Task<IdentityResult> CreateAsync(TRole role, CancellationToken cancellationToken)
		{
			try
			{
				using (IDocumentSession session = _documentStore.OpenSession())
				{
					session.Store(role);
					await session.SaveChangesAsync(cancellationToken);

					return IdentityResult.Success;
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to create the role in Marten.");
				return IdentityResult.Failed(new IdentityError() { Description = "Something went wrong saving the role." });
			}
		}

		public async Task<IdentityResult> UpdateAsync(TRole role, CancellationToken cancellationToken)
		{
			try
			{
				using (IDocumentSession session = _documentStore.OpenSession())
				{
					session.Update(role);
					await session.SaveChangesAsync(cancellationToken);

					return IdentityResult.Success;
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to update the role in Marten.");
				return IdentityResult.Failed(new IdentityError() { Description = "Something went wrong saving the role." });
			}
		}

		public async Task<IdentityResult> DeleteAsync(TRole role, CancellationToken cancellationToken)
		{
			try
			{
				using (IDocumentSession session = _documentStore.OpenSession())
				{
					session.Delete(role);
					await session.SaveChangesAsync(cancellationToken);

					return IdentityResult.Success;
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to delete the role in Marten.");
				return IdentityResult.Failed(new IdentityError() { Description = "Something went wrong deleting the role." });
			}
		}

		public async Task<string> GetRoleIdAsync(TRole role, CancellationToken cancellationToken)
		{
			using (IDocumentSession session = _documentStore.LightweightSession())
			{
				var actualRole = await session.Query<TRole>().FirstOrDefaultAsync(x => x.Id == role.Id, cancellationToken);
				return actualRole?.Id;
			}
		}

		public async Task<string> GetRoleNameAsync(TRole role, CancellationToken cancellationToken)
		{
			using (IDocumentSession session = _documentStore.LightweightSession())
			{
				var actualRole = await session.Query<TRole>().FirstOrDefaultAsync(x => x.Id == role.Id, cancellationToken);
				if (actualRole != null)
				{
					return actualRole.Name;
				}

				return null;
			}
		}

		public async Task SetRoleNameAsync(TRole role, string roleName, CancellationToken cancellationToken)
		{
			role.Name = roleName;
		}

		public Task<string> GetNormalizedRoleNameAsync(TRole role, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task SetNormalizedRoleNameAsync(TRole role, string normalizedName, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task<TRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task<TRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}