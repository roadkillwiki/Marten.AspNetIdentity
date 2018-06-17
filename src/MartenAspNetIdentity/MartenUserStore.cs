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

		public async Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken)
		{
			return user.Id;
		}

		public async Task<string> GetUserNameAsync(TUser user, CancellationToken cancellationToken)
		{
			return user.UserName;
		}

		public async Task SetUserNameAsync(TUser user, string userName, CancellationToken cancellationToken)
		{
			user.UserName = userName;
		}

		public async Task<string> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken)
		{
			return user.NormalizedUserName;
		}

		public async Task SetNormalizedUserNameAsync(TUser user, string normalizedName, CancellationToken cancellationToken)
		{
			user.NormalizedUserName = normalizedName;
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

		public async Task SetPasswordHashAsync(TUser user, string passwordHash, CancellationToken cancellationToken)
		{
			user.PasswordHash = passwordHash;
		}

		public async Task<string> GetPasswordHashAsync(TUser user, CancellationToken cancellationToken)
		{
			return user.PasswordHash;
		}

		public async Task<bool> HasPasswordAsync(TUser user, CancellationToken cancellationToken)
		{
			bool hasPassword = !string.IsNullOrEmpty(user.PasswordHash);
			return hasPassword;
		}

		// IUserEmailStore

		public async Task SetEmailAsync(TUser user, string email, CancellationToken cancellationToken)
		{
			user.Email = email;
		}

		public async Task<string> GetEmailAsync(TUser user, CancellationToken cancellationToken)
		{
			return user.Email;
		}

		public async Task<bool> GetEmailConfirmedAsync(TUser user, CancellationToken cancellationToken)
		{
			return user.EmailConfirmed;
		}

		public async Task SetEmailConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken)
		{
			user.EmailConfirmed = confirmed;
		}

		public async Task<TUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
		{
			using (IDocumentSession session = _documentStore.LightweightSession())
			{
				return await session.Query<TUser>().FirstOrDefaultAsync(x => x.NormalizedEmail == normalizedEmail, cancellationToken);
			}
		}

		public async Task<string> GetNormalizedEmailAsync(TUser user, CancellationToken cancellationToken)
		{
			return user.NormalizedEmail;
		}

		public async Task SetNormalizedEmailAsync(TUser user, string normalizedEmail, CancellationToken cancellationToken)
		{
			user.NormalizedEmail = normalizedEmail;
		}

		// IUserPhoneNumberStore

		public async Task SetPhoneNumberAsync(TUser user, string phoneNumber, CancellationToken cancellationToken)
		{
			user.PhoneNumber = phoneNumber;
		}

		public async Task<string> GetPhoneNumberAsync(TUser user, CancellationToken cancellationToken)
		{
			return user.PhoneNumber;
		}

		public async Task<bool> GetPhoneNumberConfirmedAsync(TUser user, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public async Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken)
		{
			user.PhoneNumberConfirmed = confirmed;
		}

		// IUserTwoFactorStore

		public async Task SetTwoFactorEnabledAsync(TUser user, bool enabled, CancellationToken cancellationToken)
		{
			user.TwoFactorEnabled = enabled;
		}

		public async Task<bool> GetTwoFactorEnabledAsync(TUser user, CancellationToken cancellationToken)
		{
			return user.TwoFactorEnabled;
		}

		// IUserAuthenticatorKeyStore

		public async Task SetAuthenticatorKeyAsync(TUser user, string key, CancellationToken cancellationToken)
		{
		}

		public async Task<string> GetAuthenticatorKeyAsync(TUser user, CancellationToken cancellationToken)
		{
			return "12345123451234512345";
		}

		// IUserTwoFactorRecoveryCodeStore

		public async Task ReplaceCodesAsync(TUser user, IEnumerable<string> recoveryCodes, CancellationToken cancellationToken)
		{
		}

		public async Task<bool> RedeemCodeAsync(TUser user, string code, CancellationToken cancellationToken)
		{
			return true;
		}

		public async Task<int> CountCodesAsync(TUser user, CancellationToken cancellationToken)
		{
			return 5;
		}
	}
}