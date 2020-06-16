/*
 * Program:         PasswordManager.exe
 * Module:          PasswordManager.cs
 * Date:            <Due June 7th 2020>
 * Author:          <Sonia Friesen, 0813682>
 * Description:     Some free starting code for INFO-3138 project 1, the Password Manager
 *                  application. All it does so far is demonstrate how to obtain the system date 
 *                  and how to use the PasswordTester class provided.
 *                  UPDATED: includes the full console project
 */

using System;
using System.Collections.Generic;
using System.IO;            // File class
using Newtonsoft.Json;  // JsonConvert class
using Newtonsoft.Json.Schema; //Schema Library
using Newtonsoft.Json.Linq;
using AccountManager; //holds the account information

namespace PasswordManager
{
    class Program
    {
        /*
            NOTE: Files are written to the debug folder. 
            NOTE: The Folder that says project tests is part 1 to the project. 
        */
        static void Main(string[] args)
        {
            // System date demonstration          
            Console.Write("PASSWORD MANAGEMENT SYSTEM, BY Sonia Friesen");

            string temp;
            int value;
            bool done = false; //used to determin if user is done with page

            //create file (in debuger)
           var filName = new FileInfo("Accounts.json");           

            //check to see if the file exists 
            if (!filName.Exists || filName.Length == 0)
            {
                //start the intro prompt
                Console.WriteLine("\n************************************************************************************");
                Console.WriteLine("+                                Account Entries                                   +");
                Console.WriteLine("************************************************************************************\n");
                Console.WriteLine("\nNo accounts found. Please Enter Accounts");
                var newFile = File.Create("Accounts.json"); //creates onyl if the file above is not found
                newFile.Close();

                Console.WriteLine("\n************************************************************************************");
                Console.WriteLine("+                                      Menu                                        +");
                Console.WriteLine("************************************************************************************\n");
                Console.Write("+ Press A to add the Account                                                        +");
                Console.Write("\n+ Press Q to Quit the Application                                                   +");
                Console.WriteLine("\n************************************************************************************\n");

                bool valid = false; //for checking the validation agains schema
                while (valid == false)
                {
                    Console.WriteLine("Enter Submition here:");
                    temp = Console.ReadLine();
                    valid = true;
                    //check to see what user inputs
                    if (temp.Equals("a") || temp.Equals("A"))
                    {
                        CompanyList compList = new CompanyList(); //get a new comapny databse to hold the accounts
                        bool added; //when the user is finished adding accounts

                        do //loop to get a valid answer
                        {
                            Company company = new Company();//get a new company object for accounts
                            Console.WriteLine("\n\nPlease enter the informtion below...");
                            bool ok = false; //validate agains schema

                            Console.Write("Description: ");
                            while (ok == false)
                            {
                                ok = true;
                                company.description = Console.ReadLine();
                                if (company.description == "")
                                {
                                    //tell user its not valid
                                    Console.WriteLine("\nDescription is required!!\n");
                                    Console.Write("Description: ");
                                    ok = false;
                                }
                            }
                            Account account = new Account();
                            Console.Write("UserId: ");
                            bool val = false;
                            while (val == false)
                            {
                                val = true;
                                account.UserId = Console.ReadLine();
                                if (account.UserId == "")
                                {
                                    Console.WriteLine("\nUserId is required!!\n");
                                    Console.Write("UserId:");
                                    val = false;
                                }
                            }
                            Password pass = new Password();
                            bool pgood = false;
                            while (pgood == false)
                            {
                                pgood = true;
                                Console.Write("Password: ");
                                string pwText = Console.ReadLine();
                                pass.Value = pwText;

                                try
                                {
                                    // PasswordTester class to validate 
                                    PasswordTester pw = new PasswordTester(pwText);
                                    pass.StrengthText = pw.StrengthLabel;
                                    pass.StrengthNum = pw.StrengthPercent;
                                }
                                catch (ArgumentException)
                                {
                                    Console.WriteLine("ERROR: Invalid password format");

                                    pgood = false;
                                }
                                catch (InvalidOperationException)
                                {
                                    Console.WriteLine("Password is required!!!");

                                    pgood = false;
                                }
                            }
                            Console.Write("LoginURL: ");
                            account.loginUrl = Console.ReadLine();

                            Console.Write("Account#: ");                 
                            account.AccountNum = Console.ReadLine();
                                

                            DateTime dateNow = DateTime.Now;
                            pass.LastReset = dateNow.ToShortDateString();

                            account.addPassword(pass);
                            company.addAccount(account);
                            compList.addCompany(company);

                            //see if user wants to add another account
                            Console.Write("\nAdd another Account? (y/n): ");
                            added = Console.ReadKey().KeyChar != 'y';
                            if (added.Equals(true))
                            {
                               ;
                                Console.WriteLine("\nPlease restart the application\n");
                              
                            }

                        } while (!added);

                        writeCompanyToJsonFile(compList);
                    }
                    else if (temp.Equals("q") || temp.Equals("Q"))
                    {
                        Console.WriteLine("\n************************************************************************************");
                        Console.WriteLine("+                         Applications Done..Closing                                +");
                        Console.WriteLine("**************************************************************************************");
                        System.Environment.Exit(1);
                    }

                    else
                    {
                        Console.WriteLine("***Please enter Valid Input***");
                        valid = false;
                    }
                }
            }
            else
            {
                while(done == false)
                {
                    done.Equals(true);
                    CompanyList readList = new CompanyList(); //create a new comapny list
                    readList = ReadJsonFile();

                    //main application                   
                    Console.WriteLine("\n************************************************************************************");
                    Console.WriteLine("+                                Account Entries                                   +");
                    Console.WriteLine("************************************************************************************\n");
                    if(readList.company.Count == 0)
                    {
                        
                        Console.WriteLine("\nNo Accounts! Please add some \n");
                        
                    }
                    int num = 1;
                    //if data exists, show the accounts descripdiotn/name
                    foreach(Company comp in readList.company)
                    {
                        Console.Write((num) + $". {comp.description}");
                        Console.WriteLine();
                        num += 1; 
                    }
                    Console.WriteLine("\n************************************************************************************");
                    Console.WriteLine("+                                      Menu                                        +");
                    Console.WriteLine("************************************************************************************\n");
                    Console.Write("+ Press A to add the Account                                                          +");
                    Console.Write("\n+ Press # to an Account you wish to edit                                            +");
                    Console.Write("\n+ Press Q to Quit the Application                                                   +");
                    Console.WriteLine("\n************************************************************************************\n");
                    Console.WriteLine("Enter selection here: ");
                    temp = Console.ReadLine();
                    if(int.TryParse(temp,out value) || temp.Equals("Q") || temp.Equals("q")|| temp.Equals("a") || temp.Equals("A"))
                    { }
                    else
                    {
                        //get and error if input is not any of the required options
                        
                        Console.WriteLine("\nInvalid Input!\n");
                        
                    }
                    //adding a user
                    if(temp.Equals("a") || temp.Equals("A"))
                    {
                        bool add;
                        do
                        {

                            Company company = new Company();
                            Console.WriteLine("\n\nPlease enter the following information...\n");
                            bool ok = false;
                            bool valid = false;
                            bool correct = false;
                            Console.Write("Decription: ");

                            while (ok == false)
                            {
                                ok = true;
                                company.description = Console.ReadLine();

                                if (company.description == "")
                                {
                                    
                                    Console.WriteLine("\nDescription is required!\n");
                                    
                                    Console.Write("Description: ");
                                    ok = false;
                                }
                            }
                            Account account = new Account();
                            Console.Write("UserId: ");
                            while (valid == false)
                            {
                                valid = true;
                                account.UserId = Console.ReadLine();
                                if (account.UserId == "")
                                {
                                    //ERROR Message
                                    
                                    Console.WriteLine("\nUserId is required!!\n");
                                    
                                    Console.Write("UserId:");
                                    valid = false;
                                }

                            }
                            Password pass = new Password();
                            while (correct == false)
                            {
                                correct = true;

                                Console.Write("Password: ");
                                string pwText = Console.ReadLine();

                                pass.Value = pwText;
                                //acc.password = pwText;
                                try
                                {
                                    // PasswordTester class demonstration
                                    PasswordTester pw = new PasswordTester(pwText);
                                    pass.StrengthText = pw.StrengthLabel;
                                    pass.StrengthNum = pw.StrengthPercent;
                                }
                                catch (ArgumentException)
                                {
                                    //ERROR Message                                  
                                    Console.WriteLine("\nERROR: Invalid password format\n");
                                   
                                    correct = false;
                                }
                                catch (InvalidOperationException)
                                {
                                    //ERROR Message                                   
                                    Console.WriteLine("\nPassword is required!");                                    
                                    correct = false;
                                }

                            }
                            Console.Write("LoginUrl: ");
                            account.loginUrl = Console.ReadLine();

                            Console.Write("Account#: ");
                            account.AccountNum = Console.ReadLine();

                            DateTime dateNow = DateTime.Now;
                            pass.LastReset = dateNow.ToShortDateString();


                            account.addPassword(pass);
                            company.addAccount(account);
                            readList.addCompany(company);

                            Console.Write("\nAdd another item? (y/n): ");
                            add = Console.ReadKey().KeyChar != 'y';
                        } while (!add);

                        //write the JSON file with the new data added or changed
                        writeCompanyToJsonFile(readList);
                        done = false;
                    }
                    //we need to see if they want to change any value in the list
                    else if(int.TryParse(temp, out value))
                    {
                        int numberEntry = value - 1;
                        try
                        {
                            //access information based on user input
                            Console.WriteLine("\n" + value + $". {readList.company[numberEntry].description}");
                            Console.WriteLine();

                            //loop though the accounts
                            foreach(Account account in readList.company[numberEntry].accounts)
                            {
                                string password = "";
                                string label = "";
                                int number = 0;
                                string readDate = "";
                                Console.WriteLine($"UserId: {account.UserId}");
                                foreach(Password pass in readList.company[numberEntry].accounts[0].passwords)
                                {
                                    password = pass.Value;
                                    label = pass.StrengthText;
                                    number = pass.StrengthNum;
                                    readDate = pass.LastReset;
                                }
                                Console.WriteLine($"Password: {password}");
                                Console.WriteLine($"Password Strength: {label} ({number})%");
                                Console.WriteLine($"Password Reset Date: {readDate}");
                                Console.WriteLine($"LoginUrl: {account.loginUrl}");
                                Console.WriteLine($"Account #: {account.AccountNum}");
                            }
                            //create a menu for changes
                            Console.WriteLine("\n************************************************************************************");
                            Console.WriteLine("+ Press M to for Main Menu                                                         +");
                            Console.WriteLine("+ Press P to update the password                                                   +");
                            Console.WriteLine("+ Press D to delete the Account Info                                               +");
                            Console.WriteLine("************************************************************************************\n");

                            Console.Write("Enter seleciton here: ");
                            temp = Console.ReadLine();

                            //For Main Menu
                            if (temp.Equals("M") || temp.Equals("m"))
                            {
                                done.Equals(false);
                            }

                            //user changes passswords
                            else if (temp.Equals("P") || temp.Equals("p"))
                            {
                                Console.Write("\nNew Password: ");
                                string newPass = Console.ReadLine();
                                readList.company[numberEntry].accounts[0].passwords[0].Value = newPass;
                                DateTime dateNow = DateTime.Now;
                                readList.company[numberEntry].accounts[0].passwords[0].LastReset = dateNow.ToShortDateString();
                                writeCompanyToJsonFile(readList);                                
                                Console.WriteLine("***Password Changed***");
                               
                                done.Equals(false);
                            }

                            //deleting a password/account
                            else if (temp.Equals("D") || temp.Equals("d"))
                            {
                                try
                                {
                                    Console.Write("Delete?(Y/N): ");
                                    temp = Console.ReadLine();
                                    if (temp.Equals("Y") || temp.Equals("y"))
                                    {
                                        readList.company.RemoveAt(numberEntry);
                                        writeCompanyToJsonFile(readList);
                                        done.Equals(false);
                                    }
                                    else if (temp.Equals("N") || temp.Equals("n"))
                                    {
                                        done.Equals(false);
                                    }

                                    else
                                    { 
                                        //Throws exception to handle the wrong input
                                        throw new IndexOutOfRangeException();
                                    }
                                }
                                catch (Exception e) //Catches all the exceptions
                                {                                    
                                    Console.WriteLine("\nPlease enter a valid input!\n");
                                }
                            }
                            else
                            {
                                throw new IndexOutOfRangeException();//Throws exception to handle the wrong input
                            }
                        }
                        catch (Exception e)  //Catch exceptions
                        {
                            
                            Console.WriteLine("\nPlease enter valid number!\n");                           
                            done.Equals(false);
                        }
                    }
                    //user wants to quit application
                    //IF wants to quit
                    else if (temp.Equals("q") || temp.Equals("Q"))
                    {
                        Console.WriteLine("\n**********************************************************************************");
                        Console.WriteLine("+                        Thank you,application closing.....                       +");
                        Console.WriteLine("************************************************************************************");
                        System.Environment.Exit(1);
                    }
                }
            }
        }//end main

        /*
        * Method Name: writeCompToJsonFile
        * Purpose: Converts the inputed data into JSON file and also calls the validation method
        * Parameters: CompanyList Object
        * Returns: Void
        */
        private static void writeCompanyToJsonFile(CompanyList complist)
        {
            //Serlialization
            string json = JsonConvert.SerializeObject(complist);
            string data = json;
            string Schema = "";
            ReadFile("AccountSchema.json", out Schema);

            IList<string> messages;
            if (ValidateEnteredData(data, Schema, out messages)) // Note: messages parameter is optional
            {
               
                Console.WriteLine($"\nData file is valid.\n");               
                File.WriteAllText("Accounts.json", json);

            }
            else
            {               
                Console.WriteLine($"\nERROR:\tData file is invalid.\n");
               
                // Report validation error messages
                foreach (string msg in messages)
                    Console.WriteLine($"\t{msg}");
            }

        }

        /*
       * Method Name: ReadJsonFile
       * Purpose: Reads and deserlizes the specified JSON File  
       * Parameters: Nothing
       * Returns:CompanyList Object
       */
        private static CompanyList ReadJsonFile()
        {
            string json = File.ReadAllText("Accounts.json");
            return JsonConvert.DeserializeObject<CompanyList>(json);

        }

        /*
       * Method Name: ReadFile
       * Purpose: Just reads the file, used to read the Schema
       * Arguments: path of the file
       * Returns: True/False if successful/unsuccessful in reading the file and also outputs the read file (only if true)
       */
        private static bool ReadFile(string path, out string json)
        {
            try
            {
                // Read JSON file data 
                json = File.ReadAllText(path);
                return true;
            }
            catch
            {
                json = null;
                return false;
            }
        }

        /*
       * Method Name: ValidateEnteredData
       * Purpose: Validates the JSON file against the specified Schema
       * Parameters: Json File, Schema File
       * Returns: True-If successful in validating and False- if not along with the error messages
       */
        private static bool ValidateEnteredData(string json, string Schema, out IList<string> messages)
        {
            JSchema schema = JSchema.Parse(Schema);
            JObject account = JObject.Parse(json);
            return account.IsValid(schema, out messages);
        }
    } // end class
}
