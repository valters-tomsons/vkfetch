using System;
using System.Runtime.InteropServices;
using Silk.NET.Vulkan;

namespace vkfetch
{
    public unsafe static class VulkanUtils
    {
        public static void CreateVkInfo(string appName, out ApplicationInfo applicationInfo, out InstanceCreateInfo instanceCreateInfo)
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

        public static void CreateVkInstance(Vk vk, InstanceCreateInfo instanceCreateInfo, out Instance instance)
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

        public static void GetVkPhysicalDeviceProperties2(Vk vk, PhysicalDevice device, out PhysicalDeviceProperties2 deviceProperties, out PhysicalDeviceDriverProperties driverProperties)
        {
            driverProperties = new PhysicalDeviceDriverProperties
            {
                SType = StructureType.PhysicalDeviceDriverProperties,
            };

            fixed(PhysicalDeviceDriverProperties* props = &driverProperties)
            {
                deviceProperties = new PhysicalDeviceProperties2
                {
                    PNext = props
                };
            }

            fixed (PhysicalDeviceProperties2* props = &deviceProperties)
            {
                vk.GetPhysicalDeviceProperties2(device, props);
            }
        }

        public static void CreateVkDevice(Vk vk, PhysicalDevice physicalDevice, out Device device)
        {
            var deviceInfo = new DeviceCreateInfo() {
                SType = StructureType.DeviceCreateInfo,
            };

            fixed (Device* _device = &device)
            {
                if (vk.CreateDevice(physicalDevice, &deviceInfo, null, _device) != Result.Success)
                {
                    Console.WriteLine("Failed to create vulkan device");
                    return;
                }
            }
        }

        public static void EnumerateInstanceExtensions(Vk vk, out ExtensionProperties[] extensions)
        {
            uint extcount;
            vk.EnumerateInstanceExtensionProperties((byte*)IntPtr.Zero, &extcount, null);

            extensions = new ExtensionProperties[extcount];
            vk.EnumerateInstanceExtensionProperties((byte*)IntPtr.Zero, &extcount, extensions);
        }
    }
}