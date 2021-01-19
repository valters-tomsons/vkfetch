using System.Runtime.InteropServices;
using Silk.NET.Core;
using Silk.NET.Vulkan;

namespace vkfetch
{
    public static class VulkanUtils
    {
        public unsafe static void CreateVkInfo(string appName, out ApplicationInfo applicationInfo, out InstanceCreateInfo instanceCreateInfo)
        {
            applicationInfo = new ApplicationInfo
            {
                SType = StructureType.ApplicationInfo,
                PApplicationName = (byte*)Marshal.StringToHGlobalAnsi(appName),
                ApplicationVersion = new Version32(1, 0, 0),
                PEngineName = (byte*)Marshal.StringToHGlobalAnsi("No Engine"),
                EngineVersion = new Version32(1, 0, 0),
                ApiVersion = Vk.Version12
            };

            fixed (ApplicationInfo* appInfo = &applicationInfo)
            {
                instanceCreateInfo = new InstanceCreateInfo
                {
                    SType = StructureType.InstanceCreateInfo,
                    PApplicationInfo = appInfo
                };
            }
        }
    }
}