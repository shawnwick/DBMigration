﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using PLX.DBMigration.Accessors;
using PLX.DBMigration.Managers;

namespace PLX.DBMigration.MigrationClient
{
    class Program
    {   
        static void Main(string[] args)
        {
            ConsoleColorSetup();
            
            if (CheckArgs(args))
                return;

            MigrationManager migrationManager = new MigrationManager();
            migrationManager.CurrentEdition = args[0];
            migrationManager.NextEdition = args[1];
            migrationManager.RunExport();

            ConsoleFinished();
            
        }

        /// <summary>
        /// Check the args given by the user.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        static bool CheckArgs(string[] args)
        {
            // Setup for any unhandled exceptions //
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;
            
            // Check each arg //
            foreach (var arg in args)
            {
                if (arg == "newxml")
                {
                    XmlAccessor xClass = new XmlAccessor();
                    xClass.CreateDefaultXml();
                    return true;
                }
                else if (arg == "testdb")
                {
                    Console.WriteLine("User ID: ");
                    string UserId = Console.ReadLine();
                    Console.WriteLine("Password: ");
                    string Password = Console.ReadLine();
                    Console.WriteLine("Privilege: ");
                    string Privilege = Console.ReadLine();
                    TestDbConnection(UserId, Password, Privilege);
                    return true;
                }
                else if (arg == "version")
                {
                    Console.WriteLine("Version - " + Assembly.GetEntryAssembly().GetName().Version.ToString());
                    return true;
                }
                else if (arg == "help")
                {
                    ShowHelp();
                    return true;
                }
                else if (arg.IndexOf("E") == 0)
                {
                    // Valid arg check //
                    if ((args[0].IndexOf("E") == 0) && (args[1].IndexOf("E") == 0))
                    {
                        return false;
                    }
                    else
                    {
                        Console.WriteLine("Invalid Entry!  Press enter to exit!");
                        Console.ReadLine();
                        return true;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid Entry!  Press enter to exit!");
                    Console.ReadLine();
                    return true;
                }
            }

            Console.WriteLine("Invalid Entry!  Press enter to exit!");
            Console.ReadLine();
            return true;
        }

        /// <summary>
        /// Handles any unhandled exception.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine(e.ExceptionObject.ToString());
            Console.WriteLine("Press Enter to continue");
            Console.ReadLine();
        }

        /// <summary>
        /// Test if the xml file is setup correctly for a database connection.
        /// </summary>
        static void TestDbConnection(string UserId, string Password, string Privilege)
        {
            Console.WriteLine("Testing Database Connection...");
            try
            {
                OracleAccessor oc = new OracleAccessor();
                oc.ConnectToOracle(UserId, Password, Privilege);
                oc.Connection.Close();
            }
            catch
            {
            }
        }

        /// <summary>
        /// Show Help File.
        /// </summary>
        static void ShowHelp()
        {
            Console.WriteLine();
            Console.WriteLine("*** Help File ***");
            Console.WriteLine();
            Console.WriteLine("$ EXX EXY - This will start the full upgrade process.");
            Console.WriteLine("  \"EXX\" is current edition \"EXY\" is next edition.");
            Console.WriteLine("$ newxml - Create a new default xml file \"XmlDefault.xml\".");
            Console.WriteLine("  This file can be modified for the correct database parameters.");
            Console.WriteLine("$ testdb - Test a database connection.");
            Console.WriteLine("$ version - Show the current software version.");
            Console.WriteLine();
            Console.WriteLine("*** End of Help File ***");
        }

        /// <summary>
        /// Feature setup for cmd.
        /// </summary>
        static void ConsoleColorSetup()
        {
            Console.Title = "Brian Bot";
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Green;
        }

        /// <summary>
        /// Ending features for cmd.
        /// </summary>
        static void ConsoleFinished()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("** Brian Bot Finished, press enter to exit! **");
            Console.ReadLine();
        }
    }
}
