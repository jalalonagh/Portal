using System.ComponentModel.DataAnnotations;

namespace JO.Response.Base
{
    public enum ApiResultStatusCode
    {
        [Display(Name = "عملیات با موفقیت انجام شد")]
        Success = 200,

        [Display(Name = "خطایی در سرور رخ داده است")]
        ServerError = 500,

        [Display(Name = "پارامتر های ارسالی معتبر نیستند")]
        BadRequest = 400,

        [Display(Name = "یافت نشد")]
        NotFound = 404,

        [Display(Name = "لیست خالی است")]
        ListEmpty = 204,

        [Display(Name = "خطایی در پردازش رخ داد")]
        LogicError = 409,

        [Display(Name = "خطای احراز هویت")]
        UnAuthorized = 401,

        [Display(Name = "خطای دسترسی")]
        Forbidden = 403
    }
}
