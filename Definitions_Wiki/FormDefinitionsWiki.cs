using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;

// Milo Kester 30063179
// 2024-03-13
// Definitions Wiki
// AT1 - Develop a fully functional wiki application

namespace Definitions_Wiki
{
    public partial class FormDefinitionsWiki : Form
    {
        #region SetUp

        // 9.1 Create a global 2D string array, use static variable for the dimensions (row = 12, column = 4)
        static int rowSize = 12;
        static int columnSize = 4; // name, category, structure, definition
        string[,] definitionsArray = new string[rowSize, columnSize];
        int ptr = 0; // points to next empty row
        TextInfo myTI = new CultureInfo("en-AU", false).TextInfo; // for making text title case

        public FormDefinitionsWiki()
        {
            InitializeComponent();
        }

        private void FormDefinitionsWiki_Load(object sender, EventArgs e)
        {
            // initalise, display, and sort
            InitialiseArray();
            DisplayArray();
            Sort();
        }

        private void InitialiseArray()
        {
            // for loop through array on itialisation and fill with placeholders
            for (int i = 0; i < rowSize; i++)
            {
                definitionsArray[i, 0] = "~";
                definitionsArray[i, 1] = "~";
                definitionsArray[i, 2] = "~";
                definitionsArray[i, 3] = "~";
            }
        }

        #endregion

        #region Displaying

        // 9.8 Create a display method that will show the following information in a ListView: Name and Category
        private void DisplayArray()
        {

            ListViewDisplay.Items.Clear(); // clear display of everything
            for (int i = 0; i < rowSize; i++) // loop through entire array
            {
                ListViewItem item = new ListViewItem(definitionsArray[i, 0]); // name
                item.SubItems.Add(definitionsArray[i, 1]); // category
                ListViewDisplay.Items.Add(item);
            }
        }

        // 9.9	Create a method so the user can select a definition (Name) from the ListView and all the information is displayed in the appropriate Textboxes
        private void ListViewDisplay_MouseClick(object sender, MouseEventArgs e)
        {
            int currentIndex = ListViewDisplay.SelectedIndices[0]; // get current index from mouse click
            try {
                if (currentIndex > -1) // if valid index
                {
                    // display in boxes
                    TextBoxName.Text = definitionsArray[currentIndex, 0]; // 2nd index is for different columns (name, category, structure, definition)
                    TextBoxCategory.Text = definitionsArray[currentIndex, 1];
                    TextBoxStructure.Text = definitionsArray[currentIndex, 2];
                    TextBoxDefinition.Text = definitionsArray[currentIndex, 3];
                }
            }
            catch
            {
                statusStripResponse.Text = "Error";
            }
            
        }

        #endregion

        #region SortSwapSearch

        // 9.6	Write the code for a Bubble Sort method to sort the 2D array by Name ascending,
        // ensure you use a separate swap method that passes the array element to be swapped (do not use any built-in array methods)
        private void Sort()
        {
            // loop through entire array
            for (int i = 0; i < rowSize; i++)
            {
                for (int j = 0; j < rowSize - 1; j++)
                {
                    if (string.CompareOrdinal(definitionsArray[j + 1, 0], definitionsArray[j, 0]) < 0)
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
            string temp; // temporary string to swap to
            for (int x = 0; x < columnSize; x++) // for loop to get all columns
            {
                temp = definitionsArray[indx, x]; // put row into temporary
                definitionsArray[indx, x] = definitionsArray[indx + 1, x]; // swap row into new row
                definitionsArray[indx + 1, x] = temp; // put temporary back into row
            }
        }

        // 9.7 Write the code for a Binary Search for the Name in the 2D array and display the information in the other textboxes when found,
        // add suitable feedback if the search in not successful and clear the search textbox (do not use any built-in array methods)
        private void ButtonSearchByName_Click(object sender, EventArgs e)
        {
            int minIndx = 0;
            int maxIndx = rowSize - 1;
            int midIndx = 0;
            bool flag = false; // true if found

            string capitalText = myTI.ToTitleCase(TextBoxName.Text); // make search title case to match

            while (!flag && minIndx <= maxIndx)
            {
                midIndx = (minIndx + maxIndx) / 2;
                int compareVal = string.Compare(definitionsArray[midIndx, 0], capitalText);
                
                if (compareVal == 0) // match
                {
                    flag = true;
                    break;
                }
                else if (definitionsArray[midIndx, 0] == "~" || compareVal == 1) // search term is smaller than found
                {
                    maxIndx = midIndx - 1;
                }
                else { // -1 // search term is bigger than found
                    minIndx = midIndx + 1;
                }
            }
            if (flag) // if flag == true
            {
                statusStripResponse.Text = capitalText + " Found at Index: " + midIndx;
                TextBoxName.Text = definitionsArray[midIndx, 0];
                TextBoxCategory.Text = definitionsArray[midIndx, 1];
                TextBoxStructure.Text = definitionsArray[midIndx, 2];
                TextBoxDefinition.Text = definitionsArray[midIndx, 3];

                ListViewDisplay.SelectedItems.Clear();
                ListViewDisplay.Items[midIndx].Selected = true;
                ListViewDisplay.Select();
            }
            else
            {
                statusStripResponse.Text = "Search Not Found";
            }
        }

        #endregion

        #region AddEditDelete

        // 9.2 Create an ADD button that will store the information from the 4 text boxes into the 2D array
        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            // checks if inputs are all filled
            if (!string.IsNullOrEmpty(TextBoxName.Text) && !string.IsNullOrEmpty(TextBoxCategory.Text) && !string.IsNullOrEmpty(TextBoxStructure.Text) && !string.IsNullOrEmpty(TextBoxDefinition.Text)) {
                int counter = 0;
                // loops through all rows
                while (counter < rowSize) {
                    // if finds one thats empty (has ~ symbol)
                    if (definitionsArray[counter, 0] == "~") {

                        definitionsArray[counter, 0] = myTI.ToTitleCase(TextBoxName.Text);
                        definitionsArray[counter, 1] = myTI.ToTitleCase(TextBoxCategory.Text);
                        definitionsArray[counter, 2] = myTI.ToTitleCase(TextBoxStructure.Text);
                        definitionsArray[counter, 3] = TextBoxDefinition.Text;

                        statusStripResponse.Text = "Successfully Added";

                        // only need to redisplay if an entry is added
                        ClearTextBoxes();
                        Sort();
                        DisplayArray();
                        break;
                    }
                    else { // if the row was full already, adds 1 to counter. while loop continues up to 12
                        counter++;
                        // this is here so error wont display as soon as you enter the 12th entry
                        if (definitionsArray[rowSize - 1, 0] != "~")
                        {
                            statusStripResponse.Text = "Array is Full. Please Delete or Edit an Entry to Change Content.";
                        }
                    }
                }
            }
            else {
                statusStripResponse.Text = "Please Enter Data Into All Text Fields Before Adding";
            }

            // need statusstripresponse for if array is full :/
        }

        // 9.3  Create an EDIT button that will allow the user to modify any information from the 4 text boxes into the 2D array
        // on edit, clear boxes and focus cursor on name box
        private void ButtonEdit_Click(object sender, EventArgs e)
        {
            try
            {
                int currentIndex = ListViewDisplay.SelectedIndices[0]; // get current index from mouse click
                if (currentIndex > -1) // if valid, change values to those in boxes and display
                {
                    definitionsArray[currentIndex, 0] = myTI.ToTitleCase(TextBoxName.Text);
                    definitionsArray[currentIndex, 1] = myTI.ToTitleCase(TextBoxCategory.Text);
                    definitionsArray[currentIndex, 2] = myTI.ToTitleCase(TextBoxStructure.Text);
                    definitionsArray[currentIndex, 3] = TextBoxDefinition.Text;
                }
                Sort();
                DisplayArray();
            }
            catch
            {
                statusStripResponse.Text = "Please Select An Entry To Edit";
            }
            
        }

        // 9.4 Create a DELETE button that removes all the information from a single entry of the array; the user must be prompted before the final deletion occurs
        private void ButtonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int currentIndex = ListViewDisplay.SelectedIndices[0];
                if (currentIndex > -1)
                {
                    // confirmation box settings
                    string message = "Are You Sure You Want This Delete This Entry?\nThis Action Is Permanent.";
                    string title = "Delete Definition";
                    MessageBoxButtons buttons = MessageBoxButtons.OKCancel;
                    DialogResult result;
                    result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Warning);

                    // if ok, clear those indicies of the array and redisplay
                    if (result == DialogResult.OK)
                    {
                        definitionsArray[currentIndex, 0] = "~";
                        definitionsArray[currentIndex, 1] = "~";
                        definitionsArray[currentIndex, 2] = "~";
                        definitionsArray[currentIndex, 3] = "~";

                        ClearTextBoxes();
                        Sort();
                        DisplayArray();
                    }
                }
            }
            catch
            {
                statusStripResponse.Text = "Please Select An Entry To Delete";
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
            // save dialog settings
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "dat files (*.dat) | *.dat";
            saveFileDialog.Title = "Save Dictionary";
            saveFileDialog.InitialDirectory = Application.StartupPath;
            saveFileDialog.DefaultExt = "dat";
            saveFileDialog.ShowDialog();
            string fileName = saveFileDialog.FileName;
            if(saveFileDialog.FileName != "") // if file name not empty
            {
                // save
                SaveRecord(fileName);
                statusStripResponse.Text = "File Saved";
            }
            else
            {   
                // must have a user entered filename to save and will display status when cancel or x button is hit
                statusStripResponse.Text = "File Not Saved";
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
                        // loop through array and write to file
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
            // open file dialog settings
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Application.StartupPath;
            openFileDialog.Filter = "dat files (*.dat)| *.dat";
            openFileDialog.Title = "Open File";

            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // open file and display
                OpenRecord(openFileDialog.FileName);
            }
        }

        private void OpenRecord(string openFileName)
        {
            if(File.Exists(openFileName))
            {
                using (var stream = File.Open(openFileName, FileMode.Open))
                {
                    // loop through file and write into array
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