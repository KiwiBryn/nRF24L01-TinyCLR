//---------------------------------------------------------------------------------
// Copyright (c) May 2020, devMobile Software
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
//---------------------------------------------------------------------------------
namespace devMobile.IoT.FieldGateway.TinyCLRV2nRF24Client
{
   using System;
   using System.Diagnostics;
   using System.Text;
   using System.Threading;

   using GHIElectronics.TinyCLR.Pins;

   using Radios.RF24;

   class Program
   {
      private const string BaseStationAddress = "Base1";
      private const string DeviceAddress = "Dev01";

      static void Main()
      {
         RF24 Radio = new RF24();

         try
         {
            Radio.OnDataReceived += Radio_OnDataReceived;
            Radio.OnTransmitFailed += Radio_OnTransmitFailed;
            Radio.OnTransmitSuccess += Radio_OnTransmitSuccess;

            // SC20100.GpioPin.PD3
            Radio.Initialize(SC20100.SpiBus.Spi3, SC20100.GpioPin.PD4, SC20100.GpioPin.PD3, SC20100.GpioPin.PC5);
            Radio.Address = Encoding.UTF8.GetBytes(DeviceAddress);

            Radio.Channel = 15;
            //Radio.PowerLevel = PowerLevel.Max;
            //Radio.PowerLevel = PowerLevel.High;
            //Radio.PowerLevel = PowerLevel.Low;
            //Radio.PowerLevel = PowerLevel.Minimum
            Radio.DataRate = DataRate.DR250Kbps;
            //Radio.DataRate = DataRate.DR1Mbps;
            Radio.IsEnabled = true;

            Radio.IsAutoAcknowledge = true;
            Radio.IsDyanmicAcknowledge = false;
            Radio.IsDynamicPayload = true;

            Debug.WriteLine($"Address: {Encoding.UTF8.GetString(Radio.Address)}");
            Debug.WriteLine($"PowerLevel: {Radio.PowerLevel}");
            Debug.WriteLine($"IsAutoAcknowledge: {Radio.IsAutoAcknowledge}");
            Debug.WriteLine($"Channel: {Radio.Channel}");
            Debug.WriteLine($"DataRate: {Radio.DataRate}");
            Debug.WriteLine($"IsDynamicAcknowledge: {Radio.IsDyanmicAcknowledge}");
            Debug.WriteLine($"IsDynamicPayload: {Radio.IsDynamicPayload}");
            Debug.WriteLine($"IsEnabled: {Radio.IsEnabled}");
            Debug.WriteLine($"Frequency: {Radio.Frequency}");
            Debug.WriteLine($"IsInitialized: {Radio.IsInitialized}");
            Debug.WriteLine($"IsPowered: {Radio.IsPowered}");

            while (true)
            {
               string payload = "hello " + DateTime.Now.Second;
               Debug.WriteLine($"{DateTime.UtcNow:HH:mm:ss}-TX {payload.Length} byte message {payload}");
               Radio.SendTo(Encoding.UTF8.GetBytes(BaseStationAddress), Encoding.UTF8.GetBytes(payload));

               Thread.Sleep(30000);
            }
         }
         catch (Exception ex)
         {
            Debug.WriteLine(ex.Message);

            return;
         }
      }

      private static void Radio_OnDataReceived(byte[] data)
      {
         // display as hex
         Debug.WriteLine($"{DateTime.UtcNow:HH:mm:ss}-RX Hex Length {data.Length} Payload {BitConverter.ToString(data)}");

         // Display as Unicode
         string unicodeText = Encoding.UTF8.GetString(data);
         Debug.WriteLine($"{DateTime.UtcNow:HH:mm:ss}-RX Unicode Length {unicodeText.Length} Unicode text {unicodeText}");
      }

      private static void Radio_OnTransmitSuccess()
      {
         Debug.WriteLine($"{DateTime.UtcNow:HH:mm:ss}-TX Succeeded!");
      }

      private static void Radio_OnTransmitFailed()
      {
         Debug.WriteLine($"{DateTime.UtcNow:HH:mm:ss}-TX failed!");
      }
   }
}
