using System;
using Microsoft.Win32;

namespace RegistryPersistenceSimulation
{
    class Program
    {
        // Target registry path under HKEY_CURRENT_USER
        private const string SubKeyPath = @"Software\AcademicTestVirus";
        private const string SignatureValueName = "MalwareSignature";
        private const string SignatureContent = "SIGNATURE{REGEDIT-HARMLESS-2026}";

        // Simulated Autorun path for persistence tracking
        private const string RunKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Run";
        private const string FakeAppName = "AcademicTargetBeacon";

        static void Main(string[] args)
        {
            Console.Title = "Registry Persistence & AV Simulation Tool";
            bool running = true;

            while (running)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("==================================================");
                Console.WriteLine("   REGISTRY PERSISTENCE & AV SIMULATION TOOL      ");
                Console.WriteLine("==================================================");
                Console.ResetColor();
                Console.WriteLine("1. Run Test Malware (Simulate Persistence)");
                Console.WriteLine("2. Run Antivirus Scanner & Cleaner");
                Console.WriteLine("3. Exit");
                Console.WriteLine("==================================================");
                Console.Write("Select an option (1-3): ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ExecuteMalwareSimulation();
                        break;
                    case "2":
                        ExecuteAntivirusScanner();
                        break;
                    case "3":
                        running = false;
                        Console.WriteLine("Exiting program...");
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid option. Press any key to retry.");
                        Console.ResetColor();
                        Console.ReadKey();
                        break;
                }
            }
        }

        /// <summary>
        /// Step 3: Simulates malware persistence by writing a signature and a fake autorun key to the registry.
        /// </summary>
        static void ExecuteMalwareSimulation()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[*] Launching Academic Malware Simulation...");
            Console.ResetColor();

            try
            {
                // 1. Create or open the custom academic registry path under HKEY_CURRENT_USER
                Console.WriteLine($"[*] Creating registry key: HKCU\\{SubKeyPath}");
                using (RegistryKey academicKey = Registry.CurrentUser.CreateSubKey(SubKeyPath))
                {
                    if (academicKey != null)
                    {
                        // Write the safe malware signature string
                        academicKey.SetValue(SignatureValueName, SignatureContent, RegistryValueKind.String);
                        Console.WriteLine($"[+] Successfully wrote signature: {SignatureContent}");
                    }
                }

                // 2. Simulate persistence via the Windows Run key (Autorun mechanism)
                Console.WriteLine($"[*] Injecting fake autorun persistence: HKCU\\{RunKeyPath}");
                using (RegistryKey runKey = Registry.CurrentUser.OpenSubKey(RunKeyPath, true))
                {
                    if (runKey != null)
                    {
                        // Simulate an autorun path pointing to a dummy executable location
                        string fakeExecutionPath = @"""C:\Program Files\AcademicSimulation\beacon.exe"" --silent";
                        runKey.SetValue(FakeAppName, fakeExecutionPath, RegistryValueKind.String);
                        Console.WriteLine($"[+] Successfully registered fake startup key: {FakeAppName}");
                    }
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n[SUCCESS] Malware simulation executed smoothly. Registry states are modified.");
            }
            catch (UnauthorizedAccessException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n[ERROR] Access Denied! Please ensure you have sufficient registry permissions.");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n[ERROR] An unexpected error occurred: {ex.Message}");
            }

            Console.ResetColor();
            Console.WriteLine("\nPress any key to return to main menu.");
            Console.ReadKey();
        }

        /// <summary>
        /// Step 4: Scans the registry for known malware signatures and prompts the user for automated cleanup.
        /// </summary>
        static void ExecuteAntivirusScanner()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[*] Initializing Signature-Based Antivirus Engine...");
            Console.ResetColor();

            bool signatureDetected = false;
            bool runKeyDetected = false;

            try
            {
                // 1. Scan for the unique academic malware signature
                Console.WriteLine($"[*] Scanning path: HKCU\\{SubKeyPath}");
                using (RegistryKey academicKey = Registry.CurrentUser.OpenSubKey(SubKeyPath, false)) // false = Read-only access
                {
                    if (academicKey != null)
                    {
                        object valueReceived = academicKey.GetValue(SignatureValueName);
                        if (valueReceived != null && valueReceived.ToString() == SignatureContent)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"[ALERT] Found Malware Signature: '{SignatureContent}' in SubKey!");
                            Console.ResetColor();
                            signatureDetected = true;
                        }
                    }
                }

                // 2. Scan for the associated Autorun persistence mechanism
                Console.WriteLine($"[*] Scanning path: HKCU\\{RunKeyPath}");
                using (RegistryKey runKey = Registry.CurrentUser.OpenSubKey(RunKeyPath, false))
                {
                    if (runKey != null)
                    {
                        object runValue = runKey.GetValue(FakeAppName);
                        if (runValue != null)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"[ALERT] Found Active Persistence Vector: '{FakeAppName}' -> {runValue}");
                            Console.ResetColor();
                            runKeyDetected = true;
                        }
                    }
                }

                // Decision and Remediation phase
                if (signatureDetected || runKeyDetected)
                {
                    Console.WriteLine("\n==================================================");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("[?] Threats detected! Do you want to clean the system? (Y/N): ");
                    Console.ResetColor();

                    string decision = Console.ReadLine()?.Trim().ToUpper();

                    if (decision == "Y")
                    {
                        Console.WriteLine("\n[*] Starting automated threat mitigation...");
                        ExecuteCleanup(signatureDetected, runKeyDetected);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine("\n[!] Remediation canceled by user. System remains vulnerable.");
                        Console.ResetColor();
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n[+] Scan finished. No threats detected. System is clean.");
                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n[ERROR] Scanning failed due to an exception: {ex.Message}");
                Console.ResetColor();
            }

            Console.WriteLine("\nPress any key to return to main menu.");
            Console.ReadKey();
        }

        /// <summary>
        /// Sub-method responsible for securely deleting registry values based on scan results.
        /// </summary>
        /// <param name="cleanSignature">Indicates whether the signature key should be removed.</param>
        /// <param name="cleanRunKey">Indicates whether the persistence run key should be removed.</param>
        private static void ExecuteCleanup(bool cleanSignature, bool cleanRunKey)
        {
            try
            {
                // Purge the malware signature path completely
                if (cleanSignature)
                {
                    // Delete the entire subkey tree safely
                    Registry.CurrentUser.DeleteSubKeyTree(SubKeyPath, false);
                    Console.WriteLine("[+] Cleaned up custom malware subkey tree.");
                }

                // Purge only the specific fake autorun value
                if (cleanRunKey)
                {
                    using (RegistryKey runKey = Registry.CurrentUser.OpenSubKey(RunKeyPath, true)) // true = Write access required
                    {
                        if (runKey != null)
                        {
                            runKey.DeleteValue(FakeAppName, false);
                            Console.WriteLine("[+] Cleaned up fake autorun startup key.");
                        }
                    }
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n[SUCCESS] System cleanup successfully completed!");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n[ERROR] Remediation failed: {ex.Message}");
                Console.ResetColor();
            }
        }
    }
}