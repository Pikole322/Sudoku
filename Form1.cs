using System.Drawing.Text;
using System.Security.Cryptography.X509Certificates;

namespace Sudoku
{
    public partial class Form1 : Form
    {



        int[,] table;

        TextBox selected_textbox;
        public Form1()
        {
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


            //this.Controls.Add(panel1);
            //BackgroundImage = panel1.BackgroundImage;

            this.WindowState = FormWindowState.Maximized;
            InitializeComponent();
            table = new int[9, 9]{
                { 5, 4, 2, 6, 3, 9, 8, 1, 7},
                { 6, 8, 7, 5, 2, 1, 3, 9, 4},
                { 3, 9, 1, 8, 4, 7, 5, 2, 6},
                { 9, 5, 6, 4, 1, 2, 7, 8, 3},
                { 8, 1, 3, 7, 5, 6, 9, 4, 2},
                { 2, 7, 4, 3, 9, 8, 1, 6, 5},
                { 4, 6, 8, 9, 7, 5, 2, 3, 1},
                { 1, 3, 5, 2, 8, 4, 6, 7, 9},
                { 7, 2, 9, 1, 6, 3, 4, 5, 8},
            };



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

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}