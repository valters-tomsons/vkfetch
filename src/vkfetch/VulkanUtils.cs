using System;
using System.Runtime.InteropServices;
using Silk.NET.Core;
using Silk.NET.Vulkan;
using Silk.NET.Windowing;

namespace vkfetch
{
    public unsafe static class VulkanUtils
    {
        public static void CreateInstance(Vk apiInstance, Instance vkInstance, string appName)
        {
            var appInfo = new ApplicationInfo
            {
                SType = StructureType.ApplicationInfo,
                PApplicationName = (byte*)Marshal.StringToHGlobalAnsi(appName),
                ApplicationVersion = new Version32(1, 0, 0),
                PEngineName = (byte*)Marshal.StringToHGlobalAnsi("No Engine"),
                EngineVersion = new Version32(1, 0, 0),
                ApiVersion = Vk.Version12
            };

            var instanceInfo = new InstanceCreateInfo
            {
                SType = StructureType.InstanceCreateInfo,
                PApplicationInfo = &appInfo
            };

            var result = apiInstance.CreateInstance(&instanceInfo, null, &vkInstance);

            if(result != Result.Success)
            {
                throw new NotImplementedException();
            }
        }
    }
}