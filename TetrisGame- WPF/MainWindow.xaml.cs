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

namespace TetrisGame__WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
         private readonly ImageSource[] tileImages = new ImageSource[]
            {
              new BitmapImage(new Uri("Assets/TileEmpty.png",UriKind.Relative)),
              new BitmapImage(new Uri("Assets/TileCyan.png",UriKind.Relative)),
              new BitmapImage(new Uri("Assets/TileBlue.png",UriKind.Relative)),
              new BitmapImage(new Uri("Assets/TileOrange.png",UriKind.Relative)),
              new BitmapImage(new Uri("Assets/TileYellow.png",UriKind.Relative)),
              new BitmapImage(new Uri("Assets/TileGreen.png",UriKind.Relative)),
              new BitmapImage(new Uri("Assets/TilePurple.png",UriKind.Relative)),
              new BitmapImage(new Uri("Assets/TileRed.png",UriKind.Relative))

            };

        private readonly ImageSource[] blockImages = new ImageSource[]
            {
                new BitmapImage(new Uri("Assets/Block-Empty.png",UriKind.Relative)),
              new BitmapImage(new Uri("Assets/Block-I.png",UriKind.Relative)),
              new BitmapImage(new Uri("Assets/Block-J.png",UriKind.Relative)),
              new BitmapImage(new Uri("Assets/Block-L.png",UriKind.Relative)),
              new BitmapImage(new Uri("Assets/Block-O.png",UriKind.Relative)),
              new BitmapImage(new Uri("Assets/Block-S.png",UriKind.Relative)),
              new BitmapImage(new Uri("Assets/Block-T.png",UriKind.Relative)),
              new BitmapImage(new Uri("Assets/Block-Z.png",UriKind.Relative))

            };

        private readonly Image[,] imageControls;

        private GameState gameState = new GameState();

        private const int MAXDELAY = 1000;
        private const int MINDELAY = 75;
        private const int DELAYDECREASE = 25;

        public MainWindow()
        {
            InitializeComponent();
            imageControls = SetupGameCanvas(gameState.GameGrid);
        }

        private Image[,] SetupGameCanvas(Grid grid)
        {
            Image[,] imageControls = new Image[grid.Rows, grid.Cols];
            int cellsize = 25;

            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Cols; c++)
                {
                    Image imageControl = new Image
                    {
                        Width = cellsize,
                        Height = cellsize
                    };
                    Canvas.SetTop(imageControl, (r - 2) * cellsize + 10);
                    Canvas.SetLeft(imageControl, c * cellsize);
                    GameCanvas.Children.Add(imageControl);
                    imageControls[r, c] = imageControl;
                }    
            }
            return imageControls;
           
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameState.GameOver) return;
            switch (e.Key)
            {
                case Key.Left:
                    { gameState.MoveBlockLeft(); break; }
                case Key.Right:
                    { gameState.MoveBlockRight(); break; }
                case Key.Down:
                    { gameState.RotateBlockCCW(); break; }
                case Key.Up:
                    { gameState.RotateBlockCW(); break; }
                case Key.Tab:
                    { gameState.HoldBlock(); break; }
                case Key.Space:
                    { gameState.DropBlock(); break; }

                default:
                    return;//only redraw if a key pressed does somthing

              
            }
            Draw(gameState);

        }
        private async void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            await GameLoop();
        }

        private async void PlayAgain_Click(object sender, RoutedEventArgs e)
        {
            gameState = new GameState();
            GameOverMenu.Visibility = Visibility.Hidden;
            await GameLoop();
        }
        private void DrawHeldBlock(Block heldBlock)
        {
            if(heldBlock==null)
            {
                HoldImage.Source = blockImages[0];
            }
            else
            {
                HoldImage.Source = blockImages[heldBlock.ID];
            }
        }

        private async Task GameLoop()
        {
            Draw(gameState);

            while(!gameState.GameOver)
            {
                int delay = Math.Max(MINDELAY, MAXDELAY - (gameState.Score * DELAYDECREASE));
                await Task.Delay(delay);
                gameState.MoveBlockDown();
                Draw(gameState);
            }

            GameOverMenu.Visibility = Visibility.Visible;
            FinalScoreText.Text = $"Score: {gameState.Score}";
        }
        private void DrawGrid(Grid grid)
        {
            for(int r=0; r<grid.Rows;r++)
            {
                for(int c=0; c< grid.Cols;c++)
                {
                    int id = grid[r, c];
                    imageControls[r, c].Opacity = 1;
                    imageControls[r, c].Source = tileImages[id];
                }
            }
        }

        private void DrawBlock(Block block)
        {
            foreach(Position p in block.TilePositions())
            {
                imageControls[p.Row, p.Col].Opacity = 1;
                imageControls[p.Row, p.Col].Source = tileImages[block.ID];
            }
        }

        private void Draw(GameState gameState)
        {
            DrawGrid(gameState.GameGrid);
            DrawGhostBlock(gameState.CurrentBlock);
            DrawBlock(gameState.CurrentBlock);
            DrawNextBlock(gameState.BlockQueue);
            DrawHeldBlock( gameState.HeldBlock);
            ScoreText.Text = $"Score: {gameState.Score}"; //$ is variable 
        }

        private void DrawNextBlock(BlockQueue blockQueue)
        {
            Block next = blockQueue.NextBlock;
            NextImage.Source = blockImages[next.ID];
        }

        private void DrawGhostBlock(Block block)
        {
            int dropDistance = gameState.BlockDropDistance();
            foreach( Position p in block.TilePositions())
            {

                imageControls[p.Row + dropDistance, p.Col].Opacity = 0.25;
                imageControls[p.Row + dropDistance, p.Col].Source = tileImages[block.ID];
            }

        }
      
    }
}
