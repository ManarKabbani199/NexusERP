using Microsoft.AspNet.Identity.EntityFramework;
using NexusERP.Infrastructure.Identity;



namespace NexusERP.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
    }
}

//الكلاس يرث حقول الحساب الأساسية مثل اسم المستخدم، البريد الإلكتروني، وكلمة المرور المشفّرة.