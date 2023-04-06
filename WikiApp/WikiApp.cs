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
        // 6.16 All code is required to be adequately commented.
        // Map the programming criteria and features to your code/methods by adding comments above the method signatures.
        // Ensure your code is compliant with the CITEMS coding standards (refer http://www.citems.com.au/).
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
            // Checks if there is an input and if the name already exists
            if (!String.IsNullOrEmpty(txtBoxName.Text) && comboBoxCategory.SelectedIndex != -1 && ValidName(txtBoxName.Text))
            {
                // Create a new instance of Information to add to the List
                Information information = new Information();
                information.SetName(txtBoxName.Text);
                information.SetCategory(comboBoxCategory.SelectedItem.ToString());
                information.SetStructure(GetSelectedRadioBtn());
                information.SetDefinition(txtBoxDefinition.Text);
                // Add to the List
                wiki.Add(information);
                // Update
                UpdateList();
                // Clear
                ClearInput();
                // Reset Toolstrip
                toolStripStatusLabel1.Text = "";
            } else
            {
                toolStripStatusLabel1.Text = "Not a valid entry";
            }
        }
        // 6.4 Create a custom method to populate the ComboBox when the Form Load method is called.
        // The six categories must be read from a simple text file.
        private void PopulateComboBox()
        {
            // Populates the ComboBox from a text file
            string filePath = Path.Combine(Application.StartupPath, "categories.txt");
            // Check if the file exists
            if (File.Exists(filePath))
            {
                // Save to an array
                string[] lines = File.ReadAllLines(filePath);
                // Add to combobox
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
            // Check if the name exists
            if (wiki.Exists(x => x.GetName() == name))
            {
                // If the name exists it is not a valid name
                return false;
            } else
            {
                // ValidName() == true
                return true;
            }
        }

        //6.6 Create two methods to highlight and return the values from the Radio button GroupBox.
        //The first method must return a string value from the selected radio button (Linear or Non-Linear).
        //The second method must send an integer index which will highlight an appropriate radio button.
        private string GetSelectedRadioBtn()
        {
            // Return the text of the selected radio button
            return radioBtnLinear.Checked ? radioBtnLinear.Text : radioBtnNonLinear.Text;
        }
        private void SetSelectedRadioBtn(int index)
        {
            // Find which index is selected and select radio button
            switch (index)
            {
                case 0:
                    radioBtnLinear.Checked = true;
                    break;
                case 1:
                    radioBtnNonLinear.Checked = true;
                    break;
                // For clearing
                case 2:
                    radioBtnLinear.Checked = false;
                    radioBtnNonLinear.Checked = false;
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
            // Deletes an item at a selected index
            DialogResult confirmation = MessageBox.Show("Delete item?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            // Make sure the index has been selected and is confirmed
            if (index != -1 && confirmation == DialogResult.Yes)
            {
                wiki.RemoveAt(index);
            }
            // Sort and update
            SortList();
            UpdateList();
        }
        // 6.8 Create a button method that will save the edited record of the currently selected item in the ListView.
        // All the changes in the input controls will be written back to the list.
        // Display an updated version of the sorted list at the end of this process.
        private void EditItem()
        {
            // Create new entry to replace
            Information info = new Information();
            info.SetName(txtBoxName.Text);
            info.SetCategory(comboBoxCategory.SelectedItem.ToString());
            info.SetStructure(GetSelectedRadioBtn());
            info.SetDefinition(txtBoxDefinition.Text);
            // Replace at selected index
            wiki[index] = info;
            // Sort and update
            SortList();
            UpdateList();
        }
        // 6.9 Create a single custom method that will sort and then display the Name and Category from the wiki information in the list.
        private void SortList()
        {
            // Built-in sort method
            wiki.Sort();
        }
        // 6.10 Create a button method that will use the builtin binary search to find a Data Structure name.
        // If the record is found the associated details will populate the appropriate input controls and highlight the name in the ListView.
        // At the end of the search process the search input TextBox must be cleared.
        private void BinarySearch()
        {
            // Searches using binary search
            // Sort, update and clear before searching
            SortList();
            UpdateList();
            ClearInput();
            // Create the name to search
            Information search = new Information();
            search.SetName(txtBoxSearch.Text);
            // Search using built-in method
            index = wiki.BinarySearch(search);
            // Make sure it exists and is not negative
            if (index >= 0)
            {
                // Populate input if found
                GetEntry();
                // Highlight in listview
                lstViewData.Items[index].Selected = true;
                // Clear search box
                txtBoxSearch.Clear();
                // Found
                toolStripStatusLabel1.Text = "Found";
            } else
            {
                // Not found
                toolStripStatusLabel1.Text = "Not Found";
            }
        }
        // 6.11 Create a ListView event so a user can select a Data Structure Name from the list of Names
        // and the associated information will be displayed in the related text boxes combo box and radio button.
        private void GetEntry()
        {
            // Populate input with item at selected index
            txtBoxName.Text = wiki[index].GetName();
            comboBoxCategory.SelectedItem = wiki[index].GetCategory();
            // Find radio button to select
            // Shorten?
            if (wiki[index].GetStructure() == "Linear")
            {
                SetSelectedRadioBtn(0);
            }
            else
            {
                SetSelectedRadioBtn(1);
            }
            txtBoxDefinition.Text = wiki[index].GetDefinition();
        }
        // 6.12 Create a custom method that will clear and reset the TextBoxes, ComboBox and Radio button
        private void ClearInput()
        {
            // Clears all inputs
            txtBoxName.Text = string.Empty;
            comboBoxCategory.SelectedIndex = -1;
            SetSelectedRadioBtn(2);
            txtBoxDefinition.Text = string.Empty;

        }
        // 6.14 Create two buttons for the manual open and save option;
        // this must use a dialog box to select a file or rename a saved file.
        // All Wiki data is stored/retrieved using a binary reader/writer file format.
        private void OpenFile()
        {
            // Create new instance of OFD
            OpenFileDialog openFile = new OpenFileDialog();
            // Set directory
            openFile.InitialDirectory = Application.StartupPath;
            openFile.Title = "Open Wiki File";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                // Clear all entries before opening
                wiki.Clear();
                using (BinaryReader reader = new BinaryReader(File.Open(openFile.FileName, FileMode.Open)))
                {
                    // While there is data
                    while (reader.PeekChar() > -1)
                    {
                        // Create instance
                        Information info = new Information();
                        info.SetName(reader.ReadString());
                        info.SetCategory(reader.ReadString());
                        info.SetStructure(reader.ReadString());
                        info.SetDefinition(reader.ReadString());
                        // Save instance
                        wiki.Add(info);
                    }
                }
                // Sort and update
                SortList();
                UpdateList();
            }
        }
        private void SaveFile()
        {
            // Create instance of SFD
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.InitialDirectory = Application.StartupPath;
            saveFile.Title = "Save Wiki File";
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                using (BinaryWriter writer = new BinaryWriter(File.Open(saveFile.FileName, FileMode.Create)))
                {
                    // Loop through List and write each entry
                    foreach (Information info in wiki)
                    {
                        writer.Write(info.GetName());
                        writer.Write(info.GetCategory());
                        writer.Write(info.GetStructure());
                        writer.Write(info.GetDefinition());
                    }
                }
            }
        }
        private void UpdateList()
        {
            // Clear list view
            lstViewData.Items.Clear();
            // Loop through list and add to list view
            foreach (var item in wiki)
            {
                // Create instance
                ListViewItem lvi = new ListViewItem(item.GetName());
                lvi.SubItems.Add(item.GetCategory());
                lvi.SubItems.Add(item.GetStructure());
                lvi.SubItems.Add(item.GetDefinition());
                // Add instance
                lstViewData.Items.Add(lvi);
            }
        }
        private void GetIndex()
        {
            // Save selected index to global var
            index = lstViewData.SelectedIndices[0];
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
        private void btnSearch_Click(object sender, EventArgs e)
        {
            BinarySearch();
        }
        // 6.13 Create a double click event on the Name TextBox to clear the TextBboxes, ComboBox and Radio button.
        private void txtBoxName_DoubleClick(object sender, EventArgs e)
        {
            ClearInput();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFile();
        }
        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFile();
        }
        // 6.15 The Wiki application will save data when the form closes. 
        private void WikiApp_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveFile();
        }
        #endregion
    }
}
