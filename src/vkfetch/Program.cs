using System;
using System.Linq;
using System.Runtime.InteropServices;
using Silk.NET.Vulkan;

namespace vkfetch
{
    internal static class Program
    {
        private static readonly Vk _vk = Vk.GetApi();

        internal unsafe static void Main(string[] args)
        {
            VulkanUtils.CreateVkInfo("vkfetch", out var _, out var instanceInfo);
            VulkanUtils.CreateVkInstance(_vk, instanceInfo, out var instance);

            var devices = _vk.GetPhysicalDevices(instance);

            if(devices.Count == 0)
            {
                Console.WriteLine("Failed to find Vulkan 1.2 capable device");
                return;
            }

            for(var i = 0; i < devices.Count; i++)
            {
                var dev = devices.ElementAt(i);
                PrintDevice(dev);
            }
        }

        private unsafe static void PrintDevice(PhysicalDevice physicalDevice)
        {
            VulkanUtils.GetVkPhysicalDeviceProperties2(_vk, physicalDevice, out var physicalProps, out var driverProps);
            VulkanUtils.CreateVkDevice(_vk, physicalDevice, out var _);

            Console.WriteLine();

            var str1 = Marshal.PtrToStringUTF8((IntPtr)driverProps.DriverName);
            Console.WriteLine($"Driver Name: {str1}");

            var driverId = driverProps.DriverID.ToString().Replace("DriverID", string.Empty);
            Console.WriteLine($"DriverId: {driverId}");

            var deviceName = Marshal.PtrToStringUTF8((IntPtr)physicalProps.Properties.DeviceName);
            Console.WriteLine($"Device Name: {deviceName}");

            var str = Marshal.PtrToStringUTF8((IntPtr)driverProps.DriverInfo);
            Console.WriteLine($"Driver Info: {str}");

            var ver = driverProps.ConformanceVersion;
            var conformanceVersion = $"{ver.Major}.{ver.Minor}.{ver.Patch}.{ver.Subminor}";
            Console.WriteLine($"Conformance Version: {conformanceVersion}");

            VulkanUtils.EnumerateInstanceExtensions(_vk, out var extensions);
            Console.WriteLine($"Supported extensions: {extensions.Length}");
        }
    }
}
