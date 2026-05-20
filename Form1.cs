using System.Diagnostics;

namespace Sudoku
{
    public partial class Form1 : Form
    {



        int[,] table;

        TextBox selected_textbox;
        public Form1()
        {
            
            table = generate_table();
            //var panel1 = new Panel
            //{
            //    Dock = DockStyle.Fill,
            //    BackgroundImageLayout = ImageLayout.Stretch,
            //    BackColor = Color.White
            //};

            //string imagesDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
            //string imageFile = Path.Combine(imagesDir, "background.png");

            //if (File.Exists(imageFile))
            //{
            //    try
            //    {
            //        panel1.BackgroundImage = Image.FromFile(imageFile);
            //    }
            //    catch
            //    {
            //        panel1.BackgroundImage = null;
            //    }
            //}
            //else
            //{
            //    panel1.BackgroundImage = null;
            //}

            //InitializeComponent();
            //this.Controls.Add(panel1);
            //panel1.SendToBack();

            this.WindowState = FormWindowState.Maximized;

          


            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    textboxes[y, x] = new TextBox();
                    textboxes[y, x].SetBounds(0, 0, 30, 30);
                    textboxes[y, x].Multiline = true;
                    textboxes[y, x].TextAlign = HorizontalAlignment.Center;
                    textboxes[y, x].Font = new Font("Arial", 14, FontStyle.Regular);
                    textboxes[y, x].TextChanged += on_number_changed;
                    textboxes[y, x].Tag = new Point(x, y);
                    textboxes[y, x].Text = table[y, x].ToString();
                    this.Controls.Add(textboxes[y, x]);
                }
            }

            LayoutBoard();
            this.Resize += (s, e) => LayoutBoard();
        }

        private void LayoutBoard()
        {
            float boardProportion = 0.82f;
            int minSide = Math.Min(ClientSize.Width, ClientSize.Height);
            int computedBoxSize = Math.Max(24, (int)(minSide * boardProportion / 9f));

            int blockGap = Math.Max(8, computedBoxSize / 3);

            int boardWidth = 9 * computedBoxSize + 2 * blockGap;
            int boardHeight = 9 * computedBoxSize + 2 * blockGap;

            Point offset = new Point(
                (ClientSize.Width - boardWidth) / 2,
                (ClientSize.Height - boardHeight) / 2
            );

            float fontSize = Math.Max(12f, computedBoxSize * 0.55f);

            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    int extraX = (x / 3) * blockGap;
                    int extraY = (y / 3) * blockGap;

                    int xPos = offset.X + x * computedBoxSize + extraX;
                    int yPos = offset.Y + y * computedBoxSize + extraY;

                    textboxes[y, x].SetBounds(xPos, yPos, computedBoxSize, computedBoxSize);
                    textboxes[y, x].Font = new Font(textboxes[y, x].Font.FontFamily, fontSize, textboxes[y, x].Font.Style);
                }
            }

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }

        private void on_number_changed(object sender, EventArgs e)
        {
            var box = sender as TextBox;
            Point pos = (Point)box.Tag;

            if (!int.TryParse(box.Text, out int num))
            {
                box.BackColor = Color.White;
                selected_textbox = box;
                return;
            }

            if (table[pos.Y, pos.X] != num)
            {
                box.BackColor = Color.Red;
            }
            else
            {
                box.BackColor = Color.LightGreen;
            }

            selected_textbox = sender as TextBox;
        }

        private int[,] generate_table()
        {
            int[,] table = new int[9, 9];

            int[] rand_nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            Random rnd = new Random();

            for (int y = 0; y < 9; y++)
            {
                //rnd.Shuffle(rand_nums);
                for (int x = 0; x < 9; x++)
                {
                    foreach (int num in rand_nums)
                        if (exists_in_column(x, num, ref table) || exists_in_row(y, num, ref table) || exists_in_square(x, y, num, ref table))
                            break;
                        else
                            table[y, x] = num;
                }
            }

            return table;
            
        }

        private bool exists_in_column(int x, int num, ref int[,] table)
        {
            for (int y = 0; y < 9; y++)
            {
                if (table[y, x] == num)
                    return true;
            }
            return false;
        }

        private bool exists_in_row(int y, int num, ref int[,] table)
        {
            for (int x = 0; x < 9; x++)
            {
                if (table[y, x] == num)
                    return true;
            }
            return false;
        }

        private bool exists_in_square(int cell_x, int cell_y, int num, ref int[,] table)
        {
            //if (num == 8)
            //    Debug.Assert(false);
            int square_x = cell_x / 3;
            int square_y = cell_y / 3;

            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    if (table[square_y * 3 + y, square_x * 3 + x] == num)
                        return true;
                }
            }
            return false;
        }
    }
}