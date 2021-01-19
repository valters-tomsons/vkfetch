using System;
using System.Linq;
using System.Runtime.InteropServices;
using Silk.NET.Vulkan;

namespace vkfetch
{
    internal static class Program
    {
        private static Device _device;
        private static readonly Vk _vk = Vk.GetApi();

        internal unsafe static void Main(string[] args)
        {
            VulkanUtils.CreateVkInfo("vkfetch", out var appInfo, out var instanceInfo);
            VulkanUtils.CreateInstance(_vk, instanceInfo, out var instance);

            var devices = _vk.GetPhysicalDevices(instance);

            if(devices.Count == 0)
            {
                Console.WriteLine("Failed to find Vulkan 1.2 capable device");
                return;
            }

            var physicalDevice = devices.ElementAt(0);

            var physicalProps = new PhysicalDeviceProperties2();
            var driverProps = new PhysicalDeviceDriverProperties();

            physicalProps.PNext = &driverProps;
            driverProps.SType = StructureType.PhysicalDeviceDriverProperties;

            _vk.GetPhysicalDeviceProperties2(physicalDevice, &physicalProps);

            var deviceInfo = new DeviceCreateInfo() {
                SType = StructureType.DeviceCreateInfo,
            };

            fixed (Device* device = &_device)
            {
                if (_vk.CreateDevice(physicalDevice, &deviceInfo, null, device) != Result.Success)
                {
                    Console.WriteLine("Failed to create vulkan device");
                    return;
                }
            }

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
