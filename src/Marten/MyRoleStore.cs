using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Marten;
using Microsoft.AspNetCore.Identity;

namespace Examples.MvcSecurity.Marten
{
	public class MartenRoleStore : IRoleStore<IdentityRole>
	{
		private readonly IDocumentStore _documentStore;

		public MartenRoleStore(IDocumentStore documentStore)
		{
			_documentStore = documentStore;
		}

		public void Dispose()
		{
		}

		public async Task<IdentityResult> CreateAsync(IdentityRole role, CancellationToken cancellationToken)
		{
			using (IDocumentSession session = _documentStore.OpenSession())
			{
				session.Update(role);
				await session.SaveChangesAsync(cancellationToken);

				return new IdentityResult();
			}
		}

		public async Task<IdentityResult> UpdateAsync(IdentityRole role, CancellationToken cancellationToken)
		{
			using (IDocumentSession session = _documentStore.OpenSession())
			{
				session.Update(role);
				await session.SaveChangesAsync(cancellationToken);

				return new IdentityResult();
			}
		}

		public async Task<IdentityResult> DeleteAsync(IdentityRole role, CancellationToken cancellationToken)
		{
			using (IDocumentSession session = _documentStore.OpenSession())
			{
				session.Delete(role);
				await session.SaveChangesAsync(cancellationToken);

				return new IdentityResult();
			}
		}

		public async Task<string> GetRoleIdAsync(IdentityRole role, CancellationToken cancellationToken)
		{
			using (IDocumentSession session = _documentStore.LightweightSession())
			{
				var actualRole = await session.Query<IdentityRole>().FirstOrDefaultAsync(x => x.Id == role.Id, cancellationToken);
				if (actualRole != null)
				{
					return actualRole.Id;
				}

				return "";
			}
		}

		public async Task<string> GetRoleNameAsync(IdentityRole role, CancellationToken cancellationToken)
		{
			using (IDocumentSession session = _documentStore.LightweightSession())
			{
				var actualRole = await session.Query<IdentityRole>().FirstOrDefaultAsync(x => x.Id == role.Id, cancellationToken);
				if (actualRole != null)
				{
					return actualRole.Name;
				}

				return "";
			}
		}

		public async Task SetRoleNameAsync(IdentityRole role, string roleName, CancellationToken cancellationToken)
		{
			using (IDocumentSession session = _documentStore.OpenSession())
			{
				role.Name = roleName;
				session.Update(role);
				await session.SaveChangesAsync(cancellationToken);
			}
		}

		public Task<string> GetNormalizedRoleNameAsync(IdentityRole role, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task SetNormalizedRoleNameAsync(IdentityRole role, string normalizedName, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task<IdentityRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task<IdentityRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}