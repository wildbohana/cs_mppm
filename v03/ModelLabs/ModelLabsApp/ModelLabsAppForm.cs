﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Reflection;
using CIM.Model;
using CIMParser;
using FTN;

namespace ModelLabsApp
{
    public partial class ModelLabsAppForm : Form
    {
        public const string ProfileName = "PowerTransformerCIMProfile_Labs";
        public const string Namespace = "FTN";

        private ConcreteModel concreteModel = new ConcreteModel();


        public ModelLabsAppForm()
        {
            InitializeComponent();

            buttonLoadCIM.Enabled = false;
        }



        private void ShowOpenCIMXMLFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Open CIM Document File..";
            openFileDialog.Filter = "CIM-XML Files|*.xml;*.txt;*.rdf|All Files|*.*";
            openFileDialog.RestoreDirectory = true;

            DialogResult dialogResponse = openFileDialog.ShowDialog(this);
            if (dialogResponse == DialogResult.OK)
            {
                textBoxCIMFile.Text = openFileDialog.FileName;
                toolTipControl.SetToolTip(textBoxCIMFile, openFileDialog.FileName);
                buttonLoadCIM.Enabled = true;
            }
            else
            {
                buttonLoadCIM.Enabled = false;
            }
        }

        private void LoadCIMXMLIntoConcreteModel()
        {
            ////LOAD CIM/RDF AND MAKE A ConcreateModel
            try
            {
                concreteModel = null;
                string log;
                using (FileStream fs = File.Open(textBoxCIMFile.Text, FileMode.Open))
                {
                    if (LoadModelFromExtractFile(fs, ref concreteModel, out log))
                    {
                        // TODO
                        string ispis = "";
                        int iteracije = concreteModel.ModelMap.Count;
                        List<string> kljucevi = concreteModel.ModelMap.Keys.ToList<string>();

                        // Prvi deo
                        for (int i = 0; i < iteracije; i++)
                        {
                            SortedDictionary<string, object> sd = concreteModel.ModelMap[kljucevi[i]];

                            string tip = kljucevi[i];
                            int br = sd.Count;

                            ispis += "Broj objekata klase [" + tip + "] je: " + br + "\n";
                        }

                        // Drugi deo
                        if (concreteModel.ModelMap.ContainsKey("FTN.WindingTest"))
                        {
                            if (concreteModel.ModelMap["FTN.WindingTest"].ContainsKey("939140759_TW1_WT"))
                            {
                                WindingTest wt = concreteModel.ModelMap["FTN.WindingTest"]["939140759_TW1_WT"] as WindingTest;
                                float ll = wt.LoadLoss;

                                ispis += "\nLoadLoss za element sa mrID=939140759_TW1_WT je: " + ll;
                            }
                            else
                            {
                                ispis += "\nNe postoji entitet sa traženim mrID!";
                            }
                        }
                        else
                        {
                            ispis += "\nNe postoji traženi tip entiteta!";
                        }
                        						
                        // Ispis svega
                        MessageBox.Show(string.Format(ispis), "Rezime", MessageBoxButtons.OK, MessageBoxIcon.None);
                    }
                    else
                    {
                        MessageBox.Show(string.Format("Extract is not valid.\n\n{0}", log), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(string.Format("An error occurred.\n\n{0}", e.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool LoadModelFromExtractFile(Stream extract, ref ConcreteModel concreteModelResult, out string errorLog)
        {
            bool valid = false;
            errorLog = string.Empty;

            System.Globalization.CultureInfo culture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            try
            {
                Assembly assembly;
                LoadAssembly(".\\" + ProfileName + ".dll", out assembly);

                if (assembly != null)
                {
                    CIMModel cimModel = new CIMModel();
                    CIMModelLoaderResult modelLoadResult = CIMModelLoader.LoadCIMXMLModel(extract, Namespace, out cimModel);
                    if (modelLoadResult.Success)
                    {
                        concreteModelResult = new ConcreteModel();
                        ConcreteModelBuilder builder = new ConcreteModelBuilder();
                        ConcreteModelBuildingResult modelBuildResult = builder.GenerateModel(cimModel, assembly, Namespace, ref concreteModelResult);

                        if (modelBuildResult.Success)
                        {
                            valid = true;
                        }
                        else
                        {
                            errorLog = modelBuildResult.Report.ToString();
                        }
                    }
                    else
                    {
                        errorLog = modelLoadResult.Report.ToString();
                    }
                }

                Thread.CurrentThread.CurrentCulture = culture;
            }
            catch (Exception e)
            {
                Thread.CurrentThread.CurrentCulture = culture;
                errorLog = e.Message;
            }

            return valid;
        }

        public static bool LoadAssembly(string path, out Assembly assembly)
        {
            try
            {
                assembly = Assembly.LoadFrom(path);//Assembly.LoadFrom(path)
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK);
                assembly = null;
                return false;
            }
            return true;
        }

        private void buttonBrowseLocation_Click(object sender, EventArgs e)
        {
            ShowOpenCIMXMLFileDialog();
        }

        private void textBoxCIMFile_DoubleClick(object sender, EventArgs e)
        {
            ShowOpenCIMXMLFileDialog();
        }

        private void buttonLoadCIM_Click(object sender, EventArgs e)
        {
            LoadCIMXMLIntoConcreteModel();
        }


    }
}
