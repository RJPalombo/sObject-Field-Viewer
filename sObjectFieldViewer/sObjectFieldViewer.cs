using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExcelDna.Integration;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace sObjectFieldViewer
{
    public class sObjectFieldViewer
    {
        //instantiate salesforce custom class and sforce for application state
        static salesforce sf = new salesforce();
        
        //Headers for Schema
        private static string[] headers = { "Label", "API Name", "Data Type", "Length", "Attributes", "Field Help", "Picklist Values" };

        
        //DataGridViewRowsAddedEventArgs picklist values


        [ExcelCommand(MenuName = "&SForce Excel Helpers", MenuText = "&Get fields from object", Name="GetSchema")]
        public static void MenuGetSchema()
        {
            try
            {
                if (sf.login())
                {
                    chooseSObject co = new chooseSObject();

                    //Retrieve sForce sObjects
                    co.setSObjects(sf.getSalesForceObjects());

                    //Show sForce objects for user to choose
                    DialogResult coDialog = co.ShowDialog();
                    List<sObjectDetails> sObjectSchemaList = new List<sObjectDetails>();
                    if (coDialog == DialogResult.OK)
                    {
                        sObjectSchemaList = sf.sObjectDescribe(co.getSObject());
                    }

                    //Start Excel
                    ExcelReference exRefSel = (ExcelReference)XlCall.Excel(XlCall.xlfSelection);

                    //Set max rows and only the amount of columns you need (Last Row used to be set to ExcelDnaUtil.ExcelLimits.MaxRows)
                    //object[,] ft = new object[ExcelDnaUtil.ExcelLimits.MaxRows, headers.Count()];
                    object[,] ft = new object[sObjectSchemaList.Count + 1, headers.Count()];
                    //MessageBox.Show("Row Count: " + sObjectSchemaList.Count + 1);
                    
                    exRefSel = new ExcelReference(exRefSel.RowFirst,
                                                  exRefSel.RowFirst + sObjectSchemaList.Count - 1,
                                                  exRefSel.ColumnFirst,
                                                  exRefSel.ColumnFirst + headers.Count() - 1
                                                  );

                    int increment = 0;
                    foreach (string header in headers)
                    {
                        ft[0, increment] = header;
                        increment++;
                    }

                    //reset increment
                    increment = 0;
                    foreach (sObjectDetails sObjectDetail in sObjectSchemaList)
                    {
                        for (int i = 0; i < headers.Count(); i++)
                        {
                            switch (i)
                            {
                                case 0: 
                                    ft[increment + 1, i] = sObjectDetail.Name;
                                    break;
                                case 1: 
                                    ft[increment + 1, i] = sObjectDetail.APIName;
                                    break;
                                case 2: 
                                    ft[increment + 1, i] = sObjectDetail.DataType;
                                    break;
                                case 3: 
                                    ft[increment + 1, i] = sObjectDetail.Length;
                                    break;
                                case 4:
                                    ft[increment + 1, i] = sObjectDetail.Attributes;
                                    break;
                                case 5:
                                    ft[increment + 1, i] = sObjectDetail.FieldHelp;
                                    break;
                                case 6:
                                    ft[increment + 1, i] = sObjectDetail.PicklistValues;
                                    break;
                            }
                        }
                        increment++;
                    }

                    //Set cells
                    exRefSel.SetValue(ft);
                    /*
                    dynamic xlApp = ExcelDnaUtil.Application;
                    var range = xlApp.Range[exRefSel];
                    range.value = ft;
                     * */

                    //format cells 
                    Excel.Application excelApp = (Excel.Application)ExcelDnaUtil.Application;
                    excelApp.Range[XlCall.Excel(XlCall.xlfReftext, exRefSel, true)].VerticalAlignment = Excel.XlTopBottom.xlTop10Top;
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

    }
}