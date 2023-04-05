using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


/// Mauriza Arianne
/// WikiApp
/// Displays data structures
/// 22/03/2023



namespace WikiApp
{
    public partial class WikiApp : Form
    {
        public WikiApp()
        {
            InitializeComponent();
            PopulateComboBox();
        }

        #region Variables
        //6.2 Create a global List<T> of type Information called Wiki
        private List<Information> wiki = new List<Information>();
        private int index = -1;
        #endregion



        #region Methods
        // 6.3 Create a button method to ADD a new item to the list.
        // Use a TextBox for the Name input, ComboBox for the Category, Radio group for the Structure and Multiline TextBox for the Definition.
        private void AddItem()
        {
            // Adds a new item to the list
            Information information = new Information();
            information.SetName(txtBoxName.Text);
            information.SetCategory(comboBoxCategory.SelectedItem.ToString());
            information.SetStructure(GetSelectedRadioBtn());
            information.SetDefinition(txtBoxDefinition.Text);

            wiki.Add(information);

            UpdateList();
        }
        // 6.4 Create a custom method to populate the ComboBox when the Form Load method is called.
        // The six categories must be read from a simple text file.
        private void PopulateComboBox()
        {
            // Populates the ComboBox from a text file
            string filePath = Path.Combine(Application.StartupPath, "categories.txt");

            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                comboBoxCategory.Items.AddRange(lines);
            }
            else
            {
                toolStripStatusLabel1.Text = "The file does not exist.";
            }
        }
        // 6.5 Create a custom ValidName method which will take a parameter string value from the Textbox Name and returns a Boolean after checking for duplicates.
        // Use the built in List<T> method “Exists” to answer this requirement.
        private bool ValidName(string name)
        {
            if (wiki.Exists(x => x.GetName() == name))
            {
                return true;
            } else
            {
                return false;
            }
        }

        //6.6 Create two methods to highlight and return the values from the Radio button GroupBox.
        //The first method must return a string value from the selected radio button (Linear or Non-Linear).
        //The second method must send an integer index which will highlight an appropriate radio button.
        private string GetSelectedRadioBtn()
        {
            if (radioBtnLinear.Checked) 
            { 
                return radioBtnLinear.Text; 
            } 
            else 
            { 
                return radioBtnNonLinear.Text; 
            }
        }
        private void SetSelectedRadioBtn(int index)
        {
            switch (index)
            {
                case 0:
                    radioBtnLinear.Checked = true;
                    break;
                case 1:
                    radioBtnNonLinear.Checked = true;
                    break;
                default:
                    break;
            }
        }
        // 6.7 Create a button method that will delete the currently selected record in the ListView.
        // Ensure the user has the option to backout of this action by using a dialog box.
        // Display an updated version of the sorted list at the end of this process.
        private void DeleteItem()
        {
            DialogResult confirmation = MessageBox.Show("Delete item?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (index != -1 && confirmation == DialogResult.Yes)
            {
                wiki.RemoveAt(index);
            }
            SortList();
            UpdateList();
        }
        // 6.8 Create a button method that will save the edited record of the currently selected item in the ListView.
        // All the changes in the input controls will be written back to the list.
        // Display an updated version of the sorted list at the end of this process.
        private void EditItem()
        {
            Information info = new Information();
            info.SetName(txtBoxName.Text);
            info.SetCategory(comboBoxCategory.SelectedItem.ToString());
            info.SetStructure(GetSelectedRadioBtn());
            info.SetDefinition(txtBoxDefinition.Text);

            wiki[index] = info;
            SortList();
            UpdateList();
        }
        // 6.9 Create a single custom method that will sort and then display the Name and Category from the wiki information in the list.
        private void SortList()
        {
            wiki.Sort();
        }
        // 6.10 Create a button method that will use the builtin binary search to find a Data Structure name.
        // If the record is found the associated details will populate the appropriate input controls and highlight the name in the ListView.
        // At the end of the search process the search input TextBox must be cleared.
        private void BinarySearch()
        {
            //
            // TODO
            //
            // Highlight stuff
            // Search box cleared
            Information search = new Information();
            search.SetName(txtBoxSearch.Text);
            int foundIndex = wiki.BinarySearch(search);
            if (foundIndex != -1)
            {
                txtBoxName.Text = wiki[foundIndex].GetName();
                comboBoxCategory.SelectedItem = wiki[foundIndex].GetCategory();
                if (wiki[foundIndex].GetStructure() == "Linear")
                {
                    SetSelectedRadioBtn(0);
                } else
                {
                    SetSelectedRadioBtn(1);
                }
                txtBoxDefinition.Text = wiki[foundIndex].GetDefinition();
            }
        }

        private void UpdateList()
        {
            lstViewData.Items.Clear();

            foreach (var item in wiki)
            {
                ListViewItem lvi = new ListViewItem(item.GetName());
                lvi.SubItems.Add(item.GetCategory());
                lvi.SubItems.Add(item.GetStructure());
                lvi.SubItems.Add(item.GetDefinition());

                lstViewData.Items.Add(lvi);
            }
        }
        private void GetIndex()
        {
            index = lstViewData.SelectedIndices[0];
        }
        private void GetEntry()
        {

        }
        #endregion

        #region Events
        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddItem();
        }
        private void lstViewData_Click(object sender, EventArgs e)
        {
            GetIndex();
            GetEntry();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteItem();
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            EditItem();
        }

        #endregion

    }
}
