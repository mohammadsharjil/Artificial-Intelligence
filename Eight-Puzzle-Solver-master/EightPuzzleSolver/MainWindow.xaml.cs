using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EightPuzzleSolver
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int[,] board = new int[,] { { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 } };
        private bool solvedClicked = false;
        private List<string> stepsToTake = new List<string>();
        private int stepIndex = 0;

        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void onezero_Click(object sender, RoutedEventArgs e)
        {
            swapWithtEmptyNeighbor(1, 0);
        }

        private void oneone_Click(object sender, RoutedEventArgs e)
        {
            swapWithtEmptyNeighbor(1, 1);
        }

        private void onetwo_Click(object sender, RoutedEventArgs e)
        {
            swapWithtEmptyNeighbor(1, 2);
        }

        private void twozero_Click(object sender, RoutedEventArgs e)
        {
            swapWithtEmptyNeighbor(2, 0);
        }

        private void twoone_Click(object sender, RoutedEventArgs e)
        {
            swapWithtEmptyNeighbor(2, 1);
        }

        private void twotwo_Click(object sender, RoutedEventArgs e)
        {
            swapWithtEmptyNeighbor(2, 2);
        }

        private void threezero_Click(object sender, RoutedEventArgs e)
        {
            swapWithtEmptyNeighbor(3, 0);
        }

        private void threeone_Click(object sender, RoutedEventArgs e)
        {
            swapWithtEmptyNeighbor(3, 1);
        }

        private void threetwo_Click(object sender, RoutedEventArgs e)
        {
            swapWithtEmptyNeighbor(3, 2);
        }

        private void swapWithtEmptyNeighbor(int rowNum, int colNum)
        {
            Button currentButton = getButton(rowNum, colNum);

            trySwapButtonContents(currentButton, getButton(rowNum - 1, colNum));
            trySwapButtonContents(currentButton, getButton(rowNum, colNum - 1));
            trySwapButtonContents(currentButton, getButton(rowNum + 1, colNum));
            trySwapButtonContents(currentButton, getButton(rowNum, colNum + 1));

            if (solvedClicked)
            {
                if (stepIndex < stepsToTake.Count)
                {
                    TextBlock nextStepBox = (TextBlock)this.FindName("nextStep");
                    nextStepBox.Visibility = Visibility.Visible;
                    nextStepBox.Text = stepsToTake[stepIndex++].ToUpper();
                }
                else
                {
                    solvedClicked = false;
                    stepIndex = 0;
                    ((TextBlock)this.FindName("solved")).Visibility = Visibility.Hidden;
                    ((TextBlock)this.FindName("nSteps")).Visibility = Visibility.Hidden;
                    ((TextBlock)this.FindName("nextStepText")).Visibility = Visibility.Hidden;
                    ((TextBlock)this.FindName("nextStep")).Visibility = Visibility.Hidden;
                }
            }
        }

        private bool trySwapButtonContents(Button button1, Button button2)
        {
            if (button1 == null || button2 == null)
            {
                return false;
            }

            if (0 == String.Compare(button2.Content.ToString(), ""))
            {
                button2.Content = button1.Content;
                button1.Content = "";
                return true;
            }

            return false;
        }

        private Button getButton(int rowNum, int colNum)
        {
            if (rowNum < 1 || colNum < 0 || rowNum > 3 || colNum > 2)
            {
                return null;
            }

            return GameGrid.Children.Cast<UIElement>().First(i => Grid.GetRow(i) == rowNum && Grid.GetColumn(i) == colNum) as Button;
        }

        private void randomize_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 20000; i++)
            {
                randomMove();
            }
        }

        private void randomMove()
        {
            Button emptyButton = null;
            int emptyButtonRow = -1, emptyButtonColumn = -1;
            for (int i = 1; i < 4; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    emptyButton = getButton(i, j);
                    if (String.IsNullOrEmpty(emptyButton.Content.ToString()))
                    {
                        emptyButtonRow = i;
                        emptyButtonColumn = j;
                        break;
                    }
                }
                if (emptyButtonRow != -1)
                {
                    break;
                }
            }

            bool swapSuccess = false;
            Random rnd = new Random();

            while (!swapSuccess)
            {
                int rando = rnd.Next(4);
                switch (rando)
                {
                    case (0):
                        swapSuccess = trySwapButtonContents(getButton(emptyButtonRow - 1, emptyButtonColumn), emptyButton);
                        break;
                    case (1):
                        swapSuccess = trySwapButtonContents(getButton(emptyButtonRow, emptyButtonColumn - 1), emptyButton);
                        break;
                    case (2):
                        swapSuccess = trySwapButtonContents(getButton(emptyButtonRow + 1, emptyButtonColumn), emptyButton);
                        break;
                    case (3):
                        swapSuccess = trySwapButtonContents(getButton(emptyButtonRow, emptyButtonColumn + 1), emptyButton);
                        break;
                    default:
                        break;
                }
            }
        }

        private void solve_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 1; i < 4; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    string buttonContent = getButton(i, j).Content.ToString();
                    if (String.IsNullOrEmpty(buttonContent))
                        board[i - 1, j] = 0;
                    else
                        board[i - 1, j] = int.Parse(buttonContent);
                }
            }

            ComboBox selectedAlgorithm = (ComboBox)this.FindName("selectedAlgorithm");
            ComboBox selectedHeuristic = (ComboBox)this.FindName("selectedHeuristic");
            BoardSolver.heuristic heuristicToUse;

            if (selectedHeuristic.SelectedIndex == 0)
                heuristicToUse = BoardSolver.heuristic.nMisplaced; 
            else
                heuristicToUse = BoardSolver.heuristic.manhattan;

            if (0 == String.Compare(((ComboBoxItem)selectedAlgorithm.SelectedItem).Name, "DFS"))
            {
                stepsToTake = new BoardSolver(this.board).depthFirstSearch();
            }
            if (0 == String.Compare(((ComboBoxItem)selectedAlgorithm.SelectedItem).Name, "BFS"))
            {
                stepsToTake = new BoardSolver(this.board).breadthFirstSearch();
            }
            if (0 == String.Compare(((ComboBoxItem)selectedAlgorithm.SelectedItem).Name, "GBFS"))
            {
                stepsToTake = new BoardSolver(this.board, heuristicToUse).greedyBestFirstSearch();
            }
            if (0 == String.Compare(((ComboBoxItem)selectedAlgorithm.SelectedItem).Name, "Astar"))
            {
                stepsToTake = new BoardSolver(this.board, heuristicToUse).aStarSearch();
            }

            if (stepsToTake.Count > 0)
            {
                solvedClicked = true;
                stepIndex = 0;
                ((TextBlock)this.FindName("solved")).Visibility = Visibility.Visible;
                ((TextBlock)this.FindName("nSteps")).Visibility = Visibility.Visible;
                ((TextBlock)this.FindName("nSteps")).Text = stepsToTake.Count.ToString() + " Steps";
                ((TextBlock)this.FindName("nextStepText")).Visibility = Visibility.Visible;
                ((TextBlock)this.FindName("nextStep")).Visibility = Visibility.Visible;
                ((TextBlock)this.FindName("nextStep")).Text = stepsToTake[stepIndex++].ToUpper();
            }
        }

        private void selectedAlgorithm_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox senderBox = (ComboBox)sender;
            ComboBox heuristicComboBox = (ComboBox)this.FindName("selectedHeuristic");
            TextBlock heuristicTextBlock = (TextBlock)this.FindName("Heuristic");

            if (null == heuristicComboBox || null == heuristicTextBlock)
                return;

            if (senderBox.SelectedIndex == 2 || senderBox.SelectedIndex == 3)
            {
                heuristicComboBox.Visibility = Visibility.Visible;
                heuristicTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                heuristicComboBox.Visibility = Visibility.Hidden;
                heuristicTextBlock.Visibility = Visibility.Hidden;
            }
        }
    }
}
