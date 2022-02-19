using System;
using System.IO;
using System.Security.Principal;

namespace JGeneral.IO
{
    public static class FileAccessModifier
    {
        /// <summary>
        /// Sets the ownership of the desired file to the <paramref name="iRef"/>
        /// </summary>
        /// <param name="file">The file path</param>
        /// <param name="iRef">User identity ref, if you're trying to take ownership as your account, leave it at null.</param>
        public static bool TakeOwnershipAs(this string file, IdentityReference iRef = null)
        {
            try
            {
                new FileInfo(file).GetAccessControl().SetOwner(iRef ??= WindowsIdentity.GetCurrent().Owner);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}