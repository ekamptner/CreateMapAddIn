using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Display;

// *********************
//      Author: Erika Kamptner
//      Created Date: 3/5/2017
//      Description: This form field stores input parameters and passes to the LoadLayers.cs file. //
// **************************
namespace CreateMap
{
    public partial class MapForm : Form
    {
        string m_pBUILDINGSDE;
        string m_pCSCLSDE;
        string m_pDOFTAXMAPSDE;
        bool isCSCL;
        bool isCustom;
        bool isAddressPoint;
        bool isCenterline;
        bool isBuilding;
        bool isTaxLot;
        bool isSave;

        public MapForm()
        {
            InitializeComponent();
        }

        //store input values to modular parameters
        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            m_pBUILDINGSDE = textBox9.Text;
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            m_pCSCLSDE = textBox8.Text;
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            m_pDOFTAXMAPSDE = textBox7.Text;
        }

        private void checkBox1_CheckStateChanged(object sender, EventArgs e)
        {
            checkBox2.Enabled = (checkBox1.CheckState != CheckState.Checked);
            checkBox3.Enabled = (checkBox1.CheckState != CheckState.Checked);
            checkBox4.Enabled = (checkBox1.CheckState != CheckState.Checked);
            checkBox5.Enabled = (checkBox1.CheckState != CheckState.Checked);
            checkBox6.Enabled = (checkBox1.CheckState != CheckState.Checked);

            isCSCL = checkBox1.Checked;

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            isCustom = checkBox2.Checked;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
                isAddressPoint = checkBox4.Checked;

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
                isCenterline = checkBox3.Checked;
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
                isBuilding = checkBox6.Checked;
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
                isTaxLot = checkBox5.Checked;
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
                isSave = checkBox7.Checked;
        }

        //On click, verify valid inputs are provided and send parameters to LoadLayers.cs
        private void button1_Click(object sender, EventArgs e)
        {
            if (m_pBUILDINGSDE == null || m_pDOFTAXMAPSDE == null || m_pCSCLSDE == null)
            {
                MessageBox.Show("Folder required for all connections.", "No folder provided", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //Verify that map type is selected
            else if ((isCSCL && isCustom) || (!(isCSCL) && !(isCustom)))
            {
                MessageBox.Show("Select only one map type.", "Map Type Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
            }
            //Verify that at least one layer is selected (if custom)
            else if ((isCustom) && (!(isAddressPoint) && !(isBuilding) && !(isCenterline) && !(isTaxLot)))
            {
                MessageBox.Show("Select at least one map layer to load into map", "Custom Map Type Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
            }

            else 
            {
                //Create Connection file (removed from final project)
                //SDEConnection.SDEWorkspace(server, instance, user, password, database, version);

                //Load layers using input folders/SDE connections
                LoadLayers.BuildMap(m_pBUILDINGSDE, m_pCSCLSDE, m_pDOFTAXMAPSDE, isCSCL, isCustom,
                                                    isAddressPoint, isCenterline, isBuilding, isTaxLot, isSave);
                this.Close();
            }
            

        }


    }
}
