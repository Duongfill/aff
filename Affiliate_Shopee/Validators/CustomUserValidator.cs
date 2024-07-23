using Microsoft.AspNetCore.Identity;

namespace Affiliate_Shopee.Validators
{
    public class CustomUserValidator<TUser> : UserValidator<TUser> where TUser : class
    {
        public override async Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user)
        {
            var result = await base.ValidateAsync(manager, user);
            var errors = result.Succeeded ? new List<IdentityError>() : result.Errors.ToList();
            var userName = await manager.GetUserNameAsync(user);
            if (string.IsNullOrWhiteSpace(userName))
            {
                errors.Add(new IdentityError { Description = "Username không thể để trống." });
            }
            if (userName.Any(c => char.IsControl(c)))
            {
                errors.Add(new IdentityError
                {
                    Code = "InvalidUserNamea",
                    Description = "Username không được chứa ký tự điều khiển."
                });
            }

            return errors.Count == 0 ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray());
        }
    }
}
