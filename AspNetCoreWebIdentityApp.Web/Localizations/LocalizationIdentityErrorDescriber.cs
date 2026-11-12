using Microsoft.AspNetCore.Identity;

namespace AspNetCoreWebIdentityApp.Web.Localizations
{
    public class LocalizationIdentityErrorDescriber : IdentityErrorDescriber
    {

        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateUserName),
                Description = $"Bu kullanıcı adı zaten kullanılıyor: {userName}"
            };
        }
        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateEmail),
                Description = $"Bu e-posta zaten kullanılıyor: {email}"
            };
        }
        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError
            {
                Code = nameof(PasswordTooShort),
                Description = $"Şifre en az 6 karakter uzunluğunda olmalıdır."
            };   
        }
    }
}
