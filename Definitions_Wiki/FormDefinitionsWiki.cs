using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Definitions_Wiki
{
    public partial class FormDefinitionsWiki : Form
    {
        // write definitions
        // search: make sentence case not matter and find regardless
        // duplicate error handling not required

        // BUGS
        // cannot add new entries to a loaded file without crashing: index was outside the bounds of the array

        #region SetUp

        // 9.1 Create a global 2D string array, use static variable for the dimensions (row = 12, column = 4)
        static int rowSize = 12;
        static int columnSize = 4; // name, category, structure, definition
        string[,] definitionsArray = new string[rowSize, columnSize];
        int ptr = 0; // points to next empty row
        // int counter = 0; // used once later on for adding content

        public FormDefinitionsWiki()
        {
            InitializeComponent();
        }

        private void FormDefinitionsWiki_Load(object sender, EventArgs e)
        {
            InitialiseArray();
            DisplayArray();
            Sort();
        }

        private void InitialiseArray()
        {
            //Random rnd = new Random();
            for (int i = 0; i < rowSize; i++)
            {
                //rnd.Next(10, 99).ToString()
                definitionsArray[i, 0] = "";
                definitionsArray[i, 1] = "";
                definitionsArray[i, 2] = "";
                definitionsArray[i, 3] = "";
            }
        }

        #endregion

        #region Displaying

        // 9.8 Create a display method that will show the following information in a ListView: Name and Category
        private void DisplayArray()
        {
            ListViewDisplay.Items.Clear();
            for (int i = 0; i < rowSize; i++)
            {
                ListViewItem item = new ListViewItem(definitionsArray[i, 0]); // name
                item.SubItems.Add(definitionsArray[i, 1]); // category
                ListViewDisplay.Items.Add(item);
            }
        }

        // 9.9	Create a method so the user can select a definition (Name) from the ListView and all the information is displayed in the appropriate Textboxes
        private void ListViewDisplay_MouseClick(object sender, MouseEventArgs e)
        {
            // try catch 
            int currentIndex = ListViewDisplay.SelectedIndices[0];
            if (currentIndex > -1)
            {
                TextBoxName.Text = definitionsArray[currentIndex, 0];
                TextBoxCategory.Text = definitionsArray[currentIndex, 1];
                TextBoxStructure.Text = definitionsArray[currentIndex, 2];
                TextBoxDefinition.Text = definitionsArray[currentIndex, 3];
            }
        }

        #endregion

        #region SortSwapSearch

        // 9.6	Write the code for a Bubble Sort method to sort the 2D array by Name ascending,
        // ensure you use a separate swap method that passes the array element to be swapped (do not use any built-in array methods)
        private void Sort()
        {
            for (int i = 0; i < rowSize; i++)
            {
                for (int j = 0; j < rowSize - 1; j++)
                {
                    if (string.Compare(definitionsArray[j + 1, 0], definitionsArray[j, 0]) < 0)
                    {
                        Swap(j);
                    }
                }
            }
            DisplayArray();
            statusStripResponse.Text = "Sorted";
        }

        private void Swap(int indx)
        {
            string temp;
            for (int x = 0; x < columnSize; x++)
            {
                temp = definitionsArray[indx, x];
                definitionsArray[indx, x] = definitionsArray[indx + 1, x];
                definitionsArray[indx + 1, x] = temp;
            }
        }

        // 9.7 Write the code for a Binary Search for the Name in the 2D array and display the information in the other textboxes when found,
        // add suitable feedback if the search in not successful and clear the search textbox (do not use any built-in array methods)
        private void ButtonSearchByName_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TextBoxName.Text))
            {
                int startIndx = -1;
                int finalIndx = rowSize;
                int foundIndx = -1;
                bool flag = false;

                while (!flag && !((finalIndx - startIndx) <= 1))
                {
                    int newIndx = (finalIndx + startIndx) / 2;
                    if (string.Compare(definitionsArray[newIndx, 0], TextBoxName.Text) == 0)
                    {
                        foundIndx = newIndx;
                        flag = true;
                        break;
                    }
                    else
                    {
                        if (string.Compare(definitionsArray[newIndx, 0], TextBoxName.Text) == 1)
                        {
                            finalIndx = newIndx;
                        }
                        else
                        {
                            startIndx = newIndx;
                        }
                    }
                }
                if (flag)
                {
                    TextBoxName.Text = definitionsArray[foundIndx, 0];
                    TextBoxCategory.Text = definitionsArray[foundIndx, 1];
                    TextBoxStructure.Text = definitionsArray[foundIndx, 2];
                    TextBoxDefinition.Text = definitionsArray[foundIndx, 3];

                    ListViewDisplay.SelectedItems.Clear();
                    ListViewDisplay.Items[foundIndx].Selected = true;
                    ListViewDisplay.Select();

                    statusStripResponse.Text = "Search Found";
                }
                else
                {
                    statusStripResponse.Text = "Search Not Found";
                    ClearTextBoxes();
                }
            }
            else
            {
                statusStripResponse.Text = "Please Enter Data Into The Name Textbox";
            }
        }

        #endregion

        #region AddEditDelete

        // 9.2 Create an ADD button that will store the information from the 4 text boxes into the 2D array
        // Broken. Fix
        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            // counter = 0
            // is array row empty? yes - add text, no - check next line
            // if loop through entire thing with no empty line, display error 

            if (!string.IsNullOrEmpty(TextBoxName.Text) && !string.IsNullOrEmpty(TextBoxCategory.Text) && !string.IsNullOrEmpty(TextBoxStructure.Text) && !string.IsNullOrEmpty(TextBoxDefinition.Text)) // if none of the boxes are empty
            {
                for (int counter = 0; counter < rowSize; counter++) // loop through all lines
                {
                    if (definitionsArray[counter, 0] == "") // if empty
                    {
                        definitionsArray[counter, 0] = TextBoxName.Text;
                        definitionsArray[counter, 1] = TextBoxCategory.Text;
                        definitionsArray[counter, 2] = TextBoxStructure.Text;
                        definitionsArray[counter, 3] = TextBoxDefinition.Text;

                        statusStripResponse.Text = "Successfully Added";
                        ClearTextBoxes();
                        DisplayArray();
                        Sort();
                    }
                    else if (counter >= 12) // says at end of for loop anyway 
                    {
                        statusStripResponse.Text = "Array is Full. Please Delete or Edit an Entry to Change Content.";
                    }
                    /*
                    else if(counter >= 12) { // THIS DOES NOT WORK
                        statusStripResponse.Text = "Array is Full. Please Delete or Edit an Entry to Change Content.";
                    }*/
                }
            }
            else
            {
                statusStripResponse.Text = "Please Enter Data Into All Text Fields Before Adding"; // works
            }
            /*
                if (!string.IsNullOrEmpty(TextBoxName.Text) && !string.IsNullOrEmpty(TextBoxCategory.Text) && !string.IsNullOrEmpty(TextBoxStructure.Text) && !string.IsNullOrEmpty(TextBoxDefinition.Text)) // if none of the boxes are empty
            {
                definitionsArray[ptr, 0] = TextBoxName.Text;
                definitionsArray[ptr, 1] = TextBoxCategory.Text;
                definitionsArray[ptr, 2] = TextBoxStructure.Text;
                definitionsArray[ptr, 3] = TextBoxDefinition.Text;

                ptr++;

                ClearTextBoxes();
                DisplayArray();
                Sort();
            }
            else
            {
                statusStripResponse.Text = "Please Enter Data Into All Text Fields Before Adding";
            }

            /*
                if (counter < rowSize)
                {
                    if (!string.IsNullOrEmpty(TextBoxName.Text) && !string.IsNullOrEmpty(TextBoxCategory.Text) && !string.IsNullOrEmpty(TextBoxStructure.Text) && !string.IsNullOrEmpty(TextBoxDefinition.Text)) // if none of the boxes are empty
                    {
                        // ptr is at 12 so out of range no matter how many you delete
                        definitionsArray[ptr, 0] = TextBoxName.Text;
                        definitionsArray[ptr, 1] = TextBoxCategory.Text;
                        definitionsArray[ptr, 2] = TextBoxStructure.Text;
                        definitionsArray[ptr, 3] = TextBoxDefinition.Text;

                        counter++;

                        ClearTextBoxes();
                        DisplayArray();
                        Sort();
                    }
                    else
                    {
                        statusStripResponse.Text = "Please Enter Data Into All Text Fields Before Adding";
                    }
                }
                else
                {
                    statusStripResponse.Text = "Array is Full. Please Delete or Edit an Entry to Change Content.";
                }
            */

        }

        // 9.3  Create an EDIT button that will allow the user to modify any information from the 4 text boxes into the 2D array
        // on edit, clear boxes and focus cursor on name box
        private void ButtonEdit_Click(object sender, EventArgs e)
        {
            int currentIndex = ListViewDisplay.SelectedIndices[0];
            if (currentIndex > -1)
            {
                definitionsArray[currentIndex, 0] = TextBoxName.Text;
                definitionsArray[currentIndex, 1] = TextBoxCategory.Text;
                definitionsArray[currentIndex, 2] = TextBoxStructure.Text;
                definitionsArray[currentIndex, 3] = TextBoxDefinition.Text;
            }

            Sort();
            DisplayArray();
        }

        // 9.4 Create a DELETE button that removes all the information from a single entry of the array; the user must be prompted before the final deletion occurs
        private void ButtonDelete_Click(object sender, EventArgs e)
        {
            int currentIndex = ListViewDisplay.SelectedIndices[0];
            if (currentIndex > -1)
            {
                string message = "Are You Sure You Want This Delete This Entry?\nThis Action Is Permanent.";
                string title = "Delete Definition";
                MessageBoxButtons buttons = MessageBoxButtons.OKCancel;
                DialogResult result;

                result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Warning);
                if (result == DialogResult.OK)
                {
                    definitionsArray[currentIndex, 0] = "";
                    definitionsArray[currentIndex, 1] = "";
                    definitionsArray[currentIndex, 2] = "";
                    definitionsArray[currentIndex, 3] = "";

                    ClearTextBoxes();
                    Sort();
                    DisplayArray();
                }
            }
        }

        #endregion

        #region Clear
        // 9.5 Create a CLEAR method to clear the four text boxes so a new definition can be added
        private void TextBoxName_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ClearTextBoxes();
        }

        private void ClearTextBoxes()
        {
            TextBoxName.Clear();
            TextBoxCategory.Clear();
            TextBoxStructure.Clear();
            TextBoxDefinition.Clear();
            TextBoxName.Focus();
        }

        #endregion

        #region LoadSave

        // 9.10	Create a SAVE button so the information from the 2D array can be written into a binary file called definitions.dat which is sorted by Name,
        // ensure the user has the option to select an alternative file. Use a file stream and BinaryWriter to create the file.

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "dat files (*.dat) | *.dat";
            saveFileDialog.Title = "Save Dictionary";
            saveFileDialog.InitialDirectory = Application.StartupPath;
            saveFileDialog.DefaultExt = "dat";
            saveFileDialog.ShowDialog();
            string fileName = saveFileDialog.FileName;
            if(saveFileDialog.FileName != "") 
            {
                // save as that
                SaveRecord(fileName);
            }
            else
            {
                // use default name
                SaveRecord("definitions.dat"); // does save but clicking save wont close the viewer which is required for it to save
            }
        }

        private void SaveRecord(string saveFileName)
        {
            try
            {
                using (var stream = File.Open(saveFileName, FileMode.Create))
                {
                    using (var writer = new BinaryWriter(stream, Encoding.UTF8, false))
                    {
                        for(int x = 0; x < rowSize; x++)
                        {
                            for(int y = 0; y < columnSize; y++)
                            {
                                writer.Write(definitionsArray[x, y]);
                            }
                        }
                    }
                }
                
            }
            catch(IOException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // 9.11	Create a LOAD button that will read the information from a binary file called definitions.dat into the 2D array,
        // ensure the user has the option to select an alternative file. Use a file stream and BinaryReader to complete this task.

        private void ButtonLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Application.StartupPath;
            openFileDialog.Filter = "dat files (*.dat)| *.dat";
            openFileDialog.Title = "Open File";

            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // open file
                OpenRecord(openFileDialog.FileName);
            }
            // else
        }

        private void OpenRecord(string openFileName)
        {
            if(File.Exists(openFileName))
            {
                using (var stream = File.Open(openFileName, FileMode.Open))
                {
                    using (var reader = new BinaryReader(stream, Encoding.UTF8, false))
                    {
                        ptr = 0;
                        Array.Clear(definitionsArray, 0, rowSize);
                        while(stream.Position < stream.Length)
                        {
                            for(int y = 0; y < columnSize; y++)
                            {
                                definitionsArray[ptr, y] = reader.ReadString();
                            }
                            ptr++;
                        }
                    }
                }
            }

            DisplayArray();
        }

        #endregion

    }
}