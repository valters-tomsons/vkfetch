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

            var physicalDevice = devices.ElementAt(0);

            VulkanUtils.GetVkPhysicalDeviceProperties2(_vk, physicalDevice, out var physicalProps, out var driverProps);
            VulkanUtils.CreateVkDevice(_vk, physicalDevice, out var _);

            Console.WriteLine();

            var driverId = driverProps.DriverID.ToString().Replace("DriverID", string.Empty);
            Console.WriteLine($"DriverId: {driverId}");

            var ver = driverProps.ConformanceVersion;
            var conformanceVersion = $"{ver.Major}.{ver.Minor}.{ver.Patch}.{ver.Subminor}";
            Console.WriteLine($"Conformance Version: {conformanceVersion}");

            var str = Marshal.PtrToStringUTF8((IntPtr)driverProps.DriverInfo);
            Console.WriteLine($"Driver Info: {str}");

            var str1 = Marshal.PtrToStringUTF8((IntPtr)driverProps.DriverName);
            Console.WriteLine($"Driver Name: {str1}");

            var deviceName = Marshal.PtrToStringUTF8((IntPtr)physicalProps.Properties.DeviceName);
            Console.WriteLine($"Device Name: {deviceName}");

            var vendorId = physicalProps.Properties.VendorID.ToString("x0");
            Console.WriteLine($"VendorId: 0x{vendorId}");
        }
    }
}
