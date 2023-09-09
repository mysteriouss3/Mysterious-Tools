using System;

namespace MysteriousTools.Providers
{
    public class WindowsVersionDetector
    {
        public static string GetWindowsVersion()
        {
            Version osVersion = Environment.OSVersion.Version;
            string windowsVersion = "";

            switch (osVersion.Major)
            {
                case 10 when osVersion.Build >= 22000:
                    windowsVersion = "Windows 11";
                    break; 
                case 10:
                    windowsVersion = "Windows 10";
                    break;
                case 6:
                    switch (osVersion.Minor)
                    {
                        case 3:
                            windowsVersion = "Windows 8.1";
                            break;
                        case 2:
                            windowsVersion = "Windows 8";
                            break;
                        case 1:
                            windowsVersion = "Windows 7";
                            break;
                        case 0:
                            windowsVersion = "Windows Vista";
                            break;
                    }
                    break;
                case 5:
                    switch (osVersion.Minor)
                    {
                        case 1:
                            windowsVersion = "Windows XP";
                            break;
                        case 0:
                            windowsVersion = "Windows 2000";
                            break;
                    }
                    break;
                default:
                    windowsVersion = "Bilinmeyen Windows sürümü.";
                    break;
            }

            return windowsVersion;
        }
    }
}
