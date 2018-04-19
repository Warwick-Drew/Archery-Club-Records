using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Archery_Club_Records_System_v1._1
{
    public partial class Main_Screen : Form
    {
        private DataTable dataTable = new DataTable();
        public String previousArcherRecord;
        public String previousSeasonRecord;

        public Main_Screen()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'archery_dbDataSet11.Round' table. You can move, or remove it, as needed.
            this.roundTableAdapter.Fill(this.archery_dbDataSet11.Round);
            // TODO: This line of code loads data into the 'archery_dbDataSet11.scores' table. You can move, or remove it, as needed.
            this.scoresTableAdapter1.Fill(this.archery_dbDataSet11.scores);
            // TODO: This line of code loads data into the 'archery_dbDataSet.scores' table. You can move, or remove it, as needed.
            this.scoresTableAdapter.Fill(this.archery_dbDataSet.scores);
            // TODO: This line of code loads data into the 'archery_dbDataSet.archer' table. You can move, or remove it, as needed.
            this.archerTableAdapter.Fill(this.archery_dbDataSet.archer);
            // TODO: This line of code loads data into the 'archery_dbDataSet.season' table. You can move, or remove it, as needed.
            this.seasonTableAdapter.Fill(this.archery_dbDataSet.season);

            fillRoundComboBox();
           

            setMainScreen(cbArcher.Text, comboBox1.SelectedValue.ToString()); //Set the fields on the screen and the data table
            previousArcherRecord = cbArcher.Text;
            previousSeasonRecord = comboBox1.SelectedValue.ToString();
        }

        private void fillRoundComboBox()
        {
            cbRound.Items.Insert(0, "None");

            SqlConnection conn = new SqlConnection("Data Source=DELL\\SQLEXPRESS;Initial Catalog=archery_db;Integrated Security=True");

            conn.Open();
            try
            {
                int count = 1;
                SqlCommand cmd = new SqlCommand("Select * from dbo.round", conn);
                cmd.ExecuteNonQuery();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    cbRound.Items.Insert(count, Convert.ToString(dr["roundName"]));
                }
                conn.Close();
                cbRound.SelectedIndex = 0;
            }
            catch (Exception ex3)
            {
                //MessageBox.Show(ex3.Message);
            }

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit(); // close the application
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                setMainScreen(cbArcher.Text, comboBox1.SelectedValue.ToString()); //Set the fields on the screen and the data table to the new selection
                previousArcherRecord = cbArcher.Text;
                previousSeasonRecord = comboBox1.SelectedValue.ToString();
            }
            catch (Exception ex2)
            {
                //MessageBox.Show(ex2.Message);
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            btnCancel.Enabled = true;
            btnSave.Enabled = true;
            tbArcherID.Text  = "";
            tbFirstName.Text = "";
            tbSurname.Text   = "";
            tbDOB.Text       = "";
            tbAdd1.Text      = "";
            tbAdd2.Text      = "";
            tbAdd3.Text      = "";
            tbAdd4.Text      = "";
            tbPostcode.Text  = "";
            tbCurCls.Text = "";
            tbCurHcp.Text = "";
            tbAGBNo.Text = "";
            cbInactive.Checked = false;
            btnNew.Visible = false;


            tbArcherID.Enabled = true;

        }

        private void fillByToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.archerTableAdapter.FillBy(this.archery_dbDataSet.archer);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            setMainScreen(previousArcherRecord, previousSeasonRecord); //Set the fields on the screen and the data table to the new selection
            btnNew.Visible = true;
            btnCancel.Enabled = false;
            btnSave.Enabled = false;
        }

        private void setMainScreen(string selarch, string selSeas)
        {
            SqlConnection conn = new SqlConnection("Data Source=DELL\\SQLEXPRESS;Initial Catalog=archery_db;Integrated Security=True");

            conn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("Select * from dbo.archer where archerID = " + cbArcher.Text, conn);
                cmd.ExecuteNonQuery();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    tbArcherID.Text = Convert.ToString(dr["archerId"]);
                    tbFirstName.Text = Convert.ToString(dr["FirstName"]);
                    tbSurname.Text = Convert.ToString(dr["Surname"]);
                    tbDOB.Text = Convert.ToString(dr["DOB"]);
                    tbAdd1.Text = Convert.ToString(dr["address1"]);
                    tbAdd2.Text = Convert.ToString(dr["address2"]);
                    tbAdd3.Text = Convert.ToString(dr["address3"]);
                    tbAdd4.Text = Convert.ToString(dr["address4"]);
                    tbPostcode.Text = Convert.ToString(dr["postcode"]);
                    tbCurCls.Text = Convert.ToString(dr["currClss"]);
                    tbCurHcp.Text = Convert.ToString(dr["currHcp"]);
                    tbAGBNo.Text = Convert.ToString(dr["AGBNo"]);
                    cbInactive.Checked = Convert.ToBoolean(dr["Inactive"]);
                }
                conn.Close();

                if (cbInactive.Checked)
                {
                    btDelete.Text = "Activate";
                }
                else
                {
                    btDelete.Text = "Inactivate";
                }

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }

            try
            {
                string selectCommand = "SELECT scores.scoreID, round.roundName, scores.DateShot, scores.hits, scores.golds, scores.score FROM scores INNER JOIN season ON scores.seasonid = season.seasonID INNER JOIN archer ON scores.archerid = archer.archerID INNER JOIN round ON scores.roundid = round.roundID WHERE(archer.archerID = " + selarch + ") AND (scores.seasonid = " + selSeas + ")";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(selectCommand, conn);
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
                DataTable table = new DataTable();
                table.Locale = System.Globalization.CultureInfo.InvariantCulture;
                dataAdapter.Fill(table);
                dataGridView1.DataSource = table;
                dataGridView1.AutoResizeColumns(
                    DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (SqlException sqle)
            {
                // MessageBox.Show(sqle.Message);
            }
        }

        private void btnLogOn_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection("Data Source=DELL\\SQLEXPRESS;Initial Catalog=archery_db;Integrated Security=True");

            conn.Open();

            try
            {
                SqlCommand cmd = new SqlCommand("Select * from dbo.Admin where Username = '" + tbUser.Text + "'", conn);
                cmd.ExecuteNonQuery();
                SqlDataReader dr = cmd.ExecuteReader();


                while (dr.Read())
                {

                    string user = Convert.ToString(dr["Username"]);
                    string pass = Convert.ToString(dr["Password"]);

                    if ((user == tbUser.Text) && (pass == tbPassword.Text))
                    {
                        tabControl1.Visible = true;
                        groupBox4.Visible = false;
                        label1.Visible = false;
                        label2.Visible = false;
                        tbUser.Visible = false;
                        tbPassword.Visible = false;
                        btnLogOn.Visible = false;
                        btExit.Visible = false;
                        LogOnFail.Visible = false;
                        setMainScreen("1", "1");

                    }
                    else
                    {
                        LogOnFail.Visible = true;
                    }
                }
                conn.Close();

            }
            catch (Exception ex)
            {
               // MessageBox.Show(ex.Message);
            }

        }

        private void btExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            btnNew.Visible = true;
        }

    }
}
