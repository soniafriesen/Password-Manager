/*
 * Program:         PasswordManager.exe
 * Module:          Account.cs
 * Date:            Due June 7th 2020
 * Author:          Sonia Friesen, 0813682
 * Description:     A class to make the json file and schema for the main program
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountManager
{
    class Account
    {        
        public string UserId { get; set; }
        public string loginUrl { get; set; }
        public string AccountNum { get; set; }

        public List<Password> passwords = new List<Password>(); //list of passwords used to create an account

        /*
         Name: addPassword
         Purpose: To add a password to an account
         Parameters: A password object
         Returns: nothing
         */
        public void addPassword(Password pass)
        {
            passwords.Add(pass);
        }
        
    }

    class Password //for a password object
    {
        public string Value { get; set; }
        public int StrengthNum { get; set; }
        public string StrengthText { get; set; }
        public string LastReset { get; set; }      
    }

    //using a company class to contain the accounts for a specific company
    class Company
    {
        public string description { get; set; } //compnay name to say
        public List<Account> accounts = new List<Account>(); //a list of accounts for that comapny

        /*
         Name: addAccount
         Purpose: To add an account to a company
         Parameters: A accoutn object
         Returns: nothing
         */
         public void addAccount(Account account)
        {
            accounts.Add(account);
        }
    }

    //list of comapnies
    class CompanyList
    {
        public List<Company> company = new List<Company>();

        /*
         Name: addcompany
         Purpose: To add a company to a company list
         Parameters: A company object
         Returns: nothing
         */
         public void addCompany(Company comp)
        {
            company.Add(comp);
        }
    }

}
