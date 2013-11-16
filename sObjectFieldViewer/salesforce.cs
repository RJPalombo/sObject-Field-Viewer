using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Services.Protocols;
using System.Windows.Forms;
using sObjectFieldViewer.sforce;

namespace sObjectFieldViewer
{
    class salesforce
    {
        public bool loggedin;
        private int loginattempts;
        private bool didCancel;
        public SforceService binding;
        private const string APIVersion     = "27.0";
        private const string ProductionURL  = "https://login.salesforce.com/services/Soap/u/";
        private const string SandboxURL     = "https://test.salesforce.com/services/Soap/u/";

        public salesforce()
        {
            loggedin = false;
            loginattempts = 0;
            didCancel = false;
        }

        public bool login()
        {
            //allow 3 loggin attempts
            for (int i = 0; i < 3; i++)
            {
                if (!loggedin && !didCancel)
                {
                    auth();
                }
            }

            if (!loggedin && !didCancel) { MessageBox.Show("You have attempted to loggin 3 times. Please try again later."); }

            //reset didCancel
            didCancel = false;

            return loggedin;
        }

        public void logout()
        {
            binding.logout();
            loggedin = false;
        }

        private void auth()
        {
            login lf = new login();
            DialogResult lfDialog = lf.ShowDialog();

            try
            {
                if (lfDialog == DialogResult.OK)
                {
                    loginattempts++;

                    //******************************************************
                    //NOTES: Find out how to loggin into test.salesforce.com
                    //******************************************************
                    
                    //Login to SalesForce
                    binding = new SforceService();

                    binding.Timeout = 60000;
                    if(!lf.getSandbox())
                    {
                        binding.Url = ProductionURL + APIVersion + "/";        
                    }
                    else 
                    {
                        binding.Url = SandboxURL + APIVersion + "/";
                    }

                    string un = lf.getUsername(), pw = lf.getPassword();
                    LoginResult loginResult = binding.login(un, pw);

                    binding.Url = loginResult.serverUrl;

                    //Test login URL
                    //MessageBox.Show(loginResult.serverUrl + " | Is Sandbox: " + loginResult.sandbox);

                    binding.SessionHeaderValue = new SessionHeader();
                    binding.SessionHeaderValue.sessionId = loginResult.sessionId;
                    
                    //Test if user is loggged in
                    //GetUserInfoResult userInfo = loginResult.userInfo;
                    //MessageBox.Show("Hello " + userInfo.userFullName + "!!!");

                    loggedin = true;
                }
                else
                {
                    loggedin = false;
                    didCancel = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message +
                                Environment.NewLine +
                                ex.InnerException +
                                Environment.NewLine +
                                ex.Source +
                                Environment.NewLine +
                                ex.Data +
                                Environment.NewLine +
                                ex.StackTrace);
            }
        }

        public ArrayList getSalesForceObjects()
        {
            ArrayList retArrayList = new ArrayList();

            try
            {
                DescribeGlobalResult dgr = binding.describeGlobal();

                for (int i = 0; i < dgr.sobjects.Length; i++)
                {
                    retArrayList.Add(dgr.sobjects[i].name);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return retArrayList;
        }

        public List<sObjectDetails> sObjectDescribe(string sObject)
        {
            List<sObjectDetails> sObjectDetailsList = new List<sObjectDetails>();

            try
            {
                //Invoke describeSObject and save results in DescribeSObjectResult 
                DescribeSObjectResult dsr = new DescribeSObjectResult();
                dsr = binding.describeSObject(sObject);

                if (dsr != null)
                {
                    foreach (Field fld in dsr.fields)
                    {
                        sObjectDetails sod = new sObjectDetails();
                        sod.Name = fld.label;
                        sod.APIName = fld.name;
                        sod.DataType = fld.type.ToString();
                        
                        //get Field Length or precision with decimal places where applicable
                        if (fld.length != 0)
                        {
                            sod.Length = fld.length.ToString();
                        }
                        else
                        {
                            sod.Length = fld.precision.ToString();
                            //get decimal places
                            if (fld.scale != 0) { sod.Length = sod.Length + ", " + fld.scale; }
                        }
                        
                        //Attributes
                        sod.Attributes = "Custom Field: " + fld.custom + Environment.NewLine;
                        sod.Attributes = sod.Attributes + "Required: " + !fld.nillable + Environment.NewLine;
                        
                        //get picklist values into Attributes - deprecated - will be adding picklist values into their own column
                        /*
                        string PickVals = "";
                        if (fld.picklistValues != null)
                        {
                            PickVals = Environment.NewLine;
                            PickVals = PickVals + "Picklist Values:" + Environment.NewLine;
                            foreach (PicklistEntry value in fld.picklistValues)
                            {
                                if (value.active)
                                {
                                    PickVals = PickVals + value.value + Environment.NewLine;
                                }
                            }
                        }
                        */

                        //Field Help
                        sod.FieldHelp = fld.inlineHelpText;

                        //--sod.Attributes = sod.Attributes + PickVals; //picklist values will now be in their own column - set below this line
                        sod.PicklistValues = getPicklistValues(fld);

                        sObjectDetailsList.Add(sod);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message +
                                Environment.NewLine +
                                ex.InnerException +
                                Environment.NewLine +
                                ex.Source +
                                Environment.NewLine +
                                ex.Data +
                                Environment.NewLine +
                                ex.StackTrace);
            }

            return sObjectDetailsList;
        }

        /* Used incase excel cannot handle cell text larger than 1024 characters
        private static string cleanString( string inStr )
        {
            if (inStr == null) return "";
            if (inStr.Length >= 1023)
            {
                return inStr.Substring(0, 1023); //excel can only handle 1024 characters
            }
            else
            {
                return inStr;
            }
        }
        */

        private static string getPicklistValues(Field field)
        {
            string pickVals = "";

            // If this is a picklist field, show the picklist values
            if (field.type.Equals(fieldType.picklist))
            {
                PicklistEntry[] picklistValues = field.picklistValues;
                if (picklistValues != null)
                {
                    //Console.WriteLine("Picklist values: ");
                    for (int j = 0; j < picklistValues.Length; j++)
                    {
                        if (picklistValues[j].label != null)
                        {
                            pickVals += picklistValues[j].label.ToString() + (char)10;
                        }
                    }
                }
            }

            return pickVals;
        }

    }

    public class sObjectDetails
    {
        public string Name;
        public string APIName;
        public string DataType;
        public string Length;
        public string Attributes;
        public string FieldHelp;
        public string PicklistValues;

        public sObjectDetails()
        {
        }
        public sObjectDetails(string _Name, string _APIName, string _DataType, string _Length, string _Attributes, string _FieldHelp, string _PicklistValues)
        {
            Name = _Name;
            APIName = _Name;
            DataType = _DataType;
            Length = _Length;
            Attributes = _Attributes;
            FieldHelp = _FieldHelp;
            PicklistValues = _PicklistValues;
        }
    }
}
