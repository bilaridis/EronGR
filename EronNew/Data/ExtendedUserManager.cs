using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using EronNew.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EronNew.Data
{

    public class ExtendedUserManager<TUser> : UserManager<ExtendedIdentityUser> where TUser : class
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<ExtendedUserManager<ExtendedIdentityUser>> _logger;
        public ExtendedUserManager(
            IUserStore<ExtendedIdentityUser> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<ExtendedIdentityUser> passwordHasher,
            IEnumerable<IUserValidator<ExtendedIdentityUser>> userValidators,
            IEnumerable<IPasswordValidator<ExtendedIdentityUser>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<ExtendedUserManager<ExtendedIdentityUser>> logger,
            IServiceScopeFactory serviceScopeFactory)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public override ILogger Logger { get => base.Logger; set => base.Logger = value; }

        public override bool SupportsUserAuthenticationTokens => base.SupportsUserAuthenticationTokens;

        public override bool SupportsUserAuthenticatorKey => base.SupportsUserAuthenticatorKey;

        public override bool SupportsUserTwoFactorRecoveryCodes => base.SupportsUserTwoFactorRecoveryCodes;

        public override bool SupportsUserTwoFactor => base.SupportsUserTwoFactor;

        public override bool SupportsUserPassword => base.SupportsUserPassword;

        public override bool SupportsUserSecurityStamp => base.SupportsUserSecurityStamp;

        public override bool SupportsUserRole => base.SupportsUserRole;

        public override bool SupportsUserLogin => base.SupportsUserLogin;

        public override bool SupportsUserEmail => base.SupportsUserEmail;

        public override bool SupportsUserPhoneNumber => base.SupportsUserPhoneNumber;

        public override bool SupportsUserClaim => base.SupportsUserClaim;

        public override bool SupportsUserLockout => base.SupportsUserLockout;

        public override bool SupportsQueryableUsers => base.SupportsQueryableUsers;

        public override IQueryable<ExtendedIdentityUser> Users => base.Users;

        protected override CancellationToken CancellationToken => base.CancellationToken;

        public override Task<IdentityResult> AccessFailedAsync(ExtendedIdentityUser user)
        {
            return base.AccessFailedAsync(user);
        }

        public override Task<IdentityResult> AddClaimAsync(ExtendedIdentityUser user, Claim claim)
        {
            return base.AddClaimAsync(user, claim);
        }

        public override Task<IdentityResult> AddClaimsAsync(ExtendedIdentityUser user, IEnumerable<Claim> claims)
        {
            return base.AddClaimsAsync(user, claims);
        }

        public override Task<IdentityResult> AddLoginAsync(ExtendedIdentityUser user, UserLoginInfo login)
        {
            return base.AddLoginAsync(user, login);
        }

        public override Task<IdentityResult> AddPasswordAsync(ExtendedIdentityUser user, string password)
        {
            return base.AddPasswordAsync(user, password);
        }

        public override Task<IdentityResult> AddToRoleAsync(ExtendedIdentityUser user, string role)
        {
            return base.AddToRoleAsync(user, role);
        }

        public override Task<IdentityResult> AddToRolesAsync(ExtendedIdentityUser user, IEnumerable<string> roles)
        {
            return base.AddToRolesAsync(user, roles);
        }

        public override Task<IdentityResult> ChangeEmailAsync(ExtendedIdentityUser user, string newEmail, string token)
        {
            return base.ChangeEmailAsync(user, newEmail, token);
        }

        public override Task<IdentityResult> ChangePasswordAsync(ExtendedIdentityUser user, string currentPassword, string newPassword)
        {
            return base.ChangePasswordAsync(user, currentPassword, newPassword);
        }

        public override Task<IdentityResult> ChangePhoneNumberAsync(ExtendedIdentityUser user, string phoneNumber, string token)
        {
            return base.ChangePhoneNumberAsync(user, phoneNumber, token);
        }

        public override Task<bool> CheckPasswordAsync(ExtendedIdentityUser user, string password)
        {
            return base.CheckPasswordAsync(user, password);
        }

        public override Task<IdentityResult> ConfirmEmailAsync(ExtendedIdentityUser user, string token)
        {
            return base.ConfirmEmailAsync(user, token);
        }

        public override Task<int> CountRecoveryCodesAsync(ExtendedIdentityUser user)
        {
            return base.CountRecoveryCodesAsync(user);
        }

        public override Task<IdentityResult> CreateAsync(ExtendedIdentityUser user)
        {
            return base.CreateAsync(user);
        }

        public override Task<IdentityResult> CreateAsync(ExtendedIdentityUser user, string password)
        {
            return base.CreateAsync(user, password);
        }

        public override Task<byte[]> CreateSecurityTokenAsync(ExtendedIdentityUser user)
        {
            return base.CreateSecurityTokenAsync(user);
        }

        public override Task<IdentityResult> DeleteAsync(ExtendedIdentityUser user)
        {
            return base.DeleteAsync(user);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override Task<ExtendedIdentityUser> FindByEmailAsync(string email)
        {
            return base.FindByEmailAsync(email);
        }

        public override Task<ExtendedIdentityUser> FindByIdAsync(string userId)
        {
            return base.FindByIdAsync(userId);
        }

        public override Task<ExtendedIdentityUser> FindByLoginAsync(string loginProvider, string providerKey)
        {
            return base.FindByLoginAsync(loginProvider, providerKey);
        }

        public override Task<ExtendedIdentityUser> FindByNameAsync(string userName)
        {
            return base.FindByNameAsync(userName);
        }

        public override Task<string> GenerateChangeEmailTokenAsync(ExtendedIdentityUser user, string newEmail)
        {
            return base.GenerateChangeEmailTokenAsync(user, newEmail);
        }

        public override Task<string> GenerateChangePhoneNumberTokenAsync(ExtendedIdentityUser user, string phoneNumber)
        {
            return base.GenerateChangePhoneNumberTokenAsync(user, phoneNumber);
        }

        public override Task<string> GenerateConcurrencyStampAsync(ExtendedIdentityUser user)
        {
            return base.GenerateConcurrencyStampAsync(user);
        }

        public override Task<string> GenerateEmailConfirmationTokenAsync(ExtendedIdentityUser user)
        {
            return base.GenerateEmailConfirmationTokenAsync(user);
        }

        public override string GenerateNewAuthenticatorKey()
        {
            return base.GenerateNewAuthenticatorKey();
        }

        public override Task<IEnumerable<string>> GenerateNewTwoFactorRecoveryCodesAsync(ExtendedIdentityUser user, int number)
        {
            return base.GenerateNewTwoFactorRecoveryCodesAsync(user, number);
        }

        public override Task<string> GeneratePasswordResetTokenAsync(ExtendedIdentityUser user)
        {
            return base.GeneratePasswordResetTokenAsync(user);
        }

        public override Task<string> GenerateTwoFactorTokenAsync(ExtendedIdentityUser user, string tokenProvider)
        {
            return base.GenerateTwoFactorTokenAsync(user, tokenProvider);
        }

        public override Task<string> GenerateUserTokenAsync(ExtendedIdentityUser user, string tokenProvider, string purpose)
        {
            return base.GenerateUserTokenAsync(user, tokenProvider, purpose);
        }

        public override Task<int> GetAccessFailedCountAsync(ExtendedIdentityUser user)
        {
            return base.GetAccessFailedCountAsync(user);
        }

        public override Task<string> GetAuthenticationTokenAsync(ExtendedIdentityUser user, string loginProvider, string tokenName)
        {
            return base.GetAuthenticationTokenAsync(user, loginProvider, tokenName);
        }

        public override Task<string> GetAuthenticatorKeyAsync(ExtendedIdentityUser user)
        {
            return base.GetAuthenticatorKeyAsync(user);
        }

        public override Task<IList<Claim>> GetClaimsAsync(ExtendedIdentityUser user)
        {
            return base.GetClaimsAsync(user);
        }

        public override Task<string> GetEmailAsync(ExtendedIdentityUser user)
        {
            return base.GetEmailAsync(user);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override Task<bool> GetLockoutEnabledAsync(ExtendedIdentityUser user)
        {
            return base.GetLockoutEnabledAsync(user);
        }

        public override Task<DateTimeOffset?> GetLockoutEndDateAsync(ExtendedIdentityUser user)
        {
            return base.GetLockoutEndDateAsync(user);
        }

        public override Task<IList<UserLoginInfo>> GetLoginsAsync(ExtendedIdentityUser user)
        {
            return base.GetLoginsAsync(user);
        }

        public override Task<string> GetPhoneNumberAsync(ExtendedIdentityUser user)
        {
            return base.GetPhoneNumberAsync(user);
        }

        public override Task<IList<string>> GetRolesAsync(ExtendedIdentityUser user)
        {
            return base.GetRolesAsync(user);
        }

        public override Task<string> GetSecurityStampAsync(ExtendedIdentityUser user)
        {
            return base.GetSecurityStampAsync(user);
        }

        public override Task<bool> GetTwoFactorEnabledAsync(ExtendedIdentityUser user)
        {
            return base.GetTwoFactorEnabledAsync(user);
        }

        public override Task<ExtendedIdentityUser> GetUserAsync(ClaimsPrincipal principal)
        {
            return base.GetUserAsync(principal);
        }

        public override string GetUserId(ClaimsPrincipal principal)
        {
            return base.GetUserId(principal);
        }

        public override Task<string> GetUserIdAsync(ExtendedIdentityUser user)
        {
            return base.GetUserIdAsync(user);
        }

        public override string GetUserName(ClaimsPrincipal principal)
        {
            return base.GetUserName(principal);
        }

        public override Task<string> GetUserNameAsync(ExtendedIdentityUser user)
        {
            return base.GetUserNameAsync(user);
        }

        public override Task<IList<ExtendedIdentityUser>> GetUsersForClaimAsync(Claim claim)
        {
            return base.GetUsersForClaimAsync(claim);
        }

        public override Task<IList<ExtendedIdentityUser>> GetUsersInRoleAsync(string roleName)
        {
            return base.GetUsersInRoleAsync(roleName);
        }

        public override Task<IList<string>> GetValidTwoFactorProvidersAsync(ExtendedIdentityUser user)
        {
            return base.GetValidTwoFactorProvidersAsync(user);
        }

        public override Task<bool> HasPasswordAsync(ExtendedIdentityUser user)
        {
            return base.HasPasswordAsync(user);
        }

        public override Task<bool> IsEmailConfirmedAsync(ExtendedIdentityUser user)
        {
            return base.IsEmailConfirmedAsync(user);
        }

        public override Task<bool> IsInRoleAsync(ExtendedIdentityUser user, string role)
        {
            return base.IsInRoleAsync(user, role);
        }

        public override Task<bool> IsLockedOutAsync(ExtendedIdentityUser user)
        {
            return base.IsLockedOutAsync(user);
        }

        public override Task<bool> IsPhoneNumberConfirmedAsync(ExtendedIdentityUser user)
        {
            return base.IsPhoneNumberConfirmedAsync(user);
        }

        public override string NormalizeEmail(string email)
        {
            return base.NormalizeEmail(email);
        }

        public override string NormalizeName(string name)
        {
            return base.NormalizeName(name);
        }

        public override Task<IdentityResult> RedeemTwoFactorRecoveryCodeAsync(ExtendedIdentityUser user, string code)
        {
            return base.RedeemTwoFactorRecoveryCodeAsync(user, code);
        }

        public override void RegisterTokenProvider(string providerName, IUserTwoFactorTokenProvider<ExtendedIdentityUser> provider)
        {
            base.RegisterTokenProvider(providerName, provider);
        }

        public override Task<IdentityResult> RemoveAuthenticationTokenAsync(ExtendedIdentityUser user, string loginProvider, string tokenName)
        {
            return base.RemoveAuthenticationTokenAsync(user, loginProvider, tokenName);
        }

        public override Task<IdentityResult> RemoveClaimAsync(ExtendedIdentityUser user, Claim claim)
        {
            return base.RemoveClaimAsync(user, claim);
        }

        public override Task<IdentityResult> RemoveClaimsAsync(ExtendedIdentityUser user, IEnumerable<Claim> claims)
        {
            return base.RemoveClaimsAsync(user, claims);
        }

        public override Task<IdentityResult> RemoveFromRoleAsync(ExtendedIdentityUser user, string role)
        {
            return base.RemoveFromRoleAsync(user, role);
        }

        public override Task<IdentityResult> RemoveFromRolesAsync(ExtendedIdentityUser user, IEnumerable<string> roles)
        {
            return base.RemoveFromRolesAsync(user, roles);
        }

        public override Task<IdentityResult> RemoveLoginAsync(ExtendedIdentityUser user, string loginProvider, string providerKey)
        {
            return base.RemoveLoginAsync(user, loginProvider, providerKey);
        }

        public override Task<IdentityResult> RemovePasswordAsync(ExtendedIdentityUser user)
        {
            return base.RemovePasswordAsync(user);
        }

        public override Task<IdentityResult> ReplaceClaimAsync(ExtendedIdentityUser user, Claim claim, Claim newClaim)
        {
            return base.ReplaceClaimAsync(user, claim, newClaim);
        }

        public override Task<IdentityResult> ResetAccessFailedCountAsync(ExtendedIdentityUser user)
        {
            return base.ResetAccessFailedCountAsync(user);
        }

        public override Task<IdentityResult> ResetAuthenticatorKeyAsync(ExtendedIdentityUser user)
        {
            return base.ResetAuthenticatorKeyAsync(user);
        }

        public override Task<IdentityResult> ResetPasswordAsync(ExtendedIdentityUser user, string token, string newPassword)
        {
            return base.ResetPasswordAsync(user, token, newPassword);
        }

        public override Task<IdentityResult> SetAuthenticationTokenAsync(ExtendedIdentityUser user, string loginProvider, string tokenName, string tokenValue)
        {
            return base.SetAuthenticationTokenAsync(user, loginProvider, tokenName, tokenValue);
        }

        public override Task<IdentityResult> SetEmailAsync(ExtendedIdentityUser user, string email)
        {
            return base.SetEmailAsync(user, email);
        }

        public override Task<IdentityResult> SetLockoutEnabledAsync(ExtendedIdentityUser user, bool enabled)
        {
            return base.SetLockoutEnabledAsync(user, enabled);
        }

        public override Task<IdentityResult> SetLockoutEndDateAsync(ExtendedIdentityUser user, DateTimeOffset? lockoutEnd)
        {
            return base.SetLockoutEndDateAsync(user, lockoutEnd);
        }

        public override Task<IdentityResult> SetPhoneNumberAsync(ExtendedIdentityUser user, string phoneNumber)
        {
            return base.SetPhoneNumberAsync(user, phoneNumber);
        }

        public override Task<IdentityResult> SetTwoFactorEnabledAsync(ExtendedIdentityUser user, bool enabled)
        {
            return base.SetTwoFactorEnabledAsync(user, enabled);
        }

        public override Task<IdentityResult> SetUserNameAsync(ExtendedIdentityUser user, string userName)
        {
            return base.SetUserNameAsync(user, userName);
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override Task<IdentityResult> UpdateAsync(ExtendedIdentityUser user)
        {
            return base.UpdateAsync(user);
        }

        public override Task UpdateNormalizedEmailAsync(ExtendedIdentityUser user)
        {
            return base.UpdateNormalizedEmailAsync(user);
        }

        public override Task UpdateNormalizedUserNameAsync(ExtendedIdentityUser user)
        {
            return base.UpdateNormalizedUserNameAsync(user);
        }

        public override Task<IdentityResult> UpdateSecurityStampAsync(ExtendedIdentityUser user)
        {
            return base.UpdateSecurityStampAsync(user);
        }

        public override Task<bool> VerifyChangePhoneNumberTokenAsync(ExtendedIdentityUser user, string token, string phoneNumber)
        {
            return base.VerifyChangePhoneNumberTokenAsync(user, token, phoneNumber);
        }

        public override Task<bool> VerifyTwoFactorTokenAsync(ExtendedIdentityUser user, string tokenProvider, string token)
        {
            return base.VerifyTwoFactorTokenAsync(user, tokenProvider, token);
        }

        public override Task<bool> VerifyUserTokenAsync(ExtendedIdentityUser user, string tokenProvider, string purpose, string token)
        {
            return base.VerifyUserTokenAsync(user, tokenProvider, purpose, token);
        }

        protected override string CreateTwoFactorRecoveryCode()
        {
            return base.CreateTwoFactorRecoveryCode();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        protected override Task<IdentityResult> UpdatePasswordHash(ExtendedIdentityUser user, string newPassword, bool validatePassword)
        {
            return base.UpdatePasswordHash(user, newPassword, validatePassword);
        }

        protected override Task<IdentityResult> UpdateUserAsync(ExtendedIdentityUser user)
        {
            return base.UpdateUserAsync(user);
        }

        protected override Task<PasswordVerificationResult> VerifyPasswordAsync(IUserPasswordStore<ExtendedIdentityUser> store, ExtendedIdentityUser user, string password)
        {
            return base.VerifyPasswordAsync(store, user, password);
        }

        public async Task<string> GetFirstName(ExtendedIdentityUser user)
        {
            var entity = await this.Users.FirstOrDefaultAsync(x => x.Id == user.Id);
            return entity.FirstName;
        }

        public async Task<string> GetLastName(ExtendedIdentityUser user)
        {

            var entity = await this.Users.FirstOrDefaultAsync(x => x.Id == user.Id);
            return entity.LastName;
        }

        public async Task<IdentityResult> SetFirstName(ExtendedIdentityUser user, string firstName)
        {

            try
            {
                //var entity = await this.Users.FirstOrDefaultAsync(x => x.Id == user.Id);
                user.FirstName = firstName;
                await Store.UpdateAsync(user, CancellationToken);

                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError() { Description = ex.Message });

            }
        }

        public async Task<IdentityResult> SetLastName(ExtendedIdentityUser user, string lastName)
        {
            var result = new IdentityResult();
            try
            {
                //var entity = await this.Users.FirstOrDefaultAsync(x => x.Id == user.Id);
                user.LastName = lastName;
                await Store.UpdateAsync(user, CancellationToken);
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError() { Description = ex.Message });

            }
        }

        public async Task<IdentityResult> SetSubscribeNewsLetter(string email, bool subscribe)
        {
            var result = new IdentityResult();
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var _ironKeyContext = scope.ServiceProvider.GetService<IronKeyContext>();
                //var entity = await this.Users.FirstOrDefaultAsync(x => x.Id == user.Id);
                var userObj = _ironKeyContext.AspNetUsers.FirstOrDefault(x => x.UserName == email);
                userObj.Newsletter = subscribe;
                await _ironKeyContext.SaveChangesAsync();
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError() { Description = ex.Message });

            }
        }

        public async Task<IdentityResult> CreateWallet(string user)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var _ironKeyContext = scope.ServiceProvider.GetService<IronKeyContext>();

                if (!_ironKeyContext.Wallets.Any(x => x.AspNetUser.Email == user))
                {
                    var userObj = _ironKeyContext.AspNetUsers.FirstOrDefault(x => x.Email == user);
                    await _ironKeyContext.Wallets.AddAsync(new Wallet()
                    {
                        AspNetUserId = userObj.Id,
                        Tokens = 0,
                        CreatedDate = DateTime.Now
                    });
                    if (!_ironKeyContext.AspNetUserProfiles.Any(x => x.AspNetUserId == userObj.Id))
                    {
                        await _ironKeyContext.AspNetUserProfiles.AddAsync(new AspNetUserProfile() { AspNetUserId = userObj.Id, Active = true });
                    }

                    await _ironKeyContext.SaveChangesAsync();

                    return IdentityResult.Success;
                }
                else
                {
                    return IdentityResult.Failed(new IdentityError() { Description = "The User has already a wallet" });
                }
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError() { Description = ex.Message });

            }

        }

        public async Task<Wallet> GetWallet(ExtendedIdentityUser user)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var _ironKeyContext = scope.ServiceProvider.GetService<IronKeyContext>();
                if (_ironKeyContext.Wallets.Any(x => x.AspNetUserId == user.Id))
                {
                    return await _ironKeyContext.Wallets.FirstOrDefaultAsync(x => x.AspNetUserId == user.Id);
                }
                else
                {
                    _logger.LogError("GetWallet : User " + user.Id + " cannot Find Wallet.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;

            }

        }
    }
}
