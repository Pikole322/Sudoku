using System;
using System.Drawing;
using System.Windows.Forms;

namespace Sudoku
{
    public partial class Form1 : Form
    {
        int[,] table;

        TextBox selected_textbox;
        //Menu kontroler
        public class Menu
        {
            private ComboBox comboDifficulty;
            private Button btnStart;
            private Label lblDifficulty;
            private Label lblCreatedBy;
            private Control[] controls;
            private const int controlWidth = 250;
            private const int buttonHeight = 70;
            private const int marginRight = 20;
            private const int marginLeft = 10;
            private const int marginBottom = 10;
            private const int topOffset = 20;
            private const int spacing = 12;

            public void Attach(Form mainForm, Action<int> onStart)
            {
                lblDifficulty = new Label
                {
                    Text = "Poziom trudnoœci:",
                    AutoSize = true,
                    Font = new Font("Segoe UI", 14f, FontStyle.Regular),
                };

                lblCreatedBy = new Label
                {
                    Text = "Created by: Rafa³/Bartosz",
                    AutoSize = true,
                    Font = new Font("Segoe UI", 14f, FontStyle.Regular),
                    ForeColor = Color.Black
                };

                comboDifficulty = new ComboBox
                {
                    Width = controlWidth,
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Font = new Font("Segoe UI", 14f, FontStyle.Regular),
                    IntegralHeight = true
                };
                comboDifficulty.Items.AddRange(new object[] { "£atwy", "Œredni", "Trudny" });
                comboDifficulty.SelectedIndex = 1;

                btnStart = new Button
                {
                    Text = "Start gry",
                    Width = controlWidth,
                    Height = buttonHeight,
                    Font = new Font("Segoe UI", 16f, FontStyle.Bold)
                };

                btnStart.Click += (s, e) =>
                {
                    int difficulty = Math.Max(0, comboDifficulty.SelectedIndex) + 1;
                    onStart?.Invoke(difficulty);
                };

                mainForm.Controls.Add(lblDifficulty);
                mainForm.Controls.Add(lblCreatedBy);
                mainForm.Controls.Add(comboDifficulty);
                mainForm.Controls.Add(btnStart);

                controls = new Control[] { lblDifficulty, comboDifficulty, btnStart, lblCreatedBy };

                PositionControls(mainForm);
                // kotwiczymy menu w prawym górnym rogu, a podpis w lewym dolnym
                lblDifficulty.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                comboDifficulty.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                btnStart.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                lblCreatedBy.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;

                mainForm.Resize += (s, e) => PositionControls(mainForm);
            }

            private void PositionControls(Form mainForm)
            {
                // Pozycjonowanie prawego menu (label + combo + przycisk)
                int leftRight = Math.Max(0, mainForm.ClientSize.Width - controlWidth - marginRight);
                int currentTop = topOffset;

                lblDifficulty.Left = leftRight;
                lblDifficulty.Top = currentTop;
                lblDifficulty.MaximumSize = new Size(controlWidth, 0);

                currentTop = lblDifficulty.Bottom + spacing;
                comboDifficulty.Left = leftRight;
                comboDifficulty.Top = currentTop;
                comboDifficulty.Height = 40;

                currentTop = comboDifficulty.Bottom + spacing;
                btnStart.Left = leftRight;
                btnStart.Top = currentTop;

                // Pozycjonowanie podpisu w lewym dolnym rogu
                lblCreatedBy.MaximumSize = new Size(mainForm.ClientSize.Width - marginLeft * 2, 0);
                lblCreatedBy.Left = marginLeft;
                // ustaw top tak, aby znalaz³ siê nad dolnym marginesem
                lblCreatedBy.Top = Math.Max(0, mainForm.ClientSize.Height - marginBottom - lblCreatedBy.Height);
            }
        }

        public Form1()
        {
            this.WindowState = FormWindowState.Maximized;
            InitializeComponent();

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
                    textboxes[y, x].Text = string.Empty;
                    this.Controls.Add(textboxes[y, x]);
                }
            }

            var menu = new Menu();
            menu.Attach(this, (difficulty) => gra(difficulty));

            LayoutBoard();
            this.Resize += (s, e) => LayoutBoard();
        }

        private void gra(int difficulty)
        {
            table = generate_table();

            int[,] puzzle = (int[,])table.Clone();
            Random rnd = new Random();

            int clues;
            switch (difficulty)
            {
                case 1: clues = 36; break;
                case 2: clues = 28; break;
                case 3: clues = 22; break;
                default: clues = 28; break;
            }

            int totalCells = 9 * 9;
            int cellsToClear = totalCells - clues;

            while (cellsToClear > 0)
            {
                int rx = rnd.Next(0, 9);
                int ry = rnd.Next(0, 9);
                if (puzzle[ry, rx] != 0)
                {
                    puzzle[ry, rx] = 0;
                    cellsToClear--;
                }
            }

            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    if (puzzle[y, x] != 0)
                    {
                        textboxes[y, x].Text = puzzle[y, x].ToString();
                        textboxes[y, x].ReadOnly = true;
                        textboxes[y, x].BackColor = Color.LightGray;
                        textboxes[y, x].ForeColor = Color.Black;
                    }
                    else
                    {
                        textboxes[y, x].Text = string.Empty;
                        textboxes[y, x].ReadOnly = false;
                        textboxes[y, x].BackColor = Color.White;
                        textboxes[y, x].ForeColor = Color.Black;
                    }
                }
            }

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

            if (table == null)
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
                        if (!exists_in_column(x, num, ref table) && !exists_in_row(y, num, ref table) && !exists_in_square(x, y, num, ref table))
                        {
                            table[y, x] = num;
                            break;
                        }
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

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}