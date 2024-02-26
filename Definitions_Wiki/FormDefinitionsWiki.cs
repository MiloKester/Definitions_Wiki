using System;
using System.Windows.Forms;

namespace Definitions_Wiki
{
    public partial class FormDefinitionsWiki : Form
    {
        public FormDefinitionsWiki()
        {
            InitializeComponent();
        }

        // TO DO:
        // exception handling for if you add more than 12 entries to list
        // find out proper capitalisation conventions and switch everything to that

        // 9.1 Create a global 2D string array, use static variable for the dimensions (row = 12, column = 4)

        static int rowSize = 12;
        static int columnSize = 4; // name, category, structure, definition
        string[,] definitionsArray = new string[rowSize, columnSize];
        int ptr = 0; // points to next empty row

        #region Utils

        private void FormDefinitionsWiki_Load(object sender, EventArgs e)
        {
            InitialiseArray();
            DisplayArray();
            Sort();
        }

        private void InitialiseArray()
        {
            Random rnd = new Random();
            for (int i = 0; i < rowSize; i++)
            {
                definitionsArray[i, 0] = rnd.Next(10, 99).ToString();
                definitionsArray[i, 1] = "-";
                // definitionsArray[i, 2] = "";
                // definitionsArray[i, 3] = ""; dont need?
            }
        }

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
        private void ButtonAdd_Click(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(TextBoxName.Text) && !string.IsNullOrEmpty(TextBoxCategory.Text) && !string.IsNullOrEmpty(TextBoxStructure.Text) && !string.IsNullOrEmpty(TextBoxDefinition.Text))
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
        }

        #endregion

        // 9.5 Create a CLEAR method to clear the four text boxes so a new definition can be added
        private void ClearTextBoxes()
        {
            TextBoxName.Clear();
            TextBoxCategory.Clear();
            TextBoxStructure.Clear();
            TextBoxDefinition.Clear();
            TextBoxName.Focus();
        }

        private void ButtonEdit_Click(object sender, EventArgs e)
        {

        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {

        }

        private void ButtonDelete_Click(object sender, EventArgs e)
        {

        }

        private void ButtonLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = "c:\\Documents\\";
            openFileDialog.Filter = "dat files (*.dat)|*.dat";
            openFileDialog.RestoreDirectory = true;

            openFileDialog.ShowDialog();
        }

        private void TextBoxName_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ClearTextBoxes();

        }

        private void ListViewDisplay_MouseClick(object sender, MouseEventArgs e)
        {
            // try catch 
            int currentIndex = ListViewDisplay.SelectedIndices[0];
            if(currentIndex > -1)
            {
                TextBoxName.Text = definitionsArray[currentIndex, 0];
                TextBoxCategory.Text = definitionsArray[currentIndex, 1];
                TextBoxStructure.Text = definitionsArray[currentIndex, 2];
                TextBoxDefinition.Text = definitionsArray[currentIndex, 3];
            }
        }


    }
}
