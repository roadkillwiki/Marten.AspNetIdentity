using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Marten;
using Microsoft.AspNetCore.Identity;

namespace MartenAspNetIdentity
{
	public class MartenUserStore<TUser> : IUserStore<TUser>,
										  IUserPasswordStore<TUser>,
										  IUserEmailStore<TUser>,
										  IUserPhoneNumberStore<TUser>,
										  IUserTwoFactorStore<TUser>,
										  IUserAuthenticatorKeyStore<TUser>,
										  IUserTwoFactorRecoveryCodeStore<TUser>
										where TUser : IdentityUser
	{
		private readonly IDocumentStore _documentStore;

		public MartenUserStore(IDocumentStore documentStore)
		{
			_documentStore = documentStore;
		}

		public Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken)
		{
			return Task.FromResult(user.Id);
		}

		public Task<string> GetUserNameAsync(TUser user, CancellationToken cancellationToken)
		{
			return Task.FromResult(user.UserName);
		}

		public Task SetUserNameAsync(TUser user, string userName, CancellationToken cancellationToken)
		{
			user.UserName = userName;
			return Task.CompletedTask;
		}

		public Task<string> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken)
		{
			return Task.FromResult(user.NormalizedUserName);
		}

		public Task SetNormalizedUserNameAsync(TUser user, string normalizedName, CancellationToken cancellationToken)
		{
			user.NormalizedUserName = normalizedName;
			return Task.CompletedTask;
		}

		public async Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken)
		{
			try
			{
				using (IDocumentSession session = _documentStore.OpenSession())
				{
					session.Store(user);
					await session.SaveChangesAsync(cancellationToken);
					return IdentityResult.Success;
				}
			}
			catch (Exception ex)
			{
				return IdentityResult.Failed(new IdentityError() { Description = "Failed to create the user in Marten. " + ex.Message });
			}
		}

		public async Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken)
		{
			try
			{
				using (IDocumentSession session = _documentStore.OpenSession())
				{
					session.Update(user);
					await session.SaveChangesAsync(cancellationToken);
					return IdentityResult.Success;
				}
			}
			catch (Exception ex)
			{
				return IdentityResult.Failed(new IdentityError() { Description = "Failed to update the user in Marten." });
			}
		}

		public async Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken)
		{
			try
			{
				using (IDocumentSession session = _documentStore.OpenSession())
				{
					session.Delete(user);
					await session.SaveChangesAsync(cancellationToken);
					return IdentityResult.Success;
				}
			}
			catch (Exception ex)
			{
				return IdentityResult.Failed(new IdentityError() { Description = "Failed to delete the user in Marten." });
			}
		}

		public async Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
		{
			using (IDocumentSession session = _documentStore.LightweightSession())
			{
				return await session.Query<TUser>().FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
			}
		}

		public async Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
		{
			using (IDocumentSession session = _documentStore.LightweightSession())
			{
				return await session.Query<TUser>().FirstOrDefaultAsync(x => x.NormalizedUserName == normalizedUserName, cancellationToken);
			}
		}

		public void Dispose()
		{
		}

		// IUserPasswordStore

		public Task SetPasswordHashAsync(TUser user, string passwordHash, CancellationToken cancellationToken)
		{
			user.PasswordHash = passwordHash;
			return Task.CompletedTask;
		}

		public Task<string> GetPasswordHashAsync(TUser user, CancellationToken cancellationToken)
		{
			return Task.FromResult<string>(user.PasswordHash);
		}

		public Task<bool> HasPasswordAsync(TUser user, CancellationToken cancellationToken)
		{
			bool hasPassword = !string.IsNullOrEmpty(user.PasswordHash);
			return Task.FromResult<bool>(hasPassword);
		}

		// IUserEmailStore

		public Task SetEmailAsync(TUser user, string email, CancellationToken cancellationToken)
		{
			user.Email = email;
			return Task.CompletedTask;
		}

		public Task<string> GetEmailAsync(TUser user, CancellationToken cancellationToken)
		{
			return Task.FromResult(user.Email);
		}

		public Task<bool> GetEmailConfirmedAsync(TUser user, CancellationToken cancellationToken)
		{
			return Task.FromResult(user.EmailConfirmed);
		}

		public Task SetEmailConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken)
		{
			user.EmailConfirmed = confirmed;
			return Task.CompletedTask;
		}

		public async Task<TUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
		{
			using (IDocumentSession session = _documentStore.LightweightSession())
			{
				return await session.Query<TUser>().FirstOrDefaultAsync(x => x.NormalizedEmail == normalizedEmail, cancellationToken);
			}
		}

		public Task<string> GetNormalizedEmailAsync(TUser user, CancellationToken cancellationToken)
		{
			return Task.FromResult(user.NormalizedEmail);
		}

		public Task SetNormalizedEmailAsync(TUser user, string normalizedEmail, CancellationToken cancellationToken)
		{
			user.NormalizedEmail = normalizedEmail;
			return Task.CompletedTask;
		}

		// IUserPhoneNumberStore

		public Task SetPhoneNumberAsync(TUser user, string phoneNumber, CancellationToken cancellationToken)
		{
			user.PhoneNumber = phoneNumber;
			return Task.CompletedTask;
		}

		public Task<string> GetPhoneNumberAsync(TUser user, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task<bool> GetPhoneNumberConfirmedAsync(TUser user, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}

		// IUserTwoFactorStore

		public Task SetTwoFactorEnabledAsync(TUser user, bool enabled, CancellationToken cancellationToken)
		{
			user.TwoFactorEnabled = enabled;
			return Task.CompletedTask;
		}

		public Task<bool> GetTwoFactorEnabledAsync(TUser user, CancellationToken cancellationToken)
		{
			return Task.FromResult(user.TwoFactorEnabled);
		}

		// IUserAuthenticatorKeyStore

		public Task SetAuthenticatorKeyAsync(TUser user, string key, CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}

		public Task<string> GetAuthenticatorKeyAsync(TUser user, CancellationToken cancellationToken)
		{
			return Task.FromResult("12345123451234512345");
		}

		// IUserTwoFactorRecoveryCodeStore

		public Task ReplaceCodesAsync(TUser user, IEnumerable<string> recoveryCodes, CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}

		public Task<bool> RedeemCodeAsync(TUser user, string code, CancellationToken cancellationToken)
		{
			return Task.FromResult(true);
		}

		public Task<int> CountCodesAsync(TUser user, CancellationToken cancellationToken)
		{
			return Task.FromResult(5);
		}
	}
}