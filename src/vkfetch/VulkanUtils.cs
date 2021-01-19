using System;
using System.Runtime.InteropServices;
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

        public unsafe static void CreateInstance(Vk vk, InstanceCreateInfo instanceCreateInfo, out Instance instance)
        {
            fixed (Instance* _instance = &instance)
            {
                var result = vk.CreateInstance(&instanceCreateInfo, null, _instance);

                if (result != Result.Success)
                {
                    Console.WriteLine("Failed to create vulkan instance");
                    return;
                }
            }
        }
    }
}