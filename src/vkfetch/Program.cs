﻿using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Silk.NET.Core;
using Silk.NET.Core.Native;
using Silk.NET.Vulkan;
using Silk.NET.Windowing;

namespace vkfetch
{
    internal static class Program
    {
        private static Instance _instance;
        private static Device _device;

        internal unsafe static void Main(string[] args)
        {
            var appInfo = new ApplicationInfo
            {
                SType = StructureType.ApplicationInfo,
                PApplicationName = (byte*)Marshal.StringToHGlobalAnsi("vkfetch"),
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

            var vk = Vk.GetApi();

            fixed (Instance* instance = &_instance)
            {
                if (vk.CreateInstance(&instanceInfo, null, instance) != Result.Success)
                {
                    Console.WriteLine("Failed to create vulkan instance");
                    return;
                }
            }

            var devices = vk.GetPhysicalDevices(_instance);

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

            vk.GetPhysicalDeviceProperties2(physicalDevice, &physicalProps);

            var deviceInfo = new DeviceCreateInfo() {
                SType = StructureType.DeviceCreateInfo,
            };

            fixed (Device* device = &_device)
            {
                if (vk.CreateDevice(physicalDevice, &deviceInfo, null, device) != Result.Success)
                {
                    Console.WriteLine("Failed to create vulkan device");
                    return;
                }
            }

            var ver = driverProps.ConformanceVersion;

            Console.WriteLine("DriverId: ");
            Console.WriteLine(driverProps.DriverID.ToString().Replace("DriverID", string.Empty));

            Console.WriteLine("Vulkan Version: ");
            Console.WriteLine($"{ver.Major}.{ver.Minor}.{ver.Patch}.{ver.Subminor}");

            var str = Marshal.PtrToStringUTF8((IntPtr)driverProps.DriverInfo);
            Console.WriteLine(str);

            var str1 = Marshal.PtrToStringUTF8((IntPtr)driverProps.DriverName);
            Console.WriteLine(str1);
        }
    }
}
