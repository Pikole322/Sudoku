namespace Sudoku
{
    public partial class Form1 : Form
    {
        int[,] table;
        public Form1()
        {
            InitializeComponent();

            table = new int [9, 9]{
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
 

            int box_size = 30;
            Point offset = new Point(ClientSize.Width / 2 - 9 * box_size / 2, ClientSize.Height / 2 - 9 * box_size / 2);
            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    textboxes[y, x] = new TextBox();
                    textboxes[y, x].SetBounds(x * box_size + offset.X, y * box_size + offset.Y, box_size, box_size);
                    textboxes[y, x].Multiline = true;
                    textboxes[y, x].TextAlign = HorizontalAlignment.Center;
                    textboxes[y, x].Font = new Font("Arial", 14, FontStyle.Regular);
                    textboxes[y, x].TextChanged += on_number_changed;
                    
                 
                    
                    this.Controls.Add(textboxes[y, x]);
                }
            
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            //textbox.Invalidate();

            
        }

        private void on_number_changed(object sender, EventArgs e)
        {
            TextBox box = (TextBox)sender;
            box.BackColor = Color.Red;
        }
    }
}
