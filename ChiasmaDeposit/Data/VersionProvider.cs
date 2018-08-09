using System.Reflection;

namespace ChiasmaDeposit.Data
{
    class VersionProvider
    {
        public string GetApplicationVersion()
        {
            return $"v{Assembly.GetExecutingAssembly().GetName().Version.Major}." +
                   $"{Assembly.GetExecutingAssembly().GetName().Version.Minor}." +
                   $"{Assembly.GetExecutingAssembly().GetName().Version.Build}";
        }
    }
}