using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Data;

namespace DataAccess
{
    public partial class ConnectionExample : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Makes grid view not bind data and wipe out input user put in
            // Also put previous code from last tutorial in this new method
            if (!Page.IsPostBack)
            {
                UnsortedListData();
                BindDataToGridView();
            }
                
        }

        public void UnsortedListData()
        {
            // Pass in connection string from web config file using name
            var CRCCon = WebConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
            // Make connection with SQL database
            using (SqlConnection db = new SqlConnection(CRCCon))
            {
                // If successful, then give success, if failed give error, and always close and dispose connection to release all resources used
                try
                {
                    db.Open();
                    ltConnectionMessage.Text = "Connection Successful!";

                    // Try/catch for SQL query print out in unordered list
                    try
                    {
                        // Used to create SQL queries and read from db
                        SqlCommand command = new SqlCommand("select * from SDI_TEMP_PULL with (nolock) where PickQty > 20 order by itemlookupcode, storecode", db);
                        // Just read the data a row at a time
                        SqlDataReader reader = command.ExecuteReader();
                        // If there are results from the query, while there are items to read put into output
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                // GetString(#) = what column the item info is in
                                // GetString(3) = item description for this query
                                ltOutput.Text += string.Format("<li>{0}</li>", reader.GetString(3));
                            }
                        }

                    }
                    catch (SqlException ex)
                    {
                        ltOutput.Text = "<li> Query failed: " + ex.Message + "</li>";
                    }
                }
                catch (SqlException ex)
                {
                    ltConnectionMessage.Text = "Connection failed: " + ex.Message;
                }
                finally
                {
                    db.Close();
                    db.Dispose();
                }
            }
        }

        public void BindDataToGridView()
        {
            // Pass in connection string from web config file using name
            var CRCCon = WebConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
            // Make connection with SQL database
            using (SqlConnection db = new SqlConnection(CRCCon))
            {
                // If successful, then give success, if failed give error, and always close and dispose connection to release all resources used
                try
                {
                    db.Open();
                    ltConnectionMessage.Text = "Connection Successful!";

                    // Try/catch for SQL query print out in unordered list
                    try
                    {
                        // Used to create SQL queries and read from db
                        SqlCommand command = new SqlCommand("select * from SDI_TEMP_PULL with (nolock) where PickQty > 20 order by itemlookupcode, storecode", db);

                        // Use for grid view, fill data from dataset into rows
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            gvData.DataSource = ds;
                            gvData.DataBind();
                        }

                    }
                    catch (SqlException ex)
                    {
                        ltOutput.Text = "<li> Query failed: " + ex.Message + "</li>";
                    }
                }
                catch (SqlException ex)
                {
                    ltConnectionMessage.Text = "Connection failed: " + ex.Message;
                }
                finally
                {
                    db.Close();
                    db.Dispose();
                }
            }
        }

        protected void gvData_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Get the row of data for deleting and get the hidden field by the control id name to find correct data item
            // Only need item ID, not the rest of the info
            ltError.Text = string.Empty;
            GridViewRow gvRow = (GridViewRow)gvData.Rows[e.RowIndex];
            HiddenField hdnItemId = (HiddenField)gvRow.FindControl("hdnItemId");

            // Connect to SQL server
            var CRCCon = WebConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
            using (SqlConnection db = new SqlConnection(CRCCon))
            {
                // If successful, then give success, if failed give error, and always close and dispose connection to release all resources used
                try
                {
                    db.Open();
                    string query = string.Format("DELETE FROM SDI_TEMP_PULL WHERE itemlookupcode={0}", hdnItemId.Value);
                    SqlCommand command = new SqlCommand(query, db);
                    // Not expecting result set so execute non query
                    command.ExecuteNonQuery();
                    // Set to -1 to show not editing anymore
                    gvData.EditIndex = -1;
                    BindDataToGridView();

                }
                catch (Exception ex)
                {
                    ltError.Text = ex.Message;
                }
                finally
                {
                    db.Close();
                    db.Dispose();
                }
            }
        }

        protected void gvData_RowEditing(object sender, GridViewEditEventArgs e)
        {
            // Clear error and index of row is passed so know what row is edited
            ltError.Text = string.Empty;
            gvData.EditIndex = e.NewEditIndex;
            BindDataToGridView();
        }

        protected void gvData_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            // Get the row of data for updating and get the hidden field by the control id name, get textboxes from the cells (column number) which includes hidden columns
            // Cells[0] = hidden id field
            ltError.Text = string.Empty;
            GridViewRow gvRow = (GridViewRow)gvData.Rows[e.RowIndex];
            HiddenField hdnItemId = (HiddenField)gvRow.FindControl("hdnItemId");
            TextBox txtName = (TextBox)gvRow.Cells[1].Controls[0];
            TextBox txtStore = (TextBox)gvRow.Cells[2].Controls[0];
            TextBox txtPickQty = (TextBox)gvRow.Cells[3].Controls[0];

            // Connect to SQL server
            var CRCCon = WebConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
            using (SqlConnection db = new SqlConnection(CRCCon))
            {
                // If successful, then give success, if failed give error, and always close and dispose connection to release all resources used
                try
                {
                    db.Open();
                    string query = string.Format("UPDATE SDI_TEMP_PULL set itemDescription='{0}', storecode='{1}', pickQty='{2}' WHERE itemlookupcode={3}", txtName.Text, txtStore.Text, txtPickQty.Text, hdnItemId.Value);
                    SqlCommand command = new SqlCommand(query, db);
                    // Not expecting result set so execute non query
                    command.ExecuteNonQuery();
                    // Set to -1 to show not editing anymore
                    gvData.EditIndex = -1;
                    BindDataToGridView();

                }
                catch (Exception ex)
                {
                    ltError.Text = ex.Message;
                }
                finally
                {
                    db.Close();
                    db.Dispose();
                }
            }
        }

        protected void gvData_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            // Show not editing anymore and refresh grid view
            gvData.EditIndex = -1;
            BindDataToGridView();
        }

        protected void btnAddRow_Click(object sender, EventArgs e)
        {
            // Connect to SQL server
            var CRCCon = WebConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
            using (SqlConnection db = new SqlConnection(CRCCon))
            {
                // If successful, then give success, if failed give error, and always close and dispose connection to release all resources used
                try
                {
                    db.Open();
                    SqlCommand command = new SqlCommand("INSERT INTO SDI_TEMP_PULL (itemDescription, storecode, pickQty) values (' ', ' ', ' ' )", db);
                    // Not expecting result set so execute non query
                    command.ExecuteNonQuery();
                    BindDataToGridView();

                }
                catch (Exception ex)
                {
                    ltError.Text = ex.Message;
                }
                finally
                {
                    db.Close();
                    db.Dispose();
                }
            }
        }
    }
}