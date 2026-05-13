namespace Sudoku
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        
            int box_size = 30;
            Point offset = new Point(ClientSize.Width / 2 - 9 * box_size / 2, ClientSize.Height / 2 - 9 * box_size / 2);
            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    textboxes[y, x] = new TextBox();
                    textboxes[y, x].SetBounds(x * box_size + offset.X, y * box_size + offset.Y, box_size, box_size);
                    textboxes[y, x].Multiline = true;
                    this.Controls.Add(textboxes[y, x]);
                }
            
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            //textbox.Invalidate();

            
        }
    }
}
